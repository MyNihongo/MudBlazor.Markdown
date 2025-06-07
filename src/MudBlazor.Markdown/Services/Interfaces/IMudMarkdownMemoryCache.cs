namespace MudBlazor;

internal interface IMudMarkdownMemoryCache
{
	public bool TryGetValue(in string key, out string value);

	public void Set(in string key, in string value);
}
