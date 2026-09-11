namespace MudBlazor;

/// <summary>
/// Highlighter for Go.
/// </summary>
internal sealed class GoCodeHighlighter : CodeHighlighterBase
{
	public GoCodeHighlighter()
	{
		Keywords = FrozenSets.Create(
			"break", "case", "chan", "const", "continue", "default", "defer", "else", "fallthrough", "for",
			"go", "goto", "if", "import", "interface", "map", "package", "range", "return", "select", "struct",
			"switch", "type", "var"
		);

		Types = FrozenSets.Create(
			"bool", "string", "int", "int8", "int16", "int32", "int64", "uint", "uint8", "uint16", "uint32",
			"uint64", "uintptr", "byte", "rune", "float32", "float64", "complex64", "complex128", "error", "any"
		);

		Literals = FrozenSets.Create("true", "false", "nil", "iota");
		FunctionKeywords = FrozenSets.Create("func");
		RawStringQuote = '`';
		HighlightPostfixTypes = true;
	}
}
