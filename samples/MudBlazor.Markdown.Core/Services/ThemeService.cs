using System.Reactive.Subjects;
using Blazored.LocalStorage;

namespace MudBlazor.Markdown.Core.Services;

internal sealed class ThemeService : IThemeService
{
	private const string IsDarkKey = "HnT";

	private readonly ILocalStorageService _localStorageService;
	private readonly IMudMarkdownThemeService _mudMarkdownThemeService;

	private readonly ReplaySubject<bool> _isDarkThemeSubject = new(1);
	private bool _isDark;

	public ThemeService(
		ILocalStorageService localStorageService,
		IMudMarkdownThemeService mudMarkdownThemeService)
	{
		_localStorageService = localStorageService;
		_mudMarkdownThemeService = mudMarkdownThemeService;

		// Sync service is not available in Blazor.Server
		Task.Run(async () =>
		{
			_isDark = await localStorageService.GetItemAsync<bool>(IsDarkKey)
				.ConfigureAwait(false);

			PublishTheme(_isDark);
		});
	}

	public IObservable<bool> IsDarkTheme => _isDarkThemeSubject;

	public async Task ToggleThemeAsync()
	{
		_isDark = !_isDark;

		await _localStorageService.SetItemAsync(IsDarkKey, _isDark)
			.ConfigureAwait(false);

		PublishTheme(_isDark);
	}

	private void PublishTheme(bool isDark)
	{
		var codeBlockTheme = isDark ? CodeBlockTheme.DraculaBase16 : CodeBlockTheme.Monokai;
		_mudMarkdownThemeService.SetCodeBlockTheme(codeBlockTheme);

		_isDarkThemeSubject.OnNext(isDark);
	}
}
