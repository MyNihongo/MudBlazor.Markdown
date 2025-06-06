namespace MudBlazor.Markdown.Tests.Components.MarkdownComponentTests;

public sealed class MarkdownComponentMathShould : MarkdownComponentTestsBase
{
	[Fact]
	public void RenderInline()
	{
		const string value = "$\\sqrt {2}$";

		const string expected =
			"""
			<article id:ignore class='mud-markdown-body'>
				<p class='mud-typography mud-typography-body1'>\(\sqrt {2}\)</p>
			</article>
			""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderBlock()
	{
		const string value = "$$\\sqrt {2}$$";

		const string expected =
			"""
			<article id:ignore class='mud-markdown-body'>
				<p class='mud-typography mud-typography-body1'>$$\sqrt {2}$$</p>
			</article>
			""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}
}
