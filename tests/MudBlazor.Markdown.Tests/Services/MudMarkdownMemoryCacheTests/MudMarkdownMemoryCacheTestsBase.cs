using Microsoft.Extensions.Time.Testing;

namespace MudBlazor.Markdown.Tests.Services.MudMarkdownMemoryCacheTests;

public abstract class MudMarkdownMemoryCacheTestsBase : IDisposable
{
	private ServiceProvider? _serviceProvider;
	protected readonly FakeTimeProvider TimeProvider = new();

	protected TimeSpan? TimeToLive { get; set; }

	internal IMudMarkdownMemoryCache CreateFixture()
	{
		_serviceProvider ??= new ServiceCollection()
			.AddSingleton<TimeProvider>(TimeProvider)
			.AddMudMarkdownServices(opts =>
			{
				if (TimeToLive.HasValue)
					opts.TimeToLive = TimeToLive.Value;
			})
			.BuildServiceProvider();

		return _serviceProvider.GetRequiredService<IMudMarkdownMemoryCache>();
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);
		_serviceProvider?.Dispose();
	}
}
