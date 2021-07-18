using Blazored.LocalStorage;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Markdown.Core.Services;
using MudBlazor.Markdown.Core.Services.Interfaces;

namespace MudBlazor.Markdown.Core.Utils.ServiceRegistration
{
	public static class ServiceCollectionEx
	{
		public static IServiceCollection AddCoreServices(this IServiceCollection @this) =>
			@this
				.AddSingleton<IMarkdownService>(new MarkdownService())
				.AddScoped<IThemeService, ThemeService>()
				.AddBlazoredLocalStorage();
	}
}
