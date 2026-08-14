namespace MudBlazor;

/// <summary>
/// Ready-made <see cref="MudMarkdownTheme.CodeHighlight"/> palettes that can be assigned to
/// <see cref="IMudMarkdownPalette.CodeHighlight"/>.
/// </summary>
public static class MudCodeHighlightThemes
{
	/// <summary>
	/// Palettes intended for use with a light background.
	/// </summary>
	public static class Light
	{
		/// <summary>
		/// VS Code "Light+" (default light) theme colors.
		/// </summary>
		public static readonly MudMarkdownTheme.CodeHighlight LightPlus = new()
		{
			Background = "#FFFFFF",
			Text = "#000000",
			Keyword = "#0000FF",
			String = "#A31515",
			Comment = "#008000",
			Function = "#795E26",
			Type = "#267F99",
			Numbers = "#098658",
		};
	}

	/// <summary>
	/// Palettes intended for use with a dark background.
	/// </summary>
	public static class Dark
	{
		/// <summary>
		/// VS Code "Dark+" (default dark) theme colors.
		/// </summary>
		public static readonly MudMarkdownTheme.CodeHighlight DarkPlus = new()
		{
			Background = "#1E1E1E",
			Text = "#D4D4D4",
			Keyword = "#569CD6",
			String = "#CE9178",
			Comment = "#6A9955",
			Function = "#DCDCAA",
			Type = "#4EC9B0",
			Numbers = "#B5CEA8",
		};
	}
}
