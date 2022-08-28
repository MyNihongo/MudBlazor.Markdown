namespace MudBlazor.Markdown.Server;

public class Program
{
	public static Task Main(string[] args)
	{
		return CreateHostBuilder(args).Build().RunAsync();
	}

	public static IHostBuilder CreateHostBuilder(string[] args) =>
		Host.CreateDefaultBuilder(args)
			.ConfigureWebHostDefaults(webBuilder =>
			{
				webBuilder.UseStartup<Startup>();
			});
}
