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
	private readonly FrozenSet<string>.AlternateLookup<ReadOnlySpan<char>> _typeDeclarationKeywords = FrozenSets.EmptyLookup;
#else
	private readonly FrozenSet<string> _keywords = FrozenSet<string>.Empty;
	private readonly FrozenSet<string> _types = FrozenSet<string>.Empty;
	private readonly FrozenSet<string> _literals = FrozenSet<string>.Empty;
	private readonly FrozenSet<string> _functionKeywords = FrozenSet<string>.Empty;
	private readonly FrozenSet<string> _typeDeclarationKeywords = FrozenSet<string>.Empty;
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
	/// When <see langword="true"/>, any PascalCase identifier (an upper-first name that also contains a
	/// lowercase letter) is rendered as a type regardless of its position. Suits languages such as Kotlin
	/// whose convention reserves PascalCase for types/classes and camelCase for members, so
	/// <c>Delegates.observable</c> or <c>LazyThreadSafetyMode</c> are recognised even outside a type
	/// position. Fully uppercase names (e.g. <c>MAX_RETRIES</c>, enum entries) are left untouched.
	/// </summary>
	protected bool HighlightPascalCaseTypesEverywhere { get; init; }

	/// <summary>
	/// When <see langword="true"/>, an upper-first identifier that sits in a Go-style trailing type
	/// position - right after a name (<c>x Type</c>, as in <c>CreatedAt time.Time</c> or a
	/// <c>[T Constraint]</c> type parameter) or after a pointer <c>*</c> (<c>*User</c>) - is rendered as a
	/// type. Suits languages such as Go whose declarations put the type after the name. A name reached
	/// through <c>.</c> is excluded (it is a member access).
	/// </summary>
	protected bool HighlightPostfixTypes { get; init; }

	/// <summary>
	/// When <see langword="true"/>, double-quoted strings are scanned for Kotlin-style interpolation with
	/// no string prefix: <c>$name</c> and <c>${expr}</c> holes are rendered as <c>hljs-subst</c> spans,
	/// the expression inside <c>${…}</c> being highlighted recursively. Applies to both regular
	/// <c>"…"</c> and raw <c>"""…"""</c> strings.
	/// </summary>
	protected bool InterpolateStrings { get; init; }

	/// <summary>
	/// When <see langword="true"/>, an annotation/attribute is rendered as a meta directive
	/// (<c>hljs-meta</c>): an <c>@</c> that starts a token and is followed by an identifier, including an
	/// optional use-site target (e.g. <c>@JvmInline</c>, <c>@Target</c>, <c>@file:Suppress</c>), or a
	/// Rust attribute - outer <c>#[...]</c> and inner <c>#![...]</c> - whose upper-first identifiers (such
	/// as the traits in <c>#[derive(Debug, Clone)]</c>) are rendered as types and whose string literals as
	/// strings, every other token staying meta-coloured.
	/// </summary>
	protected bool HighlightAnnotations { get; init; }

	/// <summary>
	/// When <see langword="true"/>, an identifier immediately followed by a trailing lambda (an optional
	/// run of spaces/tabs then <c>{</c>) is rendered as a function title, matching Kotlin calls that omit
	/// the parentheses (e.g. <c>list.filter { … }</c>, <c>run { … }</c>).
	/// </summary>
	protected bool HighlightTrailingLambdaCalls { get; init; }

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
	/// When <see langword="true"/>, a string may carry a language prefix that is folded into the string
	/// token: C#-style <c>$"..."</c>, <c>@"..."</c>, <c>$@"..."</c>, and Rust-style raw/byte strings
	/// <c>r"..."</c>, <c>r#"..."#</c>, <c>b"..."</c>, <c>br#"..."#</c> and byte characters <c>b'x'</c>.
	/// </summary>
	protected bool HighlightStringPrefixes { get; init; }

	/// <summary>
	/// When <see langword="true"/>, a <c>'</c> introduces either a lifetime/label such as <c>'a</c>,
	/// <c>'static</c> or <c>'outer</c> (rendered as a symbol), or a character literal such as <c>'x'</c> or
	/// <c>'\n'</c> (rendered as a string).
	/// </summary>
	protected bool HighlightLifetimes { get; init; }

	/// <summary>
	/// When <see langword="true"/>, an identifier immediately followed by <c>!</c> is rendered as a macro
	/// invocation title (e.g. <c>println!</c>, <c>create_map!</c>); the <c>!</c> is part of the token.
	/// </summary>
	protected bool HighlightMacroInvocations { get; init; }

	/// <summary>
	/// When <see langword="true"/>, the reference/borrow operator <c>&amp;</c> is rendered as an operator.
	/// </summary>
	protected bool HighlightAmpersandOperator { get; init; }

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
	/// Keywords that introduce a type declaration (e.g. <c>struct</c>, <c>enum</c>, <c>trait</c>); the
	/// upper-first name that follows is rendered as a type even when nothing else marks it (so a unit
	/// struct <c>struct Marker;</c> or a bare <c>enum Status {</c> is still recognised).
	/// </summary>
	protected FrozenSet<string> TypeDeclarationKeywords
	{
#if NET9_0_OR_GREATER
		init => _typeDeclarationKeywords = value.GetAlternateLookup<ReadOnlySpan<char>>();
#else
		init => _typeDeclarationKeywords = value;
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

			if (HighlightStringPrefixes && c is 'r' or 'b' && TryReadRustString(code, ref i, out var rustString))
			{
				FlushText();
				nodes.Add(rustString);
				continue;
			}

			if (StringQuotes.Contains(c))
			{
				FlushText();

				if (InterpolateStrings && c == '"')
				{
					nodes.Add(ReadDollarInterpolatedString(code, ref i));
					continue;
				}

				var stringStart = i;
				ReadStringLiteral(code, ref i);
				nodes.Add(new CodeSpan("hljs-string", [new CodeText(code[stringStart..i])]));
				continue;
			}

			if (HighlightLifetimes && c == '\'')
			{
				FlushText();
				nodes.Add(ReadLifetimeOrChar(code, ref i));
				continue;
			}

			if (HighlightAmpersandOperator && c == '&')
			{
				FlushText();
				nodes.Add(ReadAmpersand(code, ref i));
				continue;
			}

			if (HighlightAnnotations && c == '@' && i + 1 < code.Length && IsIdentifierStart(code[i + 1]) &&
			    (i == 0 || !IsIdentifierPart(code[i - 1])))
			{
				FlushText();
				nodes.Add(ReadAnnotation(code, ref i));
				continue;
			}

			if (HighlightAnnotations && c == '#' && (Matches(code, i, "#[") || Matches(code, i, "#![")))
			{
				FlushText();
				nodes.Add(ReadAttribute(code, ref i));
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

				if (HighlightMacroInvocations && i < code.Length && code[i] == '!' &&
				    (i + 1 >= code.Length || code[i + 1] != '='))
				{
					// Macro invocation: "println!", "create_map!". The '!' is part of the title.
					FlushText();
					i++; // '!'
					nodes.Add(new CodeSpan("hljs-title", [new CodeText(code[start..i])]));
				}
				else if (_functionKeywords.Contains(word))
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
				else if (!inHtmlText && char.IsUpper(code[start]) &&
				         ((HighlightPascalCaseTypes && (IsTypePosition(code, start, i) || IsPrecededByTypeKeyword(code, start)))
				          || (HighlightPascalCaseTypesEverywhere && IsPascalCase(code, start, i) && !IsMemberAccess(code, start))
				          || (HighlightPostfixTypes && !IsMemberAccess(code, start) &&
				              (IsPrecededByNameOrPointer(code, start) || i - start == 1))))
				{
					// PascalCase identifier used as a type: "Guid Id", "List<T>", "new Type()", or -
					// when HighlightPascalCaseTypesEverywhere is set - any PascalCase name (e.g. "Delegates"), or -
					// when HighlightPostfixTypes is set - an upper-first name in Go's trailing type slot ("*User")
					// or a single upper-case letter (a Go type parameter such as "T", highlighted in every
					// position for consistency). A name reached through '.' is a member access, not a type.
					FlushText();
					nodes.Add(new CodeSpan("hljs-type", [new CodeText(code[start..i])]));

					if (i < code.Length && code[i] == '<')
						ReadGenericArguments(code, ref i, nodes);
				}
				else if (HighlightMethodCalls && i < code.Length &&
				         (code[i] == '(' || (code[i] == '<' && IsGenericMethodCall(code, i)) ||
				          (HighlightTrailingLambdaCalls && IsTrailingLambdaCall(code, i))))
				{
					// Method call/declaration: identifier immediately followed by '(', by a generic argument
					// list that is itself followed by '(' (e.g. "mutableListOf<String>()"), or by a trailing
					// lambda (e.g. "filter { … }").
					FlushText();
					nodes.Add(new CodeSpan("hljs-title", [new CodeText(code[start..i])]));

					if (code[i] == '<')
						ReadGenericArguments(code, ref i, nodes);
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

	// Reads an annotation starting at code[i] == '@' (the caller has verified an identifier follows),
	// including an optional use-site target such as "@file:Suppress". Advances i past the annotation.
	private static CodeSpan ReadAnnotation(string code, ref int i)
	{
		var start = i;
		i++; // '@'

		while (i < code.Length && IsIdentifierPart(code[i]))
			i++;

		// Use-site target: "@file:Suppress", "@get:JvmName", etc.
		if (i + 1 < code.Length && code[i] == ':' && IsIdentifierStart(code[i + 1]))
		{
			i++; // ':'
			while (i < code.Length && IsIdentifierPart(code[i]))
				i++;
		}

		return new CodeSpan("hljs-meta", [new CodeText(code[start..i])]);
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

	// Reads a Kotlin-style string starting at the double quote code[i]. Interpolation holes need no string
	// prefix: "$name" becomes an hljs-subst span of plain text, and "${expr}" an hljs-subst span whose
	// expression is highlighted recursively. Handles both regular ("...") and raw ("""...""") strings, the
	// latter treating backslashes literally. Advances i past the closing quote(s).
	private CodeSpan ReadDollarInterpolatedString(string code, ref int i)
	{
		var quote = code[i];
		var isRaw = Matches(code, i, "\"\"\"");
		var children = new List<CodeNode>();
		var segmentStart = i;

		i += isRaw ? 3 : 1; // opening quote(s)

		while (i < code.Length)
		{
			var ch = code[i];

			if (isRaw)
			{
				if (Matches(code, i, "\"\"\""))
				{
					i += 3;
					break;
				}
			}
			else
			{
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
			}

			if (ch == '$' && i + 1 < code.Length && (code[i + 1] == '{' || IsIdentifierStart(code[i + 1])))
			{
				if (i > segmentStart)
					children.Add(new CodeText(code[segmentStart..i]));

				if (code[i + 1] == '{')
				{
					i += 2; // past "${"
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

					var substChildren = new List<CodeNode> { new CodeText("${") };
					substChildren.AddRange(Highlight(code[exprStart..i]));
					substChildren.Add(new CodeText("}"));
					children.Add(new CodeSpan("hljs-subst", substChildren));

					if (i < code.Length)
						i++; // past '}'
				}
				else
				{
					var varStart = i;
					i++; // '$'
					while (i < code.Length && IsIdentifierPart(code[i]))
						i++;

					children.Add(new CodeSpan("hljs-subst", [new CodeText(code[varStart..i])]));
				}

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
		// Followed by a path separator: the type qualifying a path ("HashMap::new", "Status::Idle").
		if (i + 1 < code.Length && code[i] == ':' && code[i + 1] == ':')
			return true;

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

	// PascalCase = upper-first with at least one lowercase letter, so "LazyThreadSafetyMode" qualifies
	// but a fully uppercase constant/enum entry such as "MAX_RETRIES" does not.
	private static bool IsPascalCase(string code, int start, int i)
	{
		if (!char.IsUpper(code[start]))
			return false;

		for (var k = start + 1; k < i; k++)
			if (char.IsLower(code[k]))
				return true;

		return false;
	}

	// True when the upper-first identifier at code[start] sits in Go's trailing type position: directly
	// after a pointer '*' ("*User"), or after "<name> " where the name is another identifier
	// ("input T", "CreatedAt time.Time"). Go declarations write the type after the name, and two adjacent
	// identifiers only occur in such declarations, so this reliably marks a type.
	private static bool IsPrecededByNameOrPointer(string code, int start)
	{
		var k = start - 1;
		if (k >= 0 && code[k] == '*')
			return true;

		var sawSpace = false;
		while (k >= 0 && code[k] is ' ' or '\t')
		{
			sawSpace = true;
			k--;
		}

		return sawSpace && k >= 0 && IsIdentifierPart(code[k]);
	}

	// True when the identifier at code[start] is reached through a member access ('.'), e.g. the "Error"
	// in "State.Error", which is a member reference rather than a type.
	private static bool IsMemberAccess(string code, int start) =>
		start > 0 && code[start - 1] == '.';

	// True when the position code[i] begins a trailing lambda (optional spaces/tabs then '{'), which in
	// Kotlin marks a call whose parentheses are omitted, e.g. "filter { … }".
	private static bool IsTrailingLambdaCall(string code, int i)
	{
		var j = i;
		while (j < code.Length && code[j] is ' ' or '\t')
			j++;

		return j < code.Length && code[j] == '{';
	}

	// Given code[i] == '<', returns true when the angle brackets form a clean generic argument list that
	// is immediately followed by '(' (a generic method call, e.g. "mutableListOf<String>()"). Anything
	// other than identifiers, separators and nested angle brackets rules it out, avoiding false positives
	// on comparison operators.
	private static bool IsGenericMethodCall(string code, int i)
	{
		var depth = 0;
		var j = i;

		while (j < code.Length)
		{
			var c = code[j];

			if (c == '<')
			{
				depth++;
			}
			else if (c == '>')
			{
				depth--;
				if (depth == 0)
				{
					j++;
					break;
				}
			}
			else if (!IsIdentifierPart(c) && c is not (',' or ' ' or '\t' or '?' or '.' or ':'))
			{
				return false;
			}

			j++;
		}

		return depth == 0 && j < code.Length && code[j] == '(';
	}

	private bool IsPrecededByTypeKeyword(string code, int start)
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

		// Keywords directly followed by a type: constructor (new), cast (as), and pattern forms (is/not/and/or),
		// plus any language-specific type-declaration keyword (e.g. struct/enum/trait).
		var word = code.AsSpan(wordStart, end - wordStart + 1);
		if (word is "new" or "is" or "as" or "not" or "and" or "or")
			return true;

#if NET9_0_OR_GREATER
		return _typeDeclarationKeywords.Contains(word);
#else
		return _typeDeclarationKeywords.Contains(word.ToString());
#endif
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

	// Reads a Rust attribute starting at code[i] == '#' (either "#[" or "#!["), up to and including the
	// matching ']'. The whole attribute is a meta directive; upper-first identifiers inside are rendered as
	// types and string literals as strings, everything else staying meta text.
	private CodeSpan ReadAttribute(string code, ref int i)
	{
		var children = new List<CodeNode>();
		var raw = new StringBuilder();

		raw.Append(code[i++]); // '#'
		if (i < code.Length && code[i] == '!')
			raw.Append(code[i++]); // '!'
		if (i < code.Length && code[i] == '[')
			raw.Append(code[i++]); // '['

		var depth = 1;
		while (i < code.Length && depth > 0)
		{
			var ch = code[i];

			if (ch == '"')
			{
				FlushRaw();
				children.Add(ReadString(code, ref i, '"'));
				continue;
			}

			if (ch == '[')
			{
				depth++;
				raw.Append(ch);
				i++;
				continue;
			}

			if (ch == ']')
			{
				depth--;
				raw.Append(ch);
				i++;
				continue;
			}

			if (IsIdentifierStart(ch))
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

				// Upper-first identifiers (e.g. the traits in derive) and built-in types (e.g. u8 in repr).
				if (char.IsUpper(code[start]) || _types.Contains(word))
				{
					FlushRaw();
					children.Add(new CodeSpan("hljs-type", [new CodeText(code[start..i])]));
				}
				else
				{
					raw.Append(code[start..i]);
				}

				continue;
			}

			raw.Append(ch);
			i++;
		}

		FlushRaw();
		return new CodeSpan("hljs-meta", children);

		void FlushRaw()
		{
			if (raw.Length == 0)
				return;

			children.Add(new CodeText(raw.ToString()));
			raw.Clear();
		}
	}

	// Reads a run of '&' starting at code[i] as an operator token.
	private static CodeSpan ReadAmpersand(string code, ref int i)
	{
		var start = i;
		while (i < code.Length && code[i] == '&')
			i++;

		return new CodeSpan("hljs-operator", [new CodeText(code[start..i])]);
	}

	// Reads either a Rust character literal ('x', '\n') as a string, or a lifetime/label ('a, 'static,
	// 'outer, '_) as a symbol, starting at code[i] == '\''.
	private static CodeNode ReadLifetimeOrChar(string code, ref int i)
	{
		var start = i;

		// Character literal: an escape ('\...') or a single character wrapped in quotes ('x').
		var isEscape = i + 1 < code.Length && code[i + 1] == '\\';
		var isSingleChar = i + 2 < code.Length && code[i + 1] != '\'' && code[i + 2] == '\'';
		if (isEscape || isSingleChar)
		{
			ReadCharLiteral(code, ref i);
			return new CodeSpan("hljs-string", [new CodeText(code[start..i])]);
		}

		// Lifetime / label: a quote followed by an identifier and no closing quote.
		if (i + 1 < code.Length && IsIdentifierStart(code[i + 1]))
		{
			i++; // '
			while (i < code.Length && IsIdentifierPart(code[i]))
				i++;

			return new CodeSpan("hljs-symbol", [new CodeText(code[start..i])]);
		}

		i++; // bare quote
		return new CodeText(code[start..i]);
	}

	// Advances i past a character literal starting at the quote code[i].
	private static void ReadCharLiteral(string code, ref int i)
	{
		i++; // opening quote

		if (i < code.Length && code[i] == '\\')
		{
			i++;
			while (i < code.Length && code[i] is not ('\'' or '\n'))
				i++;
		}
		else if (i < code.Length && code[i] != '\'')
		{
			i++;
		}

		if (i < code.Length && code[i] == '\'')
			i++;
	}

	// Attempts to read a Rust raw/byte string starting at code[i] (code[i] is 'r' or 'b'): r"...", r#"..."#,
	// b"...", br#"..."#, or the byte character b'x'. Returns false and leaves i unchanged when the prefix is
	// just the start of an ordinary identifier (e.g. "raw_data", "bin_val").
	private bool TryReadRustString(string code, ref int i, out CodeSpan node)
	{
		node = null!;
		var start = i;
		var j = i;

		if (code[j] == 'b')
		{
			j++;

			if (j < code.Length && code[j] == '\'')
			{
				ReadCharLiteral(code, ref j);
				i = j;
				node = new CodeSpan("hljs-string", [new CodeText(code[start..i])]);
				return true;
			}

			if (j < code.Length && code[j] == '"')
			{
				ReadString(code, ref j, '"');
				i = j;
				node = new CodeSpan("hljs-string", [new CodeText(code[start..i])]);
				return true;
			}

			// Anything other than a following 'r' (raw byte string) is an ordinary identifier.
			if (j >= code.Length || code[j] != 'r')
				return false;
		}

		if (j < code.Length && code[j] == 'r')
		{
			var k = j + 1;
			var hashes = 0;
			while (k < code.Length && code[k] == '#')
			{
				hashes++;
				k++;
			}

			if (k < code.Length && code[k] == '"')
			{
				k++; // opening quote
				while (k < code.Length)
				{
					if (code[k] == '"')
					{
						var run = 0;
						var q = k + 1;
						while (run < hashes && q < code.Length && code[q] == '#')
						{
							run++;
							q++;
						}

						if (run == hashes)
						{
							k = q;
							break;
						}
					}

					k++;
				}

				i = k;
				node = new CodeSpan("hljs-string", [new CodeText(code[start..i])]);
				return true;
			}
		}

		return false;
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

			if (HighlightLifetimes && c == '\'')
			{
				FlushRaw();
				nodes.Add(ReadLifetimeOrChar(code, ref i));
				continue;
			}

			if (HighlightAmpersandOperator && c == '&')
			{
				FlushRaw();
				nodes.Add(ReadAmpersand(code, ref i));
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
