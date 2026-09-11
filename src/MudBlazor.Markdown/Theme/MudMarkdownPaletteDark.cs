namespace MudBlazor;

/// <summary>
/// <see cref="PaletteDark"/> extended with code-highlight colors (<see cref="IMudMarkdownPalette"/>).<br/>
/// Assign it to <c>MudTheme.PaletteDark</c> to color code blocks when the dark theme is active.
/// </summary>
public class MudMarkdownPaletteDark : PaletteDark, IMudMarkdownPalette
{
	/// <inheritdoc />
	/// <remarks>
	/// Defaults to <see cref="MudCodeHighlightThemes.Dark.DarkPlus"/> (VS Code "Dark+").
	/// </remarks>
	public MudMarkdownTheme.CodeHighlight CodeHighlight { get; set; } = MudCodeHighlightThemes.Dark.DarkPlus;
}
