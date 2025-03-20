namespace MudBlazor;

/// <summary>
/// The type of source for the markdown content.
/// </summary>
public enum MarkdownSourceType
{
	/// <summary>
	/// The raw markdown content as a string.
	/// </summary>
	RawValue,

	/// <summary>
	/// The markdown content is loaded from a file.
	/// </summary>
	File,

	/// <summary>
	/// The markdown content is loaded from a URL.
	/// </summary>
	Url,
}
