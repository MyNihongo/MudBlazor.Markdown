using System.Text;

namespace MudBlazor;

/// <summary>
/// Handles line/block comments, strings, numbers, keywords, types and (for languages that declare
/// <see cref="FunctionKeywords"/>) function declarations rendered as nested
/// function / title / params spans.
/// </summary>
internal abstract class CodeHighlighterBase : ICodeHighlighter
{
	private static readonly IReadOnlySet<string> EmptySet = new HashSet<string>();
	private static readonly IReadOnlyList<string> DefaultLineComments = ["//"];
	private static readonly IReadOnlyList<(string, string)> DefaultBlockComments = [("/*", "*/")];
	private static readonly IReadOnlyList<char> DefaultStringQuotes = ['"', '\''];

	/// <summary>Reserved words rendered as keyword tokens.</summary>
	protected abstract IReadOnlySet<string> Keywords { get; }

	/// <summary>Built-in type names rendered as type tokens.</summary>
	protected virtual IReadOnlySet<string> Types => EmptySet;

	/// <summary>Literals (e.g. <c>true</c>, <c>null</c>) rendered as literal tokens.</summary>
	protected virtual IReadOnlySet<string> Literals => EmptySet;

	/// <summary>Keywords that introduce a function declaration (e.g. <c>func</c>, <c>fun</c>).</summary>
	protected virtual IReadOnlySet<string> FunctionKeywords => EmptySet;

	/// <summary>Prefixes that start a single-line comment.</summary>
	protected virtual IReadOnlyList<string> LineComments => DefaultLineComments;

	/// <summary>Delimiter pairs that start/end a block comment.</summary>
	protected virtual IReadOnlyList<(string Start, string End)> BlockComments => DefaultBlockComments;

	/// <summary>Characters that open/close a string or character literal.</summary>
	protected virtual IReadOnlyList<char> StringQuotes => DefaultStringQuotes;

	/// <summary>Character that opens/closes a raw string with no escaping (e.g. Go back-tick strings).</summary>
	protected virtual char? RawStringQuote => null;

