namespace MudBlazor;

/// <summary>
/// Highlighter for Kotlin.
/// </summary>
internal sealed class KotlinCodeHighlighter : CodeHighlighterBase
{
	protected override IReadOnlySet<string> Keywords { get; } = new HashSet<string>(StringComparer.Ordinal)
	{
		"abstract", "annotation", "as", "break", "by", "catch", "class", "companion", "const", "constructor",
		"continue", "crossinline", "data", "do", "dynamic", "else", "enum", "external", "final", "finally",
		"for", "get", "if", "import", "in", "infix", "init", "inline", "inner", "interface", "internal", "is",
		"lateinit", "noinline", "object", "open", "operator", "out", "override", "package", "private",
		"protected", "public", "reified", "return", "sealed", "set", "super", "suspend", "tailrec", "this",
		"throw", "try", "typealias", "typeof", "val", "var", "vararg", "when", "while",
	};

	protected override IReadOnlySet<string> Types { get; } = new HashSet<string>(StringComparer.Ordinal)
	{
		"Array", "String", "Int", "Long", "Short", "Byte", "Boolean", "Char", "Float", "Double", "Unit",
		"Any", "Nothing", "List", "MutableList", "Map", "MutableMap", "Set", "MutableSet", "Collection",
		"Iterable", "Sequence", "Pair", "Triple",
	};

	protected override IReadOnlySet<string> Literals { get; } = new HashSet<string>(StringComparer.Ordinal)
	{
		"true", "false", "null",
	};

	protected override IReadOnlySet<string> FunctionKeywords { get; } = new HashSet<string>(StringComparer.Ordinal)
	{
		"fun",
	};
}
