namespace MudBlazor.Markdown.Tests.Utils.ServiceCollectionExTests;

public sealed class AddMudMarkdownServicesShould : ServiceCollectionExTestsBase
{
	[Fact]
	public void NotRegisterDefaultMemoryCache()
	{
		using var fixture = CreateFixture()
			.AddMudMarkdownServices()
			.BuildServiceProvider();

		fixture
			.GetService<IMemoryCache>()
			.Should()
			.BeNull();
	}

	[Fact]
	public void RegisterCustomMemoryCache()
	{
		using var fixture = CreateFixture()
			.AddMudMarkdownServices()
			.BuildServiceProvider();

		fixture
			.GetService<IMudMarkdownMemoryCache>()
			.Should()
			.BeOfType<MudMarkdownMemoryCache>();

		var instance1 = fixture.GetRequiredService<IMudMarkdownMemoryCache>();
		var instance2 = fixture.GetRequiredService<IMudMarkdownMemoryCache>();

		using var scope = fixture.CreateScope();
		var scopedInstance = scope.ServiceProvider.GetRequiredService<IMudMarkdownMemoryCache>();

		instance1
			.Should()
			.Be(instance2);

		instance1
			.Should()
			.Be(scopedInstance);
	}

	[Fact]
	public void RegisterScopedMemoryCache()
	{
		using var fixture = CreateFixture()
			.AddScoped<IMemoryCache, MemoryCache>()
			.AddMudMarkdownServices()
			.BuildServiceProvider();

		var instance = fixture.GetRequiredService<IMudMarkdownMemoryCache>();

		using var scope = fixture.CreateScope();
		var scopedInstance = scope.ServiceProvider.GetRequiredService<IMemoryCache>();

		instance
			.Should()
			.NotBe(scopedInstance);
	}

	[Fact]
	public void RegisterServices()
	{
		using var serviceProvider = new ServiceCollection()
			.AddMudMarkdownServices()
			.BuildServiceProvider();

		serviceProvider.GetRequiredService<IMudMarkdownMemoryCache>()
			.Should()
			.BeOfType<MudMarkdownMemoryCache>();

		serviceProvider.GetService<IMemoryCache>()
			.Should()
			.BeNull();

		serviceProvider.GetRequiredService<IMudMarkdownThemeService>()
			.Should()
			.BeOfType<MudMarkdownThemeService>();

		serviceProvider.GetRequiredService<IMudMarkdownValueProvider>()
			.Should()
			.BeOfType<MudMarkdownValueProvider>();
	}
}
