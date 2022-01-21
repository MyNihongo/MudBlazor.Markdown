namespace MudBlazor;

public static class ServiceCollectionEx
{
	public static IServiceCollection AddMudMarkdownServices(this IServiceCollection @this)
	{
		@this.AddScoped<IMudMarkdownThemeService, MudMarkdownThemeService>();

		return @this;
	}
}