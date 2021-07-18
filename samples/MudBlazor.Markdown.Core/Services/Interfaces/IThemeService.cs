using System;

namespace MudBlazor.Markdown.Core.Services.Interfaces
{
	public interface IThemeService
	{
		IObservable<bool> IsDarkTheme { get; }

		void ToggleTheme();
	}
}
