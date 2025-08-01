namespace MudBlazor;

/// <summary>
/// Defines how the copy button is displayed for a code block.
/// </summary>
public enum CodeBlockCopyButton
{
	/// <summary>
	/// No copy button is displayed.
	/// </summary>
	None = 0,

	/// <summary>
	/// The copy button is absolutely positioned and only visible when the user hovers over the code block.
	/// </summary>
	OnHover = 1,

	/// <summary>
	/// The copy button is always visible and positioned in a fixed location within the code block.
	/// </summary>
	Sticky = 2,
}
