namespace MudBlazor.Markdown.Tests.MarkdownComponentTests;

public sealed class MarkdownComponentDetailsShould : MarkdownComponentTestsBase
{
	[Fact]
	public void RenderDetailsMultiline()
	{
		const string value =
@"<details>
	<summary>Header</summary>
	Some hidden text
	Another text
</details>";

		const string expected = "";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderDetailsSummaryMultiLine()
	{
		const string value =
@"<details>
	<summary>
		Header
	</summary>
	Some hidden text
	Another text
</details>";

		const string expected = "";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderDetailsInline()
	{
		const string value =
@"<details><summary>Header</summary>Some hidden text</details>";

		const string expected = "";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}
}
