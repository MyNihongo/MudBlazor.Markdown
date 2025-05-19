namespace MudBlazor;

internal readonly struct HeadingContent
{
	public readonly string Id, Text;

	public HeadingContent(string id, string text)
	{
		Id = id;
		Text = text;
	}
}
