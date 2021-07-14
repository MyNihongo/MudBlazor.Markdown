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
			const string expectedValue = "<article class=\"mud-markdown-body\"><p class=\"mud-typography mud-typography-body1 mud-inherit-text\">Some text <code>code</code> again text - <i>italics</i> text and <b>bold</b> text.</p></article>";

			using var fixture = CreateFixture(value);
			fixture.MarkupMatches(expectedValue);
		}

		[Fact]
		public void RenderBlockQuotes()
		{
			const string value = ">Some text `code` again text - *italics* text and **bold** text.";
			const string expectedValue = "<article class=\"mud-markdown-body\"><blockquote><p class=\"mud-typography mud-typography-body1 mud-inherit-text\">Some text <code>code</code> again text - <i>italics</i> text and <b>bold</b> text.</p></blockquote></article>";

			using var fixture = CreateFixture(value);
			fixture.MarkupMatches(expectedValue);
		}

		[Theory]
		[InlineData("\r\n")]
		[InlineData("\n")]
		public void ReplaceNewLineSymbols(string newLine)
		{
			var value = "line1" + newLine + "line2";
			const string expectedValue = "<article class=\"mud-markdown-body\"><p class=\"mud-typography mud-typography-body1 mud-inherit-text\">line1<br />line2</p></article>";

			using var fixture = CreateFixture(value);
			fixture.MarkupMatches(expectedValue);
		}
	}
}
