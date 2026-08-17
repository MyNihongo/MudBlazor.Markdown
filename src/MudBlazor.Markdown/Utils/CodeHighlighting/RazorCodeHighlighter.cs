namespace MudBlazor;

/// <summary>
/// Highlighter for Razor (<c>.razor</c> / <c>.cshtml</c>).<br/>
/// Reuses the C# token rules and adds the Razor comment syntax (<c>@* ... *@</c>).<br/>
/// Note: HTML markup around the C# is not tokenized separately yet.
/// </summary>
internal sealed class RazorCodeHighlighter : CSharpCodeHighlighter
{
	protected override IReadOnlyList<(string Start, string End)> BlockComments { get; } =
	[
		("@*", "*@"),
		("/*", "*/"),
	];
}
