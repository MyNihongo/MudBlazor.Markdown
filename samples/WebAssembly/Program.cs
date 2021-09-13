using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Markdown.Core.Utils.ServiceRegistration;
using MudBlazor.Services;

namespace MudBlazor.Markdown.Wasm
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("#app");

			builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
			builder.Services.AddCoreServices();
			builder.Services.AddMudServices();
			builder.Services.AddMudMarkdownServices();

			await builder.Build().RunAsync()
				.ConfigureAwait(false);
		}
	}
}
