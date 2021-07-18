using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using MudBlazor.Markdown.Core.Services.Interfaces;

namespace MudBlazor.Markdown.Core.Services
{
	internal sealed class ThemeService : IThemeService
	{
		private const string IsDarkKey = "HnT";

		private readonly ILocalStorageService _localStorageService;

		private readonly ReplaySubject<bool> _isDarkThemeSubject = new(1);
		private bool _isDark;

		public ThemeService(ILocalStorageService localStorageService)
		{
			_localStorageService = localStorageService;

			// TODO:
			//_isDark = localStorageService.GetItem<bool>(IsDarkKey);
			PublishTheme(_isDark);
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
			_isDarkThemeSubject.OnNext(isDark);
		}
	}
}
