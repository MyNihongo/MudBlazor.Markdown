namespace MudBlazor;

/// <summary>
/// Highlighter for Go.
/// </summary>
internal sealed class GoCodeHighlighter : CodeHighlighterBase
{
	protected override IReadOnlySet<string> Keywords { get; } = new HashSet<string>(StringComparer.Ordinal)
	{
		"break", "case", "chan", "const", "continue", "default", "defer", "else", "fallthrough", "for",
		"go", "goto", "if", "import", "interface", "map", "package", "range", "return", "select", "struct",
		"switch", "type", "var",
	};

	protected override IReadOnlySet<string> Types { get; } = new HashSet<string>(StringComparer.Ordinal)
	{
		"bool", "string", "int", "int8", "int16", "int32", "int64", "uint", "uint8", "uint16", "uint32",
		"uint64", "uintptr", "byte", "rune", "float32", "float64", "complex64", "complex128", "error", "any",
	};

	protected override IReadOnlySet<string> Literals { get; } = new HashSet<string>(StringComparer.Ordinal)
	{
		"true", "false", "nil", "iota",
	};

	protected override IReadOnlySet<string> FunctionKeywords { get; } = new HashSet<string>(StringComparer.Ordinal)
	{
		"func",
	};

	// Go raw string literals: `...`
	protected override char? RawStringQuote => '`';
}
