using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace MudBlazor
{
	public static class ServiceCollectionEx
	{
		public static IServiceCollection AddMubMarkdownServices(this IServiceCollection @this)
		{
			@this.AddScoped<IMudMarkdownThemeService, MudMarkdownThemeService>();

			return @this;
		}
	}
}
