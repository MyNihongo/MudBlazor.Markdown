using System;
using System.Reactive.Subjects;
using Blazored.LocalStorage;
using MudBlazor.Markdown.Core.Services.Interfaces;

namespace MudBlazor.Markdown.Core.Services
{
	internal sealed class ThemeService : IThemeService
	{
		private const string IsDarkKey = "HnT";

		private readonly ISyncLocalStorageService _localStorageService;

		private readonly ReplaySubject<bool> _isDarkThemeSubject = new(1);
		private bool _isDark;

		public ThemeService(ISyncLocalStorageService localStorageService)
		{
			_localStorageService = localStorageService;

			_isDark = localStorageService.GetItem<bool>(IsDarkKey);
			PublishTheme(_isDark);
		}

		public IObservable<bool> IsDarkTheme => _isDarkThemeSubject;

		public void ToggleTheme()
		{
			SetTheme(!_isDark);
		}

		private void SetTheme(bool isDark)
		{
			_localStorageService.SetItem(IsDarkKey, isDark);
			_isDark = isDark;

			PublishTheme(isDark);
		}

		private void PublishTheme(bool isDark)
		{
			_isDarkThemeSubject.OnNext(isDark);
		}
	}
}