	public IReadOnlyList<CodeNode> Highlight(string code)
	{
		var nodes = new List<CodeNode>();
		var text = new StringBuilder();
		var i = 0;

		while (i < code.Length)
		{
			if (TryReadLineComment() || TryReadBlockComment())
				continue;

			var c = code[i];

			if (RawStringQuote is { } rawQuote && c == rawQuote)
			{
				FlushText();
				var start = i++;
				while (i < code.Length && code[i] != rawQuote)
					i++;
				if (i < code.Length)
					i++;

				nodes.Add(new CodeSpan("hljs-string", [new CodeText(code[start..i])]));
				continue;
			}

			if (StringQuotes.Contains(c))
			{
				FlushText();
				nodes.Add(ReadString(code, ref i, c));
				continue;
			}

			if (char.IsDigit(c))
			{
				FlushText();
				nodes.Add(ReadNumber(code, ref i));
				continue;
			}

			if (IsIdentifierStart(c))
			{
				var start = i;
				while (i < code.Length && IsIdentifierPart(code[i]))
					i++;

				var word = code[start..i];

				if (FunctionKeywords.Contains(word))
				{
					FlushText();
					nodes.Add(ReadFunction(code, word, ref i));
				}
				else if (Keywords.Contains(word))
				{
					FlushText();
					nodes.Add(new CodeSpan("hljs-keyword", [new CodeText(word)]));
				}
				else if (Literals.Contains(word))
				{
					FlushText();
					nodes.Add(new CodeSpan("hljs-literal", [new CodeText(word)]));
				}
				else if (Types.Contains(word))
				{
					FlushText();
					nodes.Add(new CodeSpan("hljs-type", [new CodeText(word)]));
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

		FlushText();
		return nodes;

		void FlushText()
		{
			if (text.Length == 0)
				return;

			nodes.Add(new CodeText(text.ToString()));
			text.Clear();
		}

		bool TryReadLineComment()
		{
			foreach (var prefix in LineComments)
			{
				if (!Matches(code, i, prefix))
					continue;

				FlushText();
				var start = i;
				while (i < code.Length && code[i] != '\n')
					i++;

				nodes.Add(new CodeSpan("hljs-comment", [new CodeText(code[start..i])]));
				return true;
			}

			return false;
		}

		bool TryReadBlockComment()
		{
			foreach (var (open, close) in BlockComments)
			{
				if (!Matches(code, i, open))
					continue;

				FlushText();
				var start = i;
				i += open.Length;
				while (i < code.Length && !Matches(code, i, close))
					i++;

				if (i < code.Length)
					i += close.Length;

				nodes.Add(new CodeSpan("hljs-comment", [new CodeText(code[start..i])]));
				return true;
			}

			return false;
		}
	}

	private CodeSpan ReadFunction(string code, string keyword, ref int i)
	{
		var children = new List<CodeNode> { new CodeSpan("hljs-keyword", [new CodeText(keyword)]) };

		AppendWhitespace(code, ref i, children);

		// Function name
		if (i < code.Length && IsIdentifierStart(code[i]))
		{
			var start = i;
			while (i < code.Length && IsIdentifierPart(code[i]))
				i++;

			children.Add(new CodeSpan("hljs-title", [new CodeText(code[start..i])]));
		}

		AppendWhitespace(code, ref i, children);

		// Parameters
		if (i < code.Length && code[i] == '(')
			children.Add(ReadParams(code, ref i));

		return new CodeSpan("hljs-function", children);
	}

	private CodeSpan ReadParams(string code, ref int i)
	{
		var open = i;
		var depth = 0;
		var close = -1;

		var j = i;
		while (j < code.Length)
		{
			var ch = code[j];

			if (StringQuotes.Contains(ch))
			{
				ReadString(code, ref j, ch);
				continue;
			}

			if (ch == '(')
			{
				depth++;
			}
			else if (ch == ')')
			{
				depth--;
				if (depth == 0)
				{
					close = j;
					break;
				}
			}

			j++;
		}

		var hasClose = close >= 0;
		if (!hasClose)
			close = code.Length;

		var inner = code[(open + 1)..close];
		var children = new List<CodeNode> { new CodeText("(") };
		children.AddRange(Highlight(inner));

		if (hasClose)
			children.Add(new CodeText(")"));

		i = hasClose ? close + 1 : close;
		return new CodeSpan("hljs-params", children);
	}

	private static void AppendWhitespace(string code, ref int i, List<CodeNode> children)
	{
		var start = i;
		while (i < code.Length && code[i] is ' ' or '\t')
			i++;

		if (i > start)
			children.Add(new CodeText(code[start..i]));
	}

	private static CodeSpan ReadString(string code, ref int i, char quote)
	{
		var start = i;
		i++; // opening quote

		while (i < code.Length)
		{
			var ch = code[i];

			if (ch == '\\' && i + 1 < code.Length)
			{
				i += 2;
				continue;
			}

			if (ch == quote)
			{
				i++;
				break;
			}

			if (ch == '\n')
				break;

			i++;
		}

		return new CodeSpan("hljs-string", [new CodeText(code[start..i])]);
	}

	private static CodeSpan ReadNumber(string code, ref int i)
	{
		var start = i;

		if (code[i] == '0' && i + 1 < code.Length && code[i + 1] is 'x' or 'X')
		{
			i += 2;
			while (i < code.Length && Uri.IsHexDigit(code[i]))
				i++;
		}
		else
		{
			while (i < code.Length && char.IsDigit(code[i]))
				i++;

			if (i < code.Length && code[i] == '.' && i + 1 < code.Length && char.IsDigit(code[i + 1]))
			{
				i++;
				while (i < code.Length && char.IsDigit(code[i]))
					i++;
			}

			if (i < code.Length && code[i] is 'e' or 'E')
			{
				var save = i;
				i++;
				if (i < code.Length && code[i] is '+' or '-')
					i++;

				if (i < code.Length && char.IsDigit(code[i]))
					while (i < code.Length && char.IsDigit(code[i]))
						i++;
				else
					i = save;
			}
		}

		// Numeric suffixes (e.g. 10L, 1.5f, 100u)
		while (i < code.Length && char.IsLetter(code[i]))
			i++;

		return new CodeSpan("hljs-number", [new CodeText(code[start..i])]);
	}

	private static bool IsIdentifierStart(char c) =>
		char.IsLetter(c) || c == '_';

	private static bool IsIdentifierPart(char c) =>
		char.IsLetterOrDigit(c) || c == '_';

	private static bool Matches(string code, int i, string token) =>
		i + token.Length <= code.Length && string.CompareOrdinal(code, i, token, 0, token.Length) == 0;
}
