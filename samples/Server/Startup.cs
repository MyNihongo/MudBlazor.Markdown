using Microsoft.Extensions.Options;
using MudBlazor.Markdown.Core.Utils.ServiceRegistration;
using MudBlazor.Services;

namespace MudBlazor.Markdown.Server;

public class Startup
{
	public Startup(IConfiguration configuration)
	{
		Configuration = configuration;
	}

	public IConfiguration Configuration { get; }

	public void ConfigureServices(IServiceCollection services)
	{
		services.AddRazorPages();
		services.AddServerSideBlazor();
		services.AddCoreServices();
		services.AddMudServices();
		services.AddMudMarkdownServices(static cache =>
		{
			cache.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
			cache.SlidingExpiration = TimeSpan.FromHours(2);
		});
	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}
		else
		{
			app.UseExceptionHandler("/Error");
		}

		app.UseStaticFiles();
		app.UseRouting();

		app.UseEndpoints(endpoints =>
		{
			endpoints.MapBlazorHub();
			endpoints.MapFallbackToPage("/_Host");
		});
	}
}
