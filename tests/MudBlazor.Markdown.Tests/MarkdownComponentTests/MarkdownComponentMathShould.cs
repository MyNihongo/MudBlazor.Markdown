namespace MudBlazor.Markdown.Tests.MarkdownComponentTests;

public sealed class MarkdownComponentMathShould : MarkdownComponentTestsBase
{
	[Fact]
	public void RenderNotEquals()
	{
		const string value = "$a \\ne 0$";
		const string expected = "";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}
}
