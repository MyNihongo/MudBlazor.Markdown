namespace MudBlazor.Markdown.Tests.MarkdownComponentTests;

public sealed class MarkdownComponentMathShould : MarkdownComponentTestsBase
{
	[Fact]
	public void RenderEquals()
	{
		const string value = "$x = y$";
		const string expected = "";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}
}
