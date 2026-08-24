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
		/// VS Code "Light+" (default light) theme.
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
			Meta = "#808080",
		};

		/// <summary>
		/// GitHub Light theme.
		/// </summary>
		public static readonly MudMarkdownTheme.CodeHighlight GitHub = new()
		{
			Background = "#FFFFFF",
			Text = "#24292E",
			Keyword = "#D73A49",
			String = "#032F62",
			Comment = "#6A737D",
			Function = "#6F42C1",
			Type = "#E36209",
			Numbers = "#005CC5",
			Meta = "#57606A",
		};

		/// <summary>
		/// Solarized Light theme.
		/// </summary>
		public static readonly MudMarkdownTheme.CodeHighlight Solarized = new()
		{
			Background = "#FDF6E3",
			Text = "#657B83",
			Keyword = "#859900",
			String = "#2AA198",
			Comment = "#93A1A1",
			Function = "#268BD2",
			Type = "#B58900",
			Numbers = "#D33682",
			Meta = "#657B83",
		};

		/// <summary>
		/// Atom "One Light" theme.
		/// </summary>
		public static readonly MudMarkdownTheme.CodeHighlight OneLight = new()
		{
			Background = "#FAFAFA",
			Text = "#383A42",
			Keyword = "#A626A4",
			String = "#50A14F",
			Comment = "#A0A1A7",
			Function = "#4078F2",
			Type = "#C18401",
			Numbers = "#986801",
			Meta = "#7F8C98",
		};

		/// <summary>
		/// Gruvbox Light theme.
		/// </summary>
		public static readonly MudMarkdownTheme.CodeHighlight Gruvbox = new()
		{
			Background = "#FBF1C7",
			Text = "#3C3836",
			Keyword = "#9D0006",
			String = "#79740E",
			Comment = "#928374",
			Function = "#427B58",
			Type = "#B57614",
			Numbers = "#8F3F71",
			Meta = "#7C6F64",
		};

		/// <summary>
		/// Ayu Light theme.
		/// </summary>
		public static readonly MudMarkdownTheme.CodeHighlight Ayu = new()
		{
			Background = "#FCFCFC",
			Text = "#5C6166",
			Keyword = "#FA8D3E",
			String = "#86B300",
			Comment = "#ABB0B6",
			Function = "#F2AE49",
			Type = "#55B4D4",
			Numbers = "#A37ACC",
			Meta = "#787B80",
		};

		/// <summary>
		/// VS Code "Quiet Light" theme.
		/// </summary>
		public static readonly MudMarkdownTheme.CodeHighlight QuietLight = new()
		{
			Background = "#F5F5F5",
			Text = "#333333",
			Keyword = "#4B69C6",
			String = "#448C27",
			Comment = "#AAAAAA",
			Function = "#AA3731",
			Type = "#7A3E9D",
			Numbers = "#9C5D27",
			Meta = "#7A7A7A",
		};

		/// <summary>
		/// Tomorrow theme.
		/// </summary>
		public static readonly MudMarkdownTheme.CodeHighlight Tomorrow = new()
		{
			Background = "#FFFFFF",
			Text = "#4D4D4C",
			Keyword = "#8959A8",
			String = "#718C00",
			Comment = "#8E908C",
			Function = "#4271AE",
			Type = "#EAB700",
			Numbers = "#F5871F",
			Meta = "#666666",
		};

		/// <summary>
		/// Catppuccin Latte theme.
		/// </summary>
		public static readonly MudMarkdownTheme.CodeHighlight CatppuccinLatte = new()
		{
			Background = "#EFF1F5",
			Text = "#4C4F69",
			Keyword = "#8839EF",
			String = "#40A02B",
			Comment = "#8C8FA1",
			Function = "#1E66F5",
			Type = "#DF8E1D",
			Numbers = "#FE640B",
			Meta = "#6C6F85",
		};

		/// <summary>
		/// Rosé Pine Dawn theme.
		/// </summary>
		public static readonly MudMarkdownTheme.CodeHighlight RosePineDawn = new()
		{
			Background = "#FAF4ED",
			Text = "#575279",
			Keyword = "#286983",
			String = "#EA9D34",
			Comment = "#9893A5",
			Function = "#D7827E",
			Type = "#56949F",
			Numbers = "#907AA9",
			Meta = "#797593",
		};

		/// <summary>
		/// PaperColor Light theme.
		/// </summary>
		public static readonly MudMarkdownTheme.CodeHighlight PaperColor = new()
		{
			Background = "#EEEEEE",
			Text = "#444444",
			Keyword = "#D70087",
			String = "#008700",
			Comment = "#878787",
			Function = "#005F87",
			Type = "#D75F00",
			Numbers = "#D70000",
			Meta = "#5F5F5F",
		};

		/// <summary>
		/// Xcode Light (default) theme.
		/// </summary>
		public static readonly MudMarkdownTheme.CodeHighlight Xcode = new()
		{
			Background = "#FFFFFF",
			Text = "#000000",
			Keyword = "#AA0D91",
			String = "#C41A16",
			Comment = "#007400",
			Function = "#6C36A9",
			Type = "#5C2699",
			Numbers = "#1C00CF",
			Meta = "#78492A",
		};

		/// <summary>
		/// Material Lighter theme.
		/// </summary>
		public static readonly MudMarkdownTheme.CodeHighlight Material = new()
		{
			Background = "#FAFAFA",
			Text = "#546E7A",
			Keyword = "#9C3EDA",
			String = "#91B859",
			Comment = "#90A4AE",
			Function = "#6182B8",
			Type = "#E2931D",
			Numbers = "#F76D47",
			Meta = "#8796B0",
		};
	}

	/// <summary>
	/// Palettes intended for use with a dark background.
	/// </summary>
	public static class Dark
	{
		/// <summary>
		/// VS Code "Dark+" (default dark) theme.
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
			Meta = "#808080",
		};

		/// <summary>
		/// Dracula theme.
		/// </summary>
		public static readonly MudMarkdownTheme.CodeHighlight Dracula = new()
		{
			Background = "#282A36",
			Text = "#F8F8F2",
			Keyword = "#FF79C6",
			String = "#F1FA8C",
			Comment = "#6272A4",
			Function = "#50FA7B",
			Type = "#8BE9FD",
			Numbers = "#BD93F9",
			Meta = "#FFB86C",
		};

		/// <summary>
		/// Monokai theme.
		/// </summary>
		public static readonly MudMarkdownTheme.CodeHighlight Monokai = new()
		{
			Background = "#272822",
			Text = "#F8F8F2",
			Keyword = "#F92672",
			String = "#E6DB74",
			Comment = "#75715E",
			Function = "#A6E22E",
			Type = "#66D9EF",
			Numbers = "#AE81FF",
			Meta = "#FD971F",
		};

		/// <summary>
		/// Atom "One Dark" theme.
		/// </summary>
		public static readonly MudMarkdownTheme.CodeHighlight OneDark = new()
		{
			Background = "#282C34",
			Text = "#ABB2BF",
			Keyword = "#C678DD",
			String = "#98C379",
			Comment = "#5C6370",
			Function = "#61AFEF",
			Type = "#E5C07B",
			Numbers = "#D19A66",
			Meta = "#56B6C2",
		};

		/// <summary>
		/// Nord theme.
		/// </summary>
		public static readonly MudMarkdownTheme.CodeHighlight Nord = new()
		{
			Background = "#2E3440",
			Text = "#D8DEE9",
			Keyword = "#81A1C1",
			String = "#A3BE8C",
			Comment = "#616E88",
			Function = "#88C0D0",
			Type = "#8FBCBB",
			Numbers = "#B48EAD",
			Meta = "#5E81AC",
		};

		/// <summary>
		/// Solarized Dark theme.
		/// </summary>
		public static readonly MudMarkdownTheme.CodeHighlight Solarized = new()
		{
			Background = "#002B36",
			Text = "#839496",
			Keyword = "#859900",
			String = "#2AA198",
			Comment = "#586E75",
			Function = "#268BD2",
			Type = "#B58900",
			Numbers = "#D33682",
			Meta = "#6C71C4",
		};

		/// <summary>
		/// Gruvbox Dark theme.
		/// </summary>
		public static readonly MudMarkdownTheme.CodeHighlight Gruvbox = new()
		{
			Background = "#282828",
			Text = "#EBDBB2",
			Keyword = "#FB4934",
			String = "#B8BB26",
			Comment = "#928374",
			Function = "#8EC07C",
			Type = "#FABD2F",
			Numbers = "#D3869B",
			Meta = "#83A598",
		};

		/// <summary>
		/// Tokyo Night theme.
		/// </summary>
		public static readonly MudMarkdownTheme.CodeHighlight TokyoNight = new()
		{
			Background = "#1A1B26",
			Text = "#C0CAF5",
			Keyword = "#BB9AF7",
			String = "#9ECE6A",
			Comment = "#565F89",
			Function = "#7AA2F7",
			Type = "#2AC3DE",
			Numbers = "#FF9E64",
			Meta = "#7DCFFF",
		};

		/// <summary>
		/// Night Owl theme.
		/// </summary>
		public static readonly MudMarkdownTheme.CodeHighlight NightOwl = new()
		{
			Background = "#011627",
			Text = "#D6DEEB",
			Keyword = "#C792EA",
			String = "#ECC48D",
			Comment = "#637777",
			Function = "#82AAFF",
			Type = "#FFCB8B",
			Numbers = "#F78C6C",
			Meta = "#7FDBCA",
		};

		/// <summary>
		/// Material (Dark) theme.
		/// </summary>
		public static readonly MudMarkdownTheme.CodeHighlight Material = new()
		{
			Background = "#263238",
			Text = "#EEFFFF",
			Keyword = "#C792EA",
			String = "#C3E88D",
			Comment = "#546E7A",
			Function = "#82AAFF",
			Type = "#FFCB6B",
			Numbers = "#F78C6C",
			Meta = "#89DDFF",
		};

		/// <summary>
		/// GitHub Dark theme.
		/// </summary>
		public static readonly MudMarkdownTheme.CodeHighlight GitHub = new()
		{
			Background = "#0D1117",
			Text = "#C9D1D9",
			Keyword = "#FF7B72",
			String = "#A5D6FF",
			Comment = "#8B949E",
			Function = "#D2A8FF",
			Type = "#FFA657",
			Numbers = "#79C0FF",
			Meta = "#FFA198",
		};

		/// <summary>
		/// Cobalt2 theme.
		/// </summary>
		public static readonly MudMarkdownTheme.CodeHighlight Cobalt2 = new()
		{
			Background = "#193549",
			Text = "#E1EFFF",
			Keyword = "#FF9D00",
			String = "#3AD900",
			Comment = "#0088FF",
			Function = "#FFC600",
			Type = "#80FCFF",
			Numbers = "#FF628C",
			Meta = "#8DA1B9",
		};

		/// <summary>
		/// Zenburn theme.
		/// </summary>
		public static readonly MudMarkdownTheme.CodeHighlight Zenburn = new()
		{
			Background = "#3F3F3F",
			Text = "#DCDCCC",
			Keyword = "#F0DFAF",
			String = "#CC9393",
			Comment = "#7F9F7F",
			Function = "#EFEF8F",
			Type = "#DFDFBF",
			Numbers = "#8CD0D3",
			Meta = "#DFAF8F",
		};

		/// <summary>
		/// Catppuccin Mocha theme.
		/// </summary>
		public static readonly MudMarkdownTheme.CodeHighlight CatppuccinMocha = new()
		{
			Background = "#1E1E2E",
			Text = "#CDD6F4",
			Keyword = "#CBA6F7",
			String = "#A6E3A1",
			Comment = "#6C7086",
			Function = "#89B4FA",
			Type = "#F9E2AF",
			Numbers = "#FAB387",
			Meta = "#94E2D5",
		};

		/// <summary>
		/// Rosé Pine theme.
		/// </summary>
		public static readonly MudMarkdownTheme.CodeHighlight RosePine = new()
		{
			Background = "#191724",
			Text = "#E0DEF4",
			Keyword = "#31748F",
			String = "#F6C177",
			Comment = "#6E6A86",
			Function = "#EBBCBA",
			Type = "#9CCFD8",
			Numbers = "#C4A7E7",
			Meta = "#EB6F92",
		};

		/// <summary>
		/// Ayu Dark theme.
		/// </summary>
		public static readonly MudMarkdownTheme.CodeHighlight Ayu = new()
		{
			Background = "#0F1419",
			Text = "#BFBDB6",
			Keyword = "#FF8F40",
			String = "#C2D94C",
			Comment = "#5C6773",
			Function = "#FFB454",
			Type = "#59C2FF",
			Numbers = "#E6B450",
			Meta = "#F29668",
		};
	}
}
