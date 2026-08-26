namespace MudBlazor;

/// <summary>
/// Highlighter for Rust.
/// </summary>
internal sealed class RustCodeHighlighter : CodeHighlighterBase
{
	public RustCodeHighlighter()
	{
		Keywords = FrozenSets.Create(
			"as", "async", "await", "break", "const", "continue", "crate", "dyn", "else", "enum", "extern",
			"for", "if", "impl", "in", "let", "loop", "match", "mod", "move", "mut", "pub", "ref", "return",
			"self", "static", "struct", "super", "trait", "type", "union", "unsafe", "use", "where", "while"
		);

		Types = FrozenSets.Create(
			"i8", "i16", "i32", "i64", "i128", "isize", "u8", "u16", "u32", "u64", "u128", "usize",
			"f32", "f64", "bool", "char", "str"
		);

		Literals = FrozenSets.Create("true", "false", "Some", "None", "Ok", "Err");
		FunctionKeywords = FrozenSets.Create("fn");
		TypeDeclarationKeywords = FrozenSets.Create("struct", "enum", "trait", "union");
		HighlightPascalCaseTypes = true;
		HighlightAnnotations = true;
		HighlightStringPrefixes = true;
		HighlightLifetimes = true;
		HighlightMacroInvocations = true;
		HighlightAmpersandOperator = true;
		StringQuotes = SearchValuess.Create('"');
	}
}
