namespace MudBlazor;

/// <summary>
/// Highlighter for Kotlin.
/// </summary>
internal sealed class KotlinCodeHighlighter : CodeHighlighterBase
{
	public KotlinCodeHighlighter()
	{
		Keywords = FrozenSet.Create(
			"abstract", "annotation", "as", "break", "by", "catch", "class", "companion", "const", "constructor",
			"continue", "crossinline", "data", "do", "dynamic", "else", "enum", "external", "final", "finally",
			"for", "get", "if", "import", "in", "infix", "init", "inline", "inner", "interface", "internal", "is",
			"lateinit", "noinline", "object", "open", "operator", "out", "override", "package", "private",
			"protected", "public", "reified", "return", "sealed", "set", "super", "suspend", "tailrec", "this",
			"throw", "try", "typealias", "typeof", "val", "var", "vararg", "when", "while"
		);
		
		Types = FrozenSet.Create(
			"Array", "String", "Int", "Long", "Short", "Byte", "Boolean", "Char", "Float", "Double", "Unit",
			"Any", "Nothing", "List", "MutableList", "Map", "MutableMap", "Set", "MutableSet", "Collection",
			"Iterable", "Sequence", "Pair", "Triple"
		);

		Literals = FrozenSet.Create("true", "false", "null");
		FunctionKeywords = FrozenSet.Create("fun");
		HighlightPascalCaseTypes = true;
	}
}
