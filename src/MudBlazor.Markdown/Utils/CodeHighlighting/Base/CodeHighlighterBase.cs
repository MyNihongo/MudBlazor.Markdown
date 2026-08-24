using System.Buffers;
using System.Text;
using BlockComment = (string Start, string End);

namespace MudBlazor;

internal abstract class CodeHighlighterBase : ICodeHighlighter
{
	private static readonly FrozenSet<string> DefaultLineComments = FrozenSets.Create("//");
	private static readonly FrozenSet<BlockComment> DefaultBlockComments = FrozenSets.Create(("/*", "*/"));
	private static readonly SearchValues<char> DefaultStringQuotes = SearchValuess.Create('"', '\'');

#if NET9_0_OR_GREATER
	private readonly FrozenSet<string>.AlternateLookup<ReadOnlySpan<char>> _keywords = FrozenSets.EmptyLookup;
	private readonly FrozenSet<string>.AlternateLookup<ReadOnlySpan<char>> _types = FrozenSets.EmptyLookup;
	private readonly FrozenSet<string>.AlternateLookup<ReadOnlySpan<char>> _literals = FrozenSets.EmptyLookup;
	private readonly FrozenSet<string>.AlternateLookup<ReadOnlySpan<char>> _functionKeywords = FrozenSets.EmptyLookup;
#else
	private readonly FrozenSet<string> _keywords = FrozenSet<string>.Empty;
	private readonly FrozenSet<string> _types = FrozenSet<string>.Empty;
	private readonly FrozenSet<string> _literals = FrozenSet<string>.Empty;
	private readonly FrozenSet<string> _functionKeywords = FrozenSet<string>.Empty;
#endif

	/// <summary>
	/// When <see langword="true"/>, an identifier immediately followed by <c>(</c> is rendered as a
	/// function title (a method call or declaration). Languages without a function keyword rely on this.
	/// </summary>
	protected bool HighlightMethodCalls { get; init; } = true;

	/// <summary>
	/// When <see langword="true"/>, an uppercase-first identifier in a type position (e.g. <c>Guid Id</c>,
	/// <c>List&lt;T&gt;</c>, <c>name: Type</c>, <c>new Type()</c>) is rendered as a type, and its generic
	/// arguments are highlighted.
	/// </summary>
	protected bool HighlightPascalCaseTypes { get; init; }

	/// <summary>
	/// Character that opens and closes a raw string that performs no escaping (e.g. Go back-tick
	/// strings), or <see langword="null"/> when the language has none.
	/// </summary>
	protected char? RawStringQuote { get; init; }

	/// <summary>
	/// When <see langword="true"/>, HTML tags and their attributes are tokenized (for markup
	/// languages such as Razor).
	/// </summary>
	protected bool HighlightHtmlTags { get; init; }

	/// <summary>
	/// When <see langword="true"/>, a line whose first non-whitespace character is <c>#</c> is rendered
	/// as a preprocessor/meta directive (e.g. <c>#region</c>, <c>#nullable enable</c>).
	/// </summary>
	protected bool HighlightPreprocessor { get; init; }

	/// <summary>
	/// When <see langword="true"/>, a string may be prefixed by <c>$</c> and/or <c>@</c>
	/// (e.g. <c>$"..."</c>, <c>@"..."</c>, <c>$@"..."</c>); the prefix is included in the string token.
	/// </summary>
	protected bool HighlightStringPrefixes { get; init; }

	/// <summary>
	/// Reserved words rendered as keyword tokens.
	/// </summary>
	protected FrozenSet<string> Keywords
	{
#if NET9_0_OR_GREATER
		init => _keywords = value.GetAlternateLookup<ReadOnlySpan<char>>();
#else
		init => _keywords = value;
#endif
	}

	/// <summary>
	/// Built-in type names rendered as type tokens.
	/// </summary>
	protected FrozenSet<string> Types
	{
#if NET9_0_OR_GREATER
		init => _types = value.GetAlternateLookup<ReadOnlySpan<char>>();
#else
		init => _types = value;
#endif
	}

	/// <summary>
	/// Literals (e.g. <c>true</c>, <c>null</c>) rendered as literal tokens.
	/// </summary>
	protected FrozenSet<string> Literals
	{
#if NET9_0_OR_GREATER
		init => _literals = value.GetAlternateLookup<ReadOnlySpan<char>>();
#else
		init => _literals = value;
#endif
	}

	/// <summary>
	/// Keywords that introduce a function declaration (e.g. <c>func</c>, <c>fun</c>); the name that
	/// follows is rendered as a title and the parameter list as its own span.
	/// </summary>
	protected FrozenSet<string> FunctionKeywords
	{
#if NET9_0_OR_GREATER
		init => _functionKeywords = value.GetAlternateLookup<ReadOnlySpan<char>>();
#else
		init => _functionKeywords = value;
#endif
	}

