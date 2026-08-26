using System.Text;

namespace MudBlazor;

/// <summary>
/// Highlighter for YAML.<br/>
/// Mapping keys are rendered as attributes, quoted/scalar strings as strings, numbers as numbers,
/// <c>true</c>/<c>false</c>/<c>null</c>/<c>~</c> (and yes/no/on/off) as literals, <c>#</c> comments as
/// comments, document markers (<c>---</c>/<c>...</c>) and tags as meta/type, and anchors/aliases as
/// symbols. Does not inherit <see cref="CodeHighlighterBase"/> - YAML is line-oriented, not a token
/// stream of keywords.
/// </summary>
internal sealed class YamlCodeHighlighter : ICodeHighlighter
{
	public IReadOnlyList<CodeNode> Highlight(string code)
	{
		var nodes = new List<CodeNode>();
		var text = new StringBuilder();
		var i = 0;

		while (i < code.Length)
		{
			// Leading indentation stays plain.
			while (i < code.Length && code[i] is ' ' or '\t')
				text.Append(code[i++]);

			if (i >= code.Length)
				break;

			var c = code[i];

			if (c is '\n' or '\r')
			{
				text.Append(c);
				i++;
				continue;
			}

			if (c == '#')
			{
				ReadComment();
				continue;
			}

			// Document markers: --- (start) and ... (end).
			if ((Matches(code, i, "---") || Matches(code, i, "...")) && IsBoundary(code, i + 3))
			{
				Flush();
				nodes.Add(new CodeSpan("hljs-meta", [new CodeText(code[i..(i + 3)])]));
				i += 3;
				ScanValue();
				continue;
			}

			// Leading list bullets: "- " (possibly several, e.g. "- - item").
			while (i < code.Length && code[i] == '-' && (i + 1 >= code.Length || code[i + 1] is ' ' or '\n' or '\r'))
			{
				text.Append('-');
				i++;
				while (i < code.Length && code[i] == ' ')
					text.Append(code[i++]);
			}

			if (i >= code.Length || code[i] is '\n' or '\r')
				continue;

			if (code[i] == '#')
			{
				ReadComment();
				continue;
			}

			ReadKeyThenValue();
		}

		Flush();
		return nodes;

		void Flush()
		{
			if (text.Length == 0)
				return;

			nodes.Add(new CodeText(text.ToString()));
			text.Clear();
		}

		// Reads a "# ..." comment from code[i] to the end of the line.
		void ReadComment()
		{
			Flush();
			var start = i;
			while (i < code.Length && code[i] is not ('\n' or '\r'))
				i++;

			nodes.Add(new CodeSpan("hljs-comment", [new CodeText(code[start..i])]));
		}

		// Emits a "key:" as an attribute (quoted keys as strings), then scans the value that follows.
		void ReadKeyThenValue()
		{
			var colon = FindKeyColon(code, i);
			if (colon < 0)
			{
				ScanValue();
				return;
			}

			var keyEnd = colon;
			while (keyEnd > i && code[keyEnd - 1] is ' ' or '\t')
				keyEnd--;

			Flush();
			var keyClass = code[i] is '"' or '\'' ? "hljs-string" : "hljs-attr";
			nodes.Add(new CodeSpan(keyClass, [new CodeText(code[i..keyEnd])]));

			// Whitespace between the key and the colon, then the colon itself, stay plain.
			text.Append(code[keyEnd..(colon + 1)]);
			i = colon + 1;

			ScanValue();
		}

		// Scans the remainder of the current line as a value region (scalars, quoted strings, flow
		// collections, anchors/aliases, tags and inline comments).
		void ScanValue()
		{
			while (i < code.Length && code[i] is not ('\n' or '\r'))
			{
				var c = code[i];

				if (c is ' ' or '\t')
				{
					text.Append(c);
					i++;
					continue;
				}

				// Inline comment: '#' preceded by whitespace.
				if (c == '#' && i > 0 && code[i - 1] is ' ' or '\t')
				{
					ReadComment();
					return;
				}

				if (c is '"' or '\'')
				{
					ReadQuoted(c);
					continue;
				}

				// Anchor (&name) or alias (*name).
				if (c is '&' or '*' && i + 1 < code.Length && (char.IsLetterOrDigit(code[i + 1]) || code[i + 1] == '_'))
				{
					Flush();
					var s = i++;
					while (i < code.Length && (char.IsLetterOrDigit(code[i]) || code[i] is '-' or '_'))
						i++;

					nodes.Add(new CodeSpan("hljs-symbol", [new CodeText(code[s..i])]));
					continue;
				}

				// Tag: !!str, !Foo.
				if (c == '!')
				{
					Flush();
					var s = i;
					while (i < code.Length && code[i] is not (' ' or '\t' or '\n' or '\r'))
						i++;

					nodes.Add(new CodeSpan("hljs-type", [new CodeText(code[s..i])]));
					continue;
				}

				// Flow punctuation.
				if (c is '{' or '}' or '[' or ']' or ',' or ':')
				{
					text.Append(c);
					i++;
					continue;
				}

				// A plain scalar token: number, literal or plain text.
				var tokenStart = i;
				while (i < code.Length && code[i] is not (' ' or '\t' or '\n' or '\r' or ',' or '{' or '}' or '[' or ']'))
					i++;

				var token = code[tokenStart..i];

				if (IsNumber(token))
				{
					Flush();
					nodes.Add(new CodeSpan("hljs-number", [new CodeText(token)]));
				}
				else if (IsLiteral(token))
				{
					Flush();
					nodes.Add(new CodeSpan("hljs-literal", [new CodeText(token)]));
				}
				else
				{
					text.Append(token);
				}
			}
		}

		// Reads a quoted scalar starting at the quote code[i] to the end of the line at the latest.
		void ReadQuoted(char quote)
		{
			Flush();
			var start = i;
			i++; // opening quote

			while (i < code.Length && code[i] is not ('\n' or '\r'))
			{
				if (quote == '"' && code[i] == '\\' && i + 1 < code.Length)
				{
					i += 2;
					continue;
				}

				if (code[i] == quote)
				{
					// In single-quoted scalars '' is an escaped quote.
					if (quote == '\'' && i + 1 < code.Length && code[i + 1] == '\'')
					{
						i += 2;
						continue;
					}

					i++;
					break;
				}

				i++;
			}

			nodes.Add(new CodeSpan("hljs-string", [new CodeText(code[start..i])]));
		}
	}

