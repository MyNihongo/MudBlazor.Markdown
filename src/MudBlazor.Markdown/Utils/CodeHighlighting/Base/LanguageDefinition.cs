namespace MudBlazor;

/// <summary>
/// Immutable description of a language's tokens, consumed by <see cref="CodeHighlighterBase"/>.<br/>
/// Built once per highlighter instance so lookups can be precomputed.
/// </summary>
internal sealed record LanguageDefinition
{
	/// <summary>Reserved words rendered as keyword tokens.</summary>
	public required string[] Keywords { get; init; }

	/// <summary>Built-in type names rendered as type tokens.</summary>
	public string[] Types { get; init; } = [];

	/// <summary>Literals (e.g. <c>true</c>, <c>null</c>) rendered as literal tokens.</summary>
	public string[] Literals { get; init; } = [];

	/// <summary>Keywords that introduce a function declaration (e.g. <c>func</c>, <c>fun</c>).</summary>
	public string[] FunctionKeywords { get; init; } = [];

	/// <summary>Prefixes that start a single-line comment.</summary>
	public string[] LineComments { get; init; } = ["//"];

	/// <summary>Delimiter pairs that start/end a block comment.</summary>
	public (string Start, string End)[] BlockComments { get; init; } = [("/*", "*/")];

	/// <summary>Characters that open/close a string or character literal.</summary>
	public char[] StringQuotes { get; init; } = ['"', '\''];

	/// <summary>Character that opens/closes a raw string with no escaping (e.g. Go back-tick strings).</summary>
	public char? RawStringQuote { get; init; }
}
