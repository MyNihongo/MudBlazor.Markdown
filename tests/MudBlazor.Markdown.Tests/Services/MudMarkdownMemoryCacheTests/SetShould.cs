namespace MudBlazor.Markdown.Tests.Services.MudMarkdownMemoryCacheTests;

public sealed class SetShould : MudMarkdownMemoryCacheTestsBase
{
	[Fact]
	public void SetDifferentKeys()
	{
		const string key1 = nameof(key1),
			key2 = nameof(key2),
			value = nameof(value);

		var fixture = CreateFixture();
		fixture.Set(key1, value);
		fixture.Set(key2, value);

		fixture
			.Should()
			.HaveKeys([key1, key2]);
	}

	[Fact]
	public void SetSameKeys()
	{
		const string key = nameof(key), value = nameof(value);

		var fixture = CreateFixture();
		fixture.Set(key, value);
		fixture.Set(key, value);
		fixture.Set(key, value);

		fixture
			.Should()
			.HaveSingleCacheEntry(key);
	}
}
