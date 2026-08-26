using System.Text;

namespace MudBlazor;

/// <summary>
/// Highlighter for XML.<br/>
/// Tags, tag names, attributes and their quoted values are tokenized, along with comments, the XML
/// prolog / DOCTYPE (meta) and CDATA sections; text content stays plain. Does not inherit
/// <see cref="CodeHighlighterBase"/> - XML is markup, not a token stream of keywords.
/// </summary>
internal sealed class XmlCodeHighlighter : ICodeHighlighter
{
	public IReadOnlyList<CodeNode> Highlight(string code)
	{
		var nodes = new List<CodeNode>();
		var text = new StringBuilder();
		var i = 0;

		while (i < code.Length)
		{
			if (code[i] != '<')
			{
				text.Append(code[i]);
				i++;
				continue;
			}

			Flush();

			if (Matches(code, i, "<!--"))
				nodes.Add(ReadUntil(code, ref i, "-->", "hljs-comment"));
			else if (Matches(code, i, "<![CDATA["))
				nodes.Add(ReadUntil(code, ref i, "]]>", "hljs-meta"));
			else if (Matches(code, i, "<?"))
				nodes.Add(ReadUntil(code, ref i, "?>", "hljs-meta"));
			else if (Matches(code, i, "<!"))
				nodes.Add(ReadUntil(code, ref i, ">", "hljs-meta")); // DOCTYPE and other declarations
			else
				nodes.Add(ReadTag(code, ref i));
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

	// Reads from code[i] up to and including the terminator, as a single span of the given class.
	private static CodeSpan ReadUntil(string code, ref int i, string terminator, string className)
	{
		var start = i;
		while (i < code.Length && !Matches(code, i, terminator))
			i++;

		if (i < code.Length)
			i += terminator.Length;

		return new CodeSpan(className, [new CodeText(code[start..i])]);
	}

	// Reads a tag starting at code[i] == '<' (opening, closing or self-closing) up to and including '>'.
	private static CodeSpan ReadTag(string code, ref int i)
	{
		var children = new List<CodeNode>();
		var raw = new StringBuilder();

		raw.Append(code[i++]); // '<'
		if (i < code.Length && code[i] == '/')
			raw.Append(code[i++]); // '/'

		// Tag name
		if (i < code.Length && (char.IsLetter(code[i]) || code[i] == '_'))
		{
			var start = i;
			while (i < code.Length && (char.IsLetterOrDigit(code[i]) || code[i] is '-' or '.' or '_' or ':'))
				i++;

			FlushRaw();
			children.Add(new CodeSpan("hljs-name", [new CodeText(code[start..i])]));
		}

		// Attributes
		while (i < code.Length && code[i] != '>')
		{
			var c = code[i];

			if (c is '"' or '\'')
			{
				FlushRaw();
				children.Add(ReadString(code, ref i, c));
				continue;
			}

			if (char.IsLetter(c) || c is '_' or ':')
			{
				var start = i;
				while (i < code.Length && (char.IsLetterOrDigit(code[i]) || code[i] is '-' or '_' or ':' or '.'))
					i++;

				FlushRaw();
				children.Add(new CodeSpan("hljs-attr", [new CodeText(code[start..i])]));
				continue;
			}

			raw.Append(c);
			i++;
		}

		if (i < code.Length && code[i] == '>')
			raw.Append(code[i++]);

		FlushRaw();
		return new CodeSpan("hljs-tag", children);

		void FlushRaw()
		{
			if (raw.Length == 0)
				return;

			children.Add(new CodeText(raw.ToString()));
			raw.Clear();
		}
	}

	// Reads a quoted attribute value starting at the quote code[i].
	private static CodeSpan ReadString(string code, ref int i, char quote)
	{
		var start = i;
		i++; // opening quote

		while (i < code.Length && code[i] != quote)
			i++;

		if (i < code.Length)
			i++; // closing quote

		return new CodeSpan("hljs-string", [new CodeText(code[start..i])]);
	}

	private static bool Matches(string code, int i, string token) =>
		i + token.Length <= code.Length && string.CompareOrdinal(code, i, token, 0, token.Length) == 0;
}
