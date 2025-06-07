namespace MudBlazor;

public static class ServiceCollectionEx
{
	public static IServiceCollection AddMudMarkdownServices(this IServiceCollection @this, Action<MudMarkdownMemoryCacheOptions>? configureMemoryCache = null)
	{
		return @this
			.AddMudMarkdownCache(configureMemoryCache)
			.AddScoped<IMudMarkdownThemeService, MudMarkdownThemeService>()
			.AddSingleton<IMudMarkdownValueProvider, MudMarkdownValueProvider>();
	}

	private static IServiceCollection AddMudMarkdownCache(this IServiceCollection @this, Action<MudMarkdownMemoryCacheOptions>? configureMemoryCache)
	{
		return @this
			.AddOptions()
			.Configure<MudMarkdownMemoryCacheOptions>(options =>
			{
				if (configureMemoryCache != null)
					configureMemoryCache(options);
				else
					options.TimeToLive = TimeSpan.FromHours(1);
			})
			.AddSingleton<IMudMarkdownMemoryCache, MudMarkdownMemoryCache>();
	}

	public static IServiceCollection AddMudMarkdownClipboardService<T>(this IServiceCollection @this)
		where T : class, IMudMarkdownClipboardService
	{
		return @this.AddScoped<IMudMarkdownClipboardService, T>();
	}
}