	/// <summary>
	/// Prefixes that start a single-line comment. Defaults to <c>//</c>.
	/// </summary>
	protected FrozenSet<string> LineComments { get; init; } = DefaultLineComments;

	/// <summary>
	/// Start/end delimiter pairs for block comments. Defaults to <c>/* */</c>.
	/// </summary>
	protected FrozenSet<BlockComment> BlockComments { get; init; } = DefaultBlockComments;

	/// <summary>
	/// Characters that open and close a string or character literal. Defaults to <c>"</c> and <c>'</c>.
	/// </summary>
	protected SearchValues<char> StringQuotes { get; init; } = DefaultStringQuotes;

	public IReadOnlyList<CodeNode> Highlight(string code)
	{
		var nodes = new List<CodeNode>();
		var text = new StringBuilder();
		var i = 0;
		var inHtmlText = false;

		while (i < code.Length)
		{
			if (TryReadLineComment() || TryReadBlockComment())
				continue;

			var c = code[i];

			if (HighlightPreprocessor && c == '#' && IsAtLineStart(code, i))
			{
				FlushText();

				// Only the "#directive" token is meta (e.g. #region, #endregion, #nullable).
				var start = i++;
				while (i < code.Length && IsIdentifierPart(code[i]))
					i++;

				nodes.Add(new CodeSpan("hljs-meta", [new CodeText(code[start..i])]));

				// The rest of the line (e.g. the "Directive Test" label) stays plain text.
				var restStart = i;
				while (i < code.Length && code[i] != '\n')
					i++;

				if (i > restStart)
					text.Append(code[restStart..i]);

				continue;
			}

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

			if (HighlightStringPrefixes && c is '$' or '@')
			{
				var quoteIndex = StringPrefixQuoteIndex(code, i);
				if (quoteIndex >= 0)
				{
					FlushText();
					var start = i;
					var interpolated = code[start..quoteIndex].Contains('$');
					var raw = quoteIndex + 2 < code.Length && code[quoteIndex] == '"' &&
					          code[quoteIndex + 1] == '"' && code[quoteIndex + 2] == '"';

					i = quoteIndex;
					if (interpolated && !raw)
					{
						nodes.Add(ReadInterpolatedString(code, ref i, start));
					}
					else
					{
						ReadStringLiteral(code, ref i); // advances i past the string
						nodes.Add(new CodeSpan("hljs-string", [new CodeText(code[start..i])]));
					}

					continue;
				}
			}

			if (StringQuotes.Contains(c))
			{
				FlushText();
				var stringStart = i;
				ReadStringLiteral(code, ref i);
				nodes.Add(new CodeSpan("hljs-string", [new CodeText(code[stringStart..i])]));
				continue;
			}

			if (HighlightHtmlTags && c == '<')
			{
				if (Matches(code, i, "<!--"))
				{
					FlushText();
					nodes.Add(ReadHtmlComment(code, ref i));
					inHtmlText = true;
					continue;
				}

				if (IsHtmlTagStart(code, i, inHtmlText))
				{
					FlushText();
					nodes.Add(ReadHtmlTag(code, ref i));
					inHtmlText = true;
					continue;
				}
			}

			if (char.IsAsciiDigit(c))
			{
				FlushText();
				nodes.Add(ReadNumber(code, ref i));
				continue;
			}

			if (IsIdentifierStart(c))
			{
				var start = i;
				i++;
				while (i < code.Length && IsIdentifierPart(code[i]))
					i++;

#if NET9_0_OR_GREATER
				var word = code.AsSpan(start, i - start);
#else
				var word = code.Substring(start, i - start);
#endif

				if (_functionKeywords.Contains(word))
				{
					FlushText();
					nodes.Add(ReadFunction(code, code[start..i], ref i));
				}
				else if (_keywords.Contains(word))
				{
					FlushText();
					nodes.Add(new CodeSpan("hljs-keyword", [new CodeText(code[start..i])]));
				}
				else if (_literals.Contains(word))
				{
					FlushText();
					nodes.Add(new CodeSpan("hljs-literal", [new CodeText(code[start..i])]));
				}
				else if (_types.Contains(word))
				{
					FlushText();
					nodes.Add(new CodeSpan("hljs-type", [new CodeText(code[start..i])]));

					if (HighlightPascalCaseTypes && i < code.Length && code[i] == '<')
						ReadGenericArguments(code, ref i, nodes);
				}
				else if (HighlightPascalCaseTypes && !inHtmlText && char.IsUpper(code[start]) &&
				         (IsTypePosition(code, start, i) || IsPrecededByTypeKeyword(code, start)))
				{
					// PascalCase identifier used as a type: "Guid Id", "List<T>", "new Type()".
					FlushText();
					nodes.Add(new CodeSpan("hljs-type", [new CodeText(code[start..i])]));

					if (i < code.Length && code[i] == '<')
						ReadGenericArguments(code, ref i, nodes);
				}
				else if (HighlightMethodCalls && i < code.Length && code[i] == '(')
				{
					// Method call/declaration: identifier immediately followed by '('.
					FlushText();
					nodes.Add(new CodeSpan("hljs-title", [new CodeText(code[start..i])]));
				}
				else
				{
					// Plain identifier: appended by span, no substring allocation.
					text.Append(word);
				}

				continue;
			}

			if (HighlightHtmlTags && c == '@')
				inHtmlText = false;

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

	// Reads an interpolated string ($"...{expr}...") starting at the opening quote code[i].
	// Literal parts become string text; each {expr} hole becomes an hljs-subst span whose
	// content is recursively highlighted as code. Advances i past the closing quote.
	private CodeSpan ReadInterpolatedString(string code, ref int i, int start)
	{
		var quote = code[i];
		var children = new List<CodeNode>();
		var segmentStart = start;

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

			if (ch == '{')
			{
				// "{{" is an escaped brace, not an interpolation hole.
				if (i + 1 < code.Length && code[i + 1] == '{')
				{
					i += 2;
					continue;
				}

				if (i > segmentStart)
					children.Add(new CodeText(code[segmentStart..i]));

				i++; // past '{'
				var exprStart = i;
				var depth = 1;
				while (i < code.Length && depth > 0)
				{
					var hc = code[i];
					if (hc is '"' or '\'')
					{
						ReadStringLiteral(code, ref i);
					}
					else if (hc == '{')
					{
						depth++;
						i++;
					}
					else if (hc == '}')
					{
						depth--;
						if (depth == 0)
							break;
						i++;
					}
					else
					{
						i++;
					}
				}

				var substChildren = new List<CodeNode> { new CodeText("{") };
				substChildren.AddRange(Highlight(code[exprStart..i]));
				substChildren.Add(new CodeText("}"));
				children.Add(new CodeSpan("hljs-subst", substChildren));

				if (i < code.Length)
					i++; // past '}'

				segmentStart = i;
				continue;
			}

			i++;
		}

		if (i > segmentStart)
			children.Add(new CodeText(code[segmentStart..i]));

		return new CodeSpan("hljs-string", children);
	}

	// Advances i past a string literal starting at the quote code[i], handling raw strings
	// ("""..."""), closed by a run of at least as many quotes as opened it.
	private void ReadStringLiteral(string code, ref int i)
	{
		var quote = code[i];

		if (quote == '"' && i + 2 < code.Length && code[i + 1] == '"' && code[i + 2] == '"')
		{
			var openCount = 0;
			while (i < code.Length && code[i] == '"')
			{
				openCount++;
				i++;
			}

			while (i < code.Length)
			{
				if (code[i] != '"')
				{
					i++;
					continue;
				}

				var run = 0;
				while (i < code.Length && code[i] == '"')
				{
					run++;
					i++;
				}

				if (run >= openCount)
					return;
			}

			return;
		}

		ReadString(code, ref i, quote); // advances i; returned span is unused here
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
			while (i < code.Length && (char.IsAsciiHexDigit(code[i]) || code[i] == '_'))
				i++;
		}
		else
		{
			while (i < code.Length && (char.IsAsciiDigit(code[i]) || code[i] == '_'))
				i++;

			if (i < code.Length && code[i] == '.' && i + 1 < code.Length && char.IsAsciiDigit(code[i + 1]))
			{
				i++;
				while (i < code.Length && (char.IsAsciiDigit(code[i]) || code[i] == '_'))
					i++;
			}

			if (i < code.Length && code[i] is 'e' or 'E')
			{
				var save = i;
				i++;
				if (i < code.Length && code[i] is '+' or '-')
					i++;

				if (i < code.Length && char.IsAsciiDigit(code[i]))
					while (i < code.Length && (char.IsAsciiDigit(code[i]) || code[i] == '_'))
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

	private static bool IsTypePosition(string code, int start, int i)
	{
		// Followed by a generic list, an array, or another identifier: "Type name", "Type<...>", "Type[]".
		// A single trailing '?' (nullable) is skipped, so "Type? name" is still recognised.
		if (i < code.Length)
		{
			var j = i;
			if (code[j] == '?')
				j++;

			if (j < code.Length && code[j] is '<' or '[')
				return true;

			while (j < code.Length && code[j] is ' ' or '\t')
				j++;

			if (j < code.Length && IsIdentifierStart(code[j]))
				return true;
		}

		// Preceded by ':' : annotation or inheritance ("name: Type", "class X : Base").
		var k = start - 1;
		while (k >= 0 && code[k] is ' ' or '\t')
			k--;

		return k >= 0 && code[k] == ':' && (k == 0 || code[k - 1] != ':');
	}

	private static bool IsPrecededByTypeKeyword(string code, int start)
	{
		var end = start - 1;
		while (end >= 0 && code[end] is ' ' or '\t')
			end--;

		var wordStart = end;
		while (wordStart >= 0 && IsIdentifierPart(code[wordStart]))
			wordStart--;

		wordStart++;
		if (wordStart > end)
			return false;

		// Keywords directly followed by a type: constructor (new), cast (as), and pattern forms (is/not/and/or).
		var word = code.AsSpan(wordStart, end - wordStart + 1);
		return word is "new" or "is" or "as" or "not" or "and" or "or";
	}

	private static int StringPrefixQuoteIndex(string code, int i)
	{
		var j = i;
		if (j < code.Length && code[j] is '$' or '@')
			j++;
		else
			return -1;

		if (j < code.Length && code[j] is '$' or '@')
			j++;

		return j < code.Length && code[j] == '"' ? j : -1;
	}

	private static bool IsAtLineStart(string code, int i)
	{
		var k = i - 1;
		while (k >= 0 && code[k] is ' ' or '\t')
			k--;

		return k < 0 || code[k] == '\n';
	}

	private void ReadGenericArguments(string code, ref int i, List<CodeNode> nodes)
	{
		var raw = new StringBuilder();

		var depth = 0;
		while (i < code.Length)
		{
			var c = code[i];

			if (c == '<')
			{
				depth++;
				raw.Append(c);
				i++;
				continue;
			}

			if (c == '>')
			{
				depth--;
				raw.Append(c);
				i++;
				if (depth == 0)
					break;
				continue;
			}

			if (IsIdentifierStart(c))
			{
				var start = i;
				i++;
				while (i < code.Length && IsIdentifierPart(code[i]))
					i++;

#if NET9_0_OR_GREATER
				var word = code.AsSpan(start, i - start);
#else
				var word = code.Substring(start, i - start);
#endif
				if (_keywords.Contains(word))
				{
					FlushRaw();
					nodes.Add(new CodeSpan("hljs-keyword", [new CodeText(code[start..i])]));
				}
				else if (_types.Contains(word) || char.IsUpper(code[start]))
				{
					FlushRaw();
					nodes.Add(new CodeSpan("hljs-type", [new CodeText(code[start..i])]));
				}
				else
				{
					raw.Append(word);
				}

				continue;
			}

			raw.Append(c);
			i++;
		}

		FlushRaw();
		return;

		void FlushRaw()
		{
			if (raw.Length == 0)
				return;

			nodes.Add(new CodeText(raw.ToString()));
			raw.Clear();
		}
	}

	private static bool IsHtmlTagStart(string code, int i, bool inHtmlText)
	{
		var next = i + 1;
		if (next >= code.Length)
			return false;

		var c = code[next];
		if (c is '/' or '!')
			return true;

		if (!char.IsLetter(c))
			return false;

		// In HTML text any '<letter' opens a tag. In a code region a '<' that directly
		// follows an identifier is a generic argument list (e.g. List<T>), not a tag.
		if (inHtmlText)
			return true;

		return i == 0 || !IsIdentifierPart(code[i - 1]);
	}

	private static CodeSpan ReadHtmlComment(string code, ref int i)
	{
		var start = i;
		i += "<!--".Length;

		while (i < code.Length && !Matches(code, i, "-->"))
			i++;

		if (i < code.Length)
			i += "-->".Length;

		return new CodeSpan("hljs-comment", [new CodeText(code[start..i])]);
	}

	private CodeSpan ReadHtmlTag(string code, ref int i)
	{
		var children = new List<CodeNode>();
		var raw = new StringBuilder();

		void FlushRaw()
		{
			if (raw.Length == 0)
				return;

			children.Add(new CodeText(raw.ToString()));
			raw.Clear();
		}

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

			if (char.IsLetter(c) || c is '_' or '@' or ':')
			{
				var start = i;
				while (i < code.Length && (char.IsLetterOrDigit(code[i]) || code[i] is '-' or '_' or ':' or '@' or '.'))
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
	}

	private static bool IsIdentifierStart(char c) =>
		char.IsLetter(c) || c == '_';

	private static bool IsIdentifierPart(char c) =>
		char.IsLetterOrDigit(c) || c == '_';

	private static bool Matches(string code, int i, string token) =>
		i + token.Length <= code.Length && string.CompareOrdinal(code, i, token, 0, token.Length) == 0;
}
