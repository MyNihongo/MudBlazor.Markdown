using System.Buffers;

namespace MudBlazor;

/// <summary>
/// Highlighter for Rust.
/// </summary>
internal sealed class RustCodeHighlighter : CodeHighlighterBase
{
	public RustCodeHighlighter()
	{
		Keywords = FrozenSet.Create(
			"as", "async", "await", "break", "const", "continue", "crate", "dyn", "else", "enum", "extern",
			"for", "if", "impl", "in", "let", "loop", "match", "mod", "move", "mut", "pub", "ref", "return",
			"self", "static", "struct", "super", "trait", "type", "union", "unsafe", "use", "where", "while"
		);

		Types = FrozenSet.Create(
			"i8", "i16", "i32", "i64", "i128", "isize", "u8", "u16", "u32", "u64", "u128", "usize",
			"f32", "f64", "bool", "char", "str"
		);

		Literals = FrozenSet.Create("true", "false");
		FunctionKeywords = FrozenSet.Create("fn");
		HighlightPascalCaseTypes = true;
		StringQuotes = SearchValues.Create('"');
	}
}
