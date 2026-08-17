namespace MudBlazor;

/// <summary>
/// Highlighter for C#.
/// </summary>
internal class CSharpCodeHighlighter : CodeHighlighterBase
{
	protected override IReadOnlySet<string> Keywords { get; } = new HashSet<string>(StringComparer.Ordinal)
	{
		"abstract", "as", "async", "await", "base", "break", "case", "catch", "checked", "class", "const",
		"continue", "default", "delegate", "do", "else", "enum", "event", "explicit", "extern", "finally",
		"fixed", "for", "foreach", "goto", "if", "implicit", "in", "interface", "internal", "is", "lock",
		"namespace", "new", "operator", "out", "override", "params", "private", "protected", "public",
		"readonly", "ref", "return", "sealed", "sizeof", "stackalloc", "static", "struct", "switch", "this",
		"throw", "try", "typeof", "unchecked", "unsafe", "using", "virtual", "volatile", "while", "var",
		"record", "init", "nameof", "when", "where", "yield", "get", "set", "value", "partial", "global",
		"with", "required", "file", "scoped",
	};

	protected override IReadOnlySet<string> Types { get; } = new HashSet<string>(StringComparer.Ordinal)
	{
		"bool", "byte", "sbyte", "char", "decimal", "double", "float", "int", "uint", "long", "ulong",
		"short", "ushort", "object", "string", "void", "dynamic", "nint", "nuint",
	};

	protected override IReadOnlySet<string> Literals { get; } = new HashSet<string>(StringComparer.Ordinal)
	{
		"true", "false", "null",
	};
}
