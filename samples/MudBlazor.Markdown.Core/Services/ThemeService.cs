using System;
using System.Reactive.Subjects;
using MudBlazor.Markdown.Core.Services.Interfaces;

namespace MudBlazor.Markdown.Core.Services
{
	internal sealed class ThemeService : IThemeService
	{
		private bool _isLight;
		private readonly ReplaySubject<bool> _isLightThemeSubject = new(1);

		public ThemeService()
		{
			SetTheme(true);
		}

		public IObservable<bool> IsLightTheme => _isLightThemeSubject;

		public void ToggleTheme()
		{
			SetTheme(!_isLight);
		}

		private void SetTheme(bool isLight)
		{
			_isLight = isLight;
			_isLightThemeSubject.OnNext(isLight);
		}
	}
}
