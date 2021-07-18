using System;
using System.Threading.Tasks;

namespace MudBlazor.Markdown.Core.Services.Interfaces
{
	public interface IThemeService
	{
		IObservable<bool> IsDarkTheme { get; }

		Task ToggleThemeAsync();
	}
}
