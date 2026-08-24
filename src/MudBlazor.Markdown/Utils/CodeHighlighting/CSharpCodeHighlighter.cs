namespace MudBlazor;

/// <summary>
/// Highlighter for C#.
/// </summary>
internal class CSharpCodeHighlighter : CodeHighlighterBase
{
	public CSharpCodeHighlighter()
	{
		Keywords = FrozenSets.Create(
			"abstract", "as", "async", "await", "base", "break", "case", "catch", "checked", "class", "const",
			"continue", "default", "delegate", "do", "else", "enum", "event", "explicit", "extern", "finally",
			"fixed", "for", "foreach", "goto", "if", "implicit", "in", "interface", "internal", "is", "lock",
			"namespace", "new", "operator", "out", "override", "params", "private", "protected", "public",
			"readonly", "ref", "return", "sealed", "sizeof", "stackalloc", "static", "struct", "switch", "this",
			"throw", "try", "typeof", "unchecked", "unsafe", "using", "virtual", "volatile", "while", "var",
			"record", "init", "nameof", "when", "where", "yield", "get", "set", "value", "partial", "global",
			"with", "required", "file", "scoped", "and", "or", "not"
		);

		Types = FrozenSets.Create(
			"bool", "byte", "sbyte", "char", "decimal", "double", "float", "int", "uint", "long", "ulong",
			"short", "ushort", "object", "string", "void", "dynamic", "nint", "nuint"
		);

		Literals = FrozenSets.Create("true", "false", "null");
		HighlightPascalCaseTypes = true;
		HighlightPreprocessor = true;
		HighlightStringPrefixes = true;
	}
}
