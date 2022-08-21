namespace MudBlazor;

internal readonly ref struct HtmlDetailsData
{
	public HtmlDetailsData(string header, string data)
	{
		Header = header;
		Data = data;
	}

	public string Header { get; }

	public string Data { get; }
}
