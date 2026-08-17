namespace MudBlazor;

/// <summary>
/// Highlighter for Go.
/// </summary>
internal sealed class GoCodeHighlighter() : CodeHighlighterBase(Definition)
{
	private static readonly LanguageDefinition Definition = new()
	{
		Keywords =
		[
			"break", "case", "chan", "const", "continue", "default", "defer", "else", "fallthrough", "for",
			"go", "goto", "if", "import", "interface", "map", "package", "range", "return", "select", "struct",
			"switch", "type", "var",
		],
		Types =
		[
			"bool", "string", "int", "int8", "int16", "int32", "int64", "uint", "uint8", "uint16", "uint32",
			"uint64", "uintptr", "byte", "rune", "float32", "float64", "complex64", "complex128", "error", "any",
		],
		Literals = ["true", "false", "nil", "iota"],
		FunctionKeywords = ["func"],
		// Go raw string literals: `...`
		RawStringQuote = '`',
	};
}
