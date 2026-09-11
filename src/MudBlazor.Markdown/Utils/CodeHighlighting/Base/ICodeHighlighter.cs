namespace MudBlazor;

/// <summary>
/// Parses source code into a tree of <see cref="CodeNode"/>s that map to highlighted token spans.
/// </summary>
internal interface ICodeHighlighter
{
	/// <summary>
	/// Tokenizes <paramref name="code"/> into highlighted nodes.
	/// </summary>
	IReadOnlyList<CodeNode> Highlight(string code);
}
