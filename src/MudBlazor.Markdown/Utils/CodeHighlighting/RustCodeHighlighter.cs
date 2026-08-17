namespace MudBlazor;

/// <summary>
/// Highlighter for Rust.
/// </summary>
internal sealed class RustCodeHighlighter() : CodeHighlighterBase(Definition)
{
	private static readonly LanguageDefinition Definition = new()
	{
		Keywords =
		[
			"as", "async", "await", "break", "const", "continue", "crate", "dyn", "else", "enum", "extern",
			"for", "if", "impl", "in", "let", "loop", "match", "mod", "move", "mut", "pub", "ref", "return",
			"self", "static", "struct", "super", "trait", "type", "union", "unsafe", "use", "where", "while",
		],
		Types =
		[
			// Built-in primitives are lowercase, so they must be listed explicitly;
			// PascalCase std types (String, Vec, Option, Result, ...) are handled by HighlightPascalCaseTypes.
			"i8", "i16", "i32", "i64", "i128", "isize", "u8", "u16", "u32", "u64", "u128", "usize",
			"f32", "f64", "bool", "char", "str",
		],
		Literals = ["true", "false"],
		FunctionKeywords = ["fn"],
		// Only double quotes: a single quote also starts a lifetime ('a, 'static), so treating it as a
		// string delimiter would corrupt lifetimes and generic bounds.
		StringQuotes = ['"'],
		HighlightPascalCaseTypes = true,
	};
}
