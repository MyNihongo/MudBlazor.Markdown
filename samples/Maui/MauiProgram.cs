using Microsoft.Extensions.Logging;
using MudBlazor.Markdown.Core.Utils.ServiceRegistration;
using MudBlazor.Markdown.Maui.Services;
using MudBlazor.Services;

namespace MudBlazor.Markdown.Maui
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				});

			builder.Services.AddMauiBlazorWebView();
			builder.Services.AddCoreServices();
			builder.Services.AddMudServices();
			builder.Services.AddMudMarkdownServices();
			builder.Services.AddMudMarkdownClipboardService<MauiClipboardService>();

#if DEBUG
			builder.Services.AddBlazorWebViewDeveloperTools();
			builder.Logging.AddDebug();
#endif

			return builder.Build();
		}
	}
}
