namespace MudBlazor.Markdown.Core.Services;

public interface IThemeService
{
	IObservable<bool> IsDarkTheme { get; }

	Task ToggleThemeAsync();
}
