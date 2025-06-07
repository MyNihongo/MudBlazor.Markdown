﻿namespace MudBlazor;

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
		public bool IsStriped { get; set; } = true;

		/// <summary>
		/// If true, table's cells will have left/right borders.
		/// </summary>
		public bool IsBordered { get; set; } = true;

		/// <summary>
		/// Child content of component.
		/// </summary>
		public int Elevation { set; get; } = 1;

		/// <summary>
		/// Minimum width (in pixels) for a table cell.<br/>
		/// If <see langword="null" /> or negative the minimum width is not applied.
		/// </summary>
		public int? CellMinWidth { get; set; }
	}

	/// <summary>
	/// Styling properties for the link.
	/// </summary>
	public sealed class LinkStyling
	{
		/// <summary>
		/// Underline style.
		/// </summary>
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