	// Returns the index of the ':' that ends a mapping key (a ':' followed by whitespace or end of line),
	// or -1 when the line has none before its end or an inline comment. Quoted scalars are skipped.
	private static int FindKeyColon(string code, int from)
	{
		var j = from;
		while (j < code.Length && code[j] is not ('\n' or '\r'))
		{
			var c = code[j];

			if (c is '"' or '\'')
			{
				j++;
				while (j < code.Length && code[j] is not ('\n' or '\r'))
				{
					if (c == '"' && code[j] == '\\')
					{
						j += 2;
						continue;
					}

					if (code[j] == c)
					{
						j++;
						break;
					}

					j++;
				}

				continue;
			}

			if (c == '#' && j > from && code[j - 1] is ' ' or '\t')
				return -1;

			if (c == ':' && (j + 1 >= code.Length || code[j + 1] is ' ' or '\n' or '\r'))
				return j;

			j++;
		}

		return -1;
	}

	private static bool IsNumber(string s)
	{
		if (s.Length == 0)
			return false;

		var k = 0;
		if (s[k] is '-' or '+')
			k++;

		if (k >= s.Length)
			return false;

		// Hexadecimal.
		if (k + 1 < s.Length && s[k] == '0' && s[k + 1] is 'x' or 'X')
		{
			k += 2;
			if (k >= s.Length)
				return false;

			for (; k < s.Length; k++)
				if (!Uri.IsHexDigit(s[k]))
					return false;

			return true;
		}

		var anyDigit = false;
		for (; k < s.Length && char.IsAsciiDigit(s[k]); k++)
			anyDigit = true;

		if (k < s.Length && s[k] == '.')
		{
			k++;
			for (; k < s.Length && char.IsAsciiDigit(s[k]); k++)
				anyDigit = true;
		}

		if (!anyDigit)
			return false;

		if (k < s.Length && s[k] is 'e' or 'E')
		{
			k++;
			if (k < s.Length && s[k] is '+' or '-')
				k++;

			var anyExp = false;
			for (; k < s.Length && char.IsAsciiDigit(s[k]); k++)
				anyExp = true;

			if (!anyExp)
				return false;
		}

		return k == s.Length;
	}

	private static bool IsLiteral(string s)
	{
		if (s == "~")
			return true;

		const StringComparison ic = StringComparison.OrdinalIgnoreCase;
		return s.Equals("true", ic) || s.Equals("false", ic) || s.Equals("null", ic) ||
		       s.Equals("yes", ic) || s.Equals("no", ic) || s.Equals("on", ic) || s.Equals("off", ic);
	}

	private static bool IsBoundary(string code, int i) =>
		i >= code.Length || code[i] is ' ' or '\t' or '\n' or '\r';

	private static bool Matches(string code, int i, string token) =>
		i + token.Length <= code.Length && string.CompareOrdinal(code, i, token, 0, token.Length) == 0;
}
