using System.Text;

namespace MudBlazor;

/// <summary>
/// Highlighter for JSON.<br/>
/// Object keys are rendered as attributes, string values as strings, numbers as numbers and
/// <c>true</c>/<c>false</c>/<c>null</c> as literals; structural punctuation stays plain. Does not
/// inherit <see cref="CodeHighlighterBase"/> - JSON is a data format, not a token stream of keywords.
/// </summary>
internal sealed class JsonCodeHighlighter : ICodeHighlighter
{
	public IReadOnlyList<CodeNode> Highlight(string code)
	{
		var nodes = new List<CodeNode>();
		var text = new StringBuilder();
		var i = 0;

		while (i < code.Length)
		{
			var c = code[i];

			if (c == '"')
			{
				Flush();
				var start = i;
				ReadString(code, ref i);

				// A string immediately followed (past whitespace) by ':' is an object key.
				var className = IsKey(code, i) ? "hljs-attr" : "hljs-string";
				nodes.Add(new CodeSpan(className, [new CodeText(code[start..i])]));
				continue;
			}

			if (char.IsAsciiDigit(c) || (c == '-' && i + 1 < code.Length && char.IsAsciiDigit(code[i + 1])))
			{
				Flush();
				var start = i;
				ReadNumber(code, ref i);
				nodes.Add(new CodeSpan("hljs-number", [new CodeText(code[start..i])]));
				continue;
			}

			if (char.IsLetter(c))
			{
				var start = i;
				while (i < code.Length && char.IsLetter(code[i]))
					i++;

				var word = code[start..i];
				if (word is "true" or "false" or "null")
				{
					Flush();
					nodes.Add(new CodeSpan("hljs-literal", [new CodeText(word)]));
				}
				else
				{
					text.Append(word);
				}

				continue;
			}

			text.Append(c);
			i++;
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
	}

	// True when the next non-whitespace character at code[i] is ':' (the string just read is a key).
	private static bool IsKey(string code, int i)
	{
		while (i < code.Length && char.IsWhiteSpace(code[i]))
			i++;

		return i < code.Length && code[i] == ':';
	}

	// Advances i past a JSON string starting at the quote code[i].
	private static void ReadString(string code, ref int i)
	{
		i++; // opening quote

		while (i < code.Length)
		{
			var ch = code[i];

			if (ch == '\\' && i + 1 < code.Length)
			{
				i += 2;
				continue;
			}

			if (ch == '"')
			{
				i++;
				break;
			}

			i++;
		}
	}

	// Advances i past a JSON number starting at code[i].
	private static void ReadNumber(string code, ref int i)
	{
		if (code[i] == '-')
			i++;

		while (i < code.Length && char.IsAsciiDigit(code[i]))
			i++;

		if (i < code.Length && code[i] == '.')
		{
			i++;
			while (i < code.Length && char.IsAsciiDigit(code[i]))
				i++;
		}

		if (i < code.Length && code[i] is 'e' or 'E')
		{
			i++;
			if (i < code.Length && code[i] is '+' or '-')
				i++;

			while (i < code.Length && char.IsAsciiDigit(code[i]))
				i++;
		}
	}
}
