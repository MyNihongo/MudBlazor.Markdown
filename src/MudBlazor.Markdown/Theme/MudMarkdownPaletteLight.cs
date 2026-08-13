namespace MudBlazor;

/// <summary>
/// <see cref="PaletteLight"/> extended with code-highlight colors (<see cref="IMudMarkdownPalette"/>).<br/>
/// Assign it to <c>MudTheme.PaletteLight</c> to color code blocks when the light theme is active.
/// </summary>
public class MudMarkdownPaletteLight : PaletteLight, IMudMarkdownPalette
{
	/// <inheritdoc />
	/// <remarks>
	/// Defaults to <see cref="MudCodeHighlightThemes.Light.LightPlus"/> (VS Code "Light+").
	/// </remarks>
	public MudMarkdownTheme.CodeHighlight CodeHighlight { get; set; } = MudCodeHighlightThemes.Light.LightPlus;
}
