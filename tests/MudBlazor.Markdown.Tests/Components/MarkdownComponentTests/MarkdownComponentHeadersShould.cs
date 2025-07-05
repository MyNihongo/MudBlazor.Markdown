namespace MudBlazor.Markdown.Tests.Components.MarkdownComponentTests;

public sealed class MarkdownComponentHeadersShould : MarkdownComponentTestsBase
{
	[Theory]
	[InlineData("#", "h1")]
	[InlineData("##", "h2")]
	[InlineData("###", "h3")]
	[InlineData("####", "h4")]
	[InlineData("#####", "h5")]
	[InlineData("######", "h6")]
	public void RenderHeaders(string valueInput, string expectedTag)
	{
		var value = valueInput + " some text";

		var expected =
			$"""
			 <article id:ignore class='mud-markdown-body'>
			 	<{expectedTag} id='some-text' class='mud-typography mud-typography-{expectedTag}'>
			 		some text
			 	</{expectedTag}>
			 </article>
			 """;

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}
}
