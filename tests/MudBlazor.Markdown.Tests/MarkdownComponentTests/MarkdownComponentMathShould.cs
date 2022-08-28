namespace MudBlazor.Markdown.Tests.MarkdownComponentTests;

public sealed class MarkdownComponentMathShould : MarkdownComponentTestsBase
{
	[Fact]
	public void RenderInline()
	{
		const string value = "$\\sqrt {2}$";

		const string expected = "";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderBlock()
	{
		const string value = "$$\\sqrt {2}$$";

		const string expected = "";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}
}
