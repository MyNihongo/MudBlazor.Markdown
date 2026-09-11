namespace MudBlazor;

/// <summary>
/// Extends a MudBlazor palette with the colors used to highlight code blocks.<br/>
/// Implemented by <see cref="MudMarkdownPaletteLight"/> and <see cref="MudMarkdownPaletteDark"/> so the
/// code-highlight colors travel with the active <c>MudTheme</c> palette (light or dark).
/// </summary>
public interface IMudMarkdownPalette
{
	/// <summary>
	/// Colors that map source-code tokens to the highlighting displayed in a code block.
	/// </summary>
	public MudMarkdownTheme.CodeHighlight CodeHighlight { get; set; }
}
