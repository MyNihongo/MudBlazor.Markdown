namespace MudBlazor;

/// <summary>
/// Highlighter for C#.
/// </summary>
internal sealed class CSharpCodeHighlighter() : CodeHighlighterBase(Definition)
{
	// Shared so RazorCodeHighlighter can reuse the C# token rules.
	internal static readonly LanguageDefinition Definition = new()
	{
		Keywords =
		[
			"abstract", "as", "async", "await", "base", "break", "case", "catch", "checked", "class", "const",
			"continue", "default", "delegate", "do", "else", "enum", "event", "explicit", "extern", "finally",
			"fixed", "for", "foreach", "goto", "if", "implicit", "in", "interface", "internal", "is", "lock",
			"namespace", "new", "operator", "out", "override", "params", "private", "protected", "public",
			"readonly", "ref", "return", "sealed", "sizeof", "stackalloc", "static", "struct", "switch", "this",
			"throw", "try", "typeof", "unchecked", "unsafe", "using", "virtual", "volatile", "while", "var",
			"record", "init", "nameof", "when", "where", "yield", "get", "set", "value", "partial", "global",
			"with", "required", "file", "scoped",
		],
		Types =
		[
			"bool", "byte", "sbyte", "char", "decimal", "double", "float", "int", "uint", "long", "ulong",
			"short", "ushort", "object", "string", "void", "dynamic", "nint", "nuint",
		],
		Literals = ["true", "false", "null"],
	};
}
