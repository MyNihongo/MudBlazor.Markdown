namespace MudBlazor.Markdown.Tests.Services.MudMarkdownMemoryCacheTests;

public sealed class TryGetValueShould : MudMarkdownMemoryCacheTestsBase
{
	[Fact]
	public void ReturnFalseIfNotSet()
	{
		const string key = nameof(key);

		var fixture = CreateFixture();
		var actual = fixture.TryGetValue(key, out var actualValue);

		actual
			.Should()
			.BeFalse();

		actualValue
			.Should()
			.BeEmpty();

		fixture
			.Should()
			.HaveEmptyCache();
	}

	[Fact]
	public void ReturnTrueIfSet()
	{
		const string key = nameof(key), value = nameof(value);

		var fixture = CreateFixture();
		fixture.Set(key, value);

		var actual = fixture.TryGetValue(key, out var actualValue);

		actual
			.Should()
			.BeTrue();

		actualValue
			.Should()
			.Be(value);

		fixture
			.Should()
			.HaveSingleCacheEntry(key);
	}

	[Fact]
	public async Task ReturnFalseAfterExpiration()
	{
		TimeToLive = TimeSpan.FromSeconds(1);
		const string key = nameof(key), value = nameof(value);

		var fixture = CreateFixture();
		fixture.Set(key, value);

		await Task.Delay(TimeToLive.Value);

		var actual = fixture.TryGetValue(key, out var actualValue);

		actual
			.Should()
			.BeFalse();

		actualValue
			.Should()
			.BeEmpty();

		fixture
			.Should()
			.HaveEmptyCache();
	}
}
