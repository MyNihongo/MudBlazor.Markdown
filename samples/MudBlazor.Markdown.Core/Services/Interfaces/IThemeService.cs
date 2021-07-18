using System;

namespace MudBlazor.Markdown.Core.Services.Interfaces
{
	public interface IThemeService
	{
		IObservable<bool> IsLightTheme { get; }

		void ToggleTheme();
	}
}
