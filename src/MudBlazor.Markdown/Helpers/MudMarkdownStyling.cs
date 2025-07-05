namespace MudBlazor;

/// <summary>
/// Provides styling options for Markdown elements.
/// </summary>
public sealed class MudMarkdownStyling
{
	/// <summary>
	/// Styling properties for the table.
	/// </summary>
	public TableStyling Table { get; } = new();

	/// <summary>
	/// Styling properties for the link.
	/// </summary>
	public LinkStyling Link { get; } = new();

	/// <summary>
	/// Styling properties for the code block.
	/// </summary>
	public CodeBlockStyling CodeBlock { get; } = new();

	/// <summary>
	/// Styling properties for the table.
	/// </summary>
	public sealed class TableStyling
	{
		/// <summary>
		/// If true, striped table rows will be used.
		/// </summary>
		/// <remarks>
		/// Defaults to <c>true</c>.
		/// </remarks>
		public bool IsStriped { get; set; } = true;

		/// <summary>
		/// If true, table's cells will have left/right borders.
		/// </summary>
		/// <remarks>
		/// Defaults to <c>true</c>.
		/// </remarks>
		public bool IsBordered { get; set; } = true;

		/// <summary>
		/// Child content of component.
		/// </summary>
		/// <remarks>
		/// Defaults to <c>1</c>.
		/// </remarks>
		public int Elevation { set; get; } = 1;

		/// <summary>
		/// Minimum width (in pixels) for a table cell.<br/>
		/// If <see langword="null" /> or negative the minimum width is not applied.
		/// </summary>
		/// <remarks>
		/// Defaults to <c>null</c>.
		/// </remarks>
		public int? CellMinWidth { get; set; }

		/// <summary>
		/// Uses compact padding for all rows.
		/// </summary>
		/// <remarks>
		/// Defaults to <c>false</c>.
		/// </remarks>
		public bool Dense { get; set; }
	}

	/// <summary>
	/// Styling properties for the link.
	/// </summary>
	public sealed class LinkStyling
	{
		/// <summary>
		/// Underline style.
		/// </summary>
		/// <remarks>
		/// Defaults to <c>Hover</c>.
		/// </remarks>
		public Underline Underline { get; set; } = Underline.Hover;
	}

	/// <summary>
	/// Styling properties for the code block.
	/// </summary>
	public sealed class CodeBlockStyling
	{
		/// <summary>
		/// Theme of the code block.<br/>
		/// Browse available themes here: https://highlightjs.org/static/demo/
		/// </summary>
		public CodeBlockTheme Theme { get; set; }
	}
}
