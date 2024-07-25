namespace MudBlazor;

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

	public sealed class TableStyling
	{
		/// <summary>
		/// If true, striped table rows will be used.
		/// </summary>
		public bool IsStriped { get; set; } = true;

		/// <summary>
		/// If true, table's cells will have left/right borders.
		/// </summary>
		public bool IsBordered { get; set; } = true;

		/// <summary>
		/// Child content of component.
		/// </summary>
		public int Elevation { set; get; } = 1;
	}

	public sealed class LinkStyling
	{
		public Underline Underline { get; set; } = Underline.Hover;
	}
}
