using Bunit;
using Xunit;

namespace MudBlazor.Markdown.Tests
{
	public sealed class MudMarkdownTests : MudMarkdownTestsBase
	{
		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData(" ")]
		public void RenderNothingIfNullOrWhitespace(string value)
		{
			using var fixture = CreateFixture(value);
			fixture.MarkupMatches(string.Empty);
		}

		[Fact]
		public void RenderCodeItalicAndBold()
		{
			const string value = "Some text `code` again text - *italics* text and **bold** text.";
			const string expectedValue = "<p class=\"mud-typography mud-typography-body1 mud-inherit-text\">Some text <code class=\"mud-markdown-code\">code</code> again text - <i>italics</i> text and <b>bold</b> text.</p>";

			using var ctx = new TestContext();

			using var fixture = ctx.RenderComponent<MudMarkdown>(parameters => parameters
				.Add(x => x.Value, value));

			fixture.MarkupMatches(expectedValue);
		}
	}
}
