using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MudBlazor;

public static class ServiceCollectionEx
{
	extension(IServiceCollection @this)
	{
		public IServiceCollection AddMudMarkdownServices(Action<MudMarkdownMemoryCacheOptions>? configureMemoryCache = null)
		{
			return @this
				.AddMudMarkdownCache(configureMemoryCache)
				.AddSingleton<IMudMarkdownThemeService, MudMarkdownThemeService>()
				.AddSingleton<IMudMarkdownValueProvider, MudMarkdownValueProvider>();
		}

		private IServiceCollection AddMudMarkdownCache(Action<MudMarkdownMemoryCacheOptions>? configureMemoryCache)
		{
			return @this
				.AddOptions()
				.Configure<MudMarkdownMemoryCacheOptions>(options =>
				{
					if (configureMemoryCache is not null)
						configureMemoryCache(options);
					else
						options.TimeToLive = TimeSpan.FromHours(1);
				})
				.TryAddSingletonEx(TimeProvider.System)
				.AddSingleton<IMudMarkdownMemoryCache, MudMarkdownMemoryCache>();
		}

		public IServiceCollection AddMudMarkdownClipboardService<T>()
			where T : class, IMudMarkdownClipboardService
		{
			return @this.AddScoped<IMudMarkdownClipboardService, T>();
		}

		private IServiceCollection TryAddSingletonEx<T>(T instance)
			where T : class
		{
			@this.TryAddSingleton(instance);
			return @this;
		}
	}
}
