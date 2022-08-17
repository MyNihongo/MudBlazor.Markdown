namespace MudBlazor.Markdown.Tests.MarkdownComponentTests;

public sealed class MarkdownComponentDetailsShould : MarkdownComponentTestsBase
{
	[Fact]
	public void RenderDetails()
	{
		const string value =
@"<details>
	<summary>Header</summary>
	Some hidden text
</details>";

		const string expected = "";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}
}
