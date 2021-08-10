using System;
using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace MudBlazor.Markdown.Tests.Services
{
	public sealed class TestNavigationManager : NavigationManager
	{
		public void Initialize(string uri)
		{
			const string url = "http://localhost:1234/";

#if DEBUG
			var isInitialised = (bool) typeof(NavigationManager)
				.GetField("_isInitialized", BindingFlags.Instance | BindingFlags.NonPublic)
				!.GetValue(this)!;

			if (!isInitialised)
#endif
				Initialize(url, url + uri);
		}

		protected override void NavigateToCore(string uri, bool forceLoad)
		{
			throw new NotImplementedException();
		}
	}
}
