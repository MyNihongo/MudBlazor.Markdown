using System.Buffers;
using System.Collections.Frozen;
using System.Text;

namespace MudBlazor;

/// <summary>
/// Handles line/block comments, strings, numbers, keywords, types and (for languages that declare
/// <see cref="FunctionKeywords"/>) function declarations rendered as nested
/// function / title / params spans.
/// </summary>
internal abstract class CodeHighlighterBase : ICodeHighlighter
{
	private readonly WordSet _keywords;
	private readonly WordSet _types;
	private readonly WordSet _literals;
	private readonly WordSet _functionKeywords;
	private readonly string[] _lineComments;
	private readonly (string Start, string End)[] _blockComments;
	private readonly SearchValues<char> _stringQuotes;
	private readonly char? _rawStringQuote;

	protected CodeHighlighterBase(LanguageDefinition definition)
	{
		_keywords = new WordSet(definition.Keywords);
		_types = new WordSet(definition.Types);
		_literals = new WordSet(definition.Literals);
		_functionKeywords = new WordSet(definition.FunctionKeywords);
		_lineComments = definition.LineComments;
		_blockComments = definition.BlockComments;
		_stringQuotes = SearchValues.Create(definition.StringQuotes);
		_rawStringQuote = definition.RawStringQuote;
	}

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

			if (_rawStringQuote is { } rawQuote && c == rawQuote)
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

			if (_stringQuotes.Contains(c))
			{
				FlushText();
				nodes.Add(ReadString(code, ref i, c));
				continue;
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

				var word = code.AsSpan(start, i - start);

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
				}
				else
				{
					// Plain identifier: appended by span, no substring allocation.
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
			foreach (var prefix in _lineComments)
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
			foreach (var (open, close) in _blockComments)
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

			if (_stringQuotes.Contains(ch))
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

	private CodeSpan ReadString(string code, ref int i, char quote)
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
			while (i < code.Length && char.IsAsciiHexDigit(code[i]))
				i++;
		}
		else
		{
			while (i < code.Length && char.IsAsciiDigit(code[i]))
				i++;

			if (i < code.Length && code[i] == '.' && i + 1 < code.Length && char.IsAsciiDigit(code[i + 1]))
			{
				i++;
				while (i < code.Length && char.IsAsciiDigit(code[i]))
					i++;
			}

			if (i < code.Length && code[i] is 'e' or 'E')
			{
				var save = i;
				i++;
				if (i < code.Length && code[i] is '+' or '-')
					i++;

				if (i < code.Length && char.IsAsciiDigit(code[i]))
					while (i < code.Length && char.IsAsciiDigit(code[i]))
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

	/// <summary>
	/// A frozen word set that can be probed by <see cref="ReadOnlySpan{T}"/> without allocating
	/// (using the alternate span lookup on .NET 9+); falls back to a string probe otherwise.
	/// </summary>
	private readonly struct WordSet
	{
		private readonly FrozenSet<string> _set;
#if NET9_0_OR_GREATER
		private readonly FrozenSet<string>.AlternateLookup<ReadOnlySpan<char>> _lookup;
		private readonly bool _hasLookup;
#endif

		public WordSet(string[] words)
		{
			_set = words.ToFrozenSet(StringComparer.Ordinal);
#if NET9_0_OR_GREATER
			_hasLookup = _set.Count > 0 && _set.Comparer is IAlternateEqualityComparer<ReadOnlySpan<char>, string>;
			if (_hasLookup)
				_lookup = _set.GetAlternateLookup<ReadOnlySpan<char>>();
#endif
		}

		public bool Contains(ReadOnlySpan<char> word)
		{
			if (_set.Count == 0)
				return false;

#if NET9_0_OR_GREATER
			if (_hasLookup)
				return _lookup.Contains(word);
#endif
			return _set.Contains(word.ToString());
		}
	}
}
