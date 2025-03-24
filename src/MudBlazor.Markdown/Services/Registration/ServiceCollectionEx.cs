namespace MudBlazor;

public static class ServiceCollectionEx
{
	public static IServiceCollection AddMudMarkdownServices(this IServiceCollection @this, Action<MudMarkdownMemoryCacheEntryOptions>? configureMemoryCache)
	{
		return @this
			.AddMudMarkdownCache(configureMemoryCache)
			.AddScoped<IMudMarkdownThemeService, MudMarkdownThemeService>()
			.AddSingleton<IMudMarkdownValueProvider, MudMarkdownValueProvider>();
	}

	private static IServiceCollection AddMudMarkdownCache(this IServiceCollection @this, Action<MudMarkdownMemoryCacheEntryOptions>? configureMemoryCache)
	{
		return @this
			.AddMemoryCache()
			.Configure<MudMarkdownMemoryCacheEntryOptions>(options =>
			{
				if (configureMemoryCache != null)
				{
					configureMemoryCache(options);
				}
				else
				{
					options.SlidingExpiration = TimeSpan.FromHours(1);
				}
			});
	}

	public static IServiceCollection AddMudMarkdownClipboardService<T>(this IServiceCollection @this)
		where T : class, IMudMarkdownClipboardService
	{
		return @this.AddScoped<IMudMarkdownClipboardService, T>();
	}
}
