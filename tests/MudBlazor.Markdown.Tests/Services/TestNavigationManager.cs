using System.Reflection;

namespace MudBlazor.Markdown.Tests.Services;

public sealed class TestNavigationManager : NavigationManager
{
	internal const string TestUrl = "http://localhost:1234/";

	public void Initialize(string uri)
	{
#if DEBUG
		var isInitialised = (bool)typeof(NavigationManager)
			.GetField("_isInitialized", BindingFlags.Instance | BindingFlags.NonPublic)
			!.GetValue(this)!;

		if (!isInitialised)
#endif
			Initialize(TestUrl, TestUrl + uri);
	}

	protected override void NavigateToCore(string uri, bool forceLoad)
	{
		var newUri = new Uri(BaseUri);
		newUri = new Uri(newUri, uri);

		Uri = newUri.AbsoluteUri;
	}
}