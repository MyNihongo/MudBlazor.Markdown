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
			const string expectedValue = "<article class='mud-markdown-body'><p class='mud-typography mud-typography-body1 mud-inherit-text'>Some text <code>code</code> again text - <i>italics</i> text and <b>bold</b> text.</p></article>";

			using var fixture = CreateFixture(value);
			fixture.MarkupMatches(expectedValue);
		}

		[Fact]
		public void RenderBlockQuotes()
		{
			const string value = ">Some text `code` again text - *italics* text and **bold** text.";
			const string expectedValue = "<article class='mud-markdown-body'><blockquote><p class='mud-typography mud-typography-body1 mud-inherit-text'>Some text <code>code</code> again text - <i>italics</i> text and <b>bold</b> text.</p></blockquote></article>";

			using var fixture = CreateFixture(value);
			fixture.MarkupMatches(expectedValue);
		}

		[Theory]
		[InlineData("\r\n")]
		[InlineData("\n")]
		public void ReplaceNewLineSymbols(string newLine)
		{
			var value = "line1" + newLine + "line2";
			const string expectedValue = "<article class='mud-markdown-body'><p class='mud-typography mud-typography-body1 mud-inherit-text'>line1<br />line2</p></article>";

			using var fixture = CreateFixture(value);
			fixture.MarkupMatches(expectedValue);
		}

		[Fact]
		public void RenderLink()
		{
			const string value = "text before [link display](123) text after";
			const string expectedValue =
@"<article class='mud-markdown-body'>
<p class='mud-typography mud-typography-body1 mud-inherit-text'>
text before <button blazor:onclick='1' type='button' class='mud-button-root mud-button mud-button-text mud-button-text-primary mud-button-text-size-medium mud-ripple' blazor:onclick:stopPropagation blazor:elementReference=''><span class='mud-button-label'>link display</span></button> text after
</p>
</article>";

			using var fixture = CreateFixture(value);
			fixture.MarkupMatches(expectedValue);
		}

		[Fact]
		public void RenderTable()
		{
			const string value =
@"|Column1|Column2|Column3|
|-|-|-|
|cell1-1|cell1-2|cell1-3|
|cell2-1|cell2-2|cell2-3|";

			const string expectedValue =
@"<article class='mud-markdown-body'>
  <div class='mud-table mud-simple-table mud-table-bordered mud-table-striped mud-elevation-1'>
    <div class='mud-table-container'>
      <table>
        <thead>
          <tr>
            <th>
              <p class='mud-typography mud-typography-body1 mud-inherit-text'>Column1</p>
            </th>
            <th>
              <p class='mud-typography mud-typography-body1 mud-inherit-text'>Column2</p>
            </th>
            <th>
              <p class='mud-typography mud-typography-body1 mud-inherit-text'>Column3</p>
            </th>
          </tr>
        </thead>
        <tbody>
          <tr>
            <td>
              <p class='mud-typography mud-typography-body1 mud-inherit-text'>cell1-1</p>
            </td>
            <td>
              <p class='mud-typography mud-typography-body1 mud-inherit-text'>cell1-2</p>
            </td>
            <td>
              <p class='mud-typography mud-typography-body1 mud-inherit-text'>cell1-3</p>
            </td>
          </tr>
          <tr>
            <td>
              <p class='mud-typography mud-typography-body1 mud-inherit-text'>cell2-1</p>
            </td>
            <td>
              <p class='mud-typography mud-typography-body1 mud-inherit-text'>cell2-2</p>
            </td>
            <td>
              <p class='mud-typography mud-typography-body1 mud-inherit-text'>cell2-3</p>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</article>";

			using var fixture = CreateFixture(value);
			fixture.MarkupMatches(expectedValue);
		}

		[Fact]
		public void RenderTableWithAdjacentText()
		{
			const string value =
@"text before

|col1|col2|
|-|-|
|cell1|cell2|

text after";

            const string expectedValue =
@"<article class='mud-markdown-body'>
  <p class='mud-typography mud-typography-body1 mud-inherit-text'>text before</p>
  <div class='mud-table mud-simple-table mud-table-bordered mud-table-striped mud-elevation-1'>
    <div class='mud-table-container'>
      <table>
        <thead>
          <tr>
            <th>
              <p class='mud-typography mud-typography-body1 mud-inherit-text'>col1</p>
            </th>
            <th>
              <p class='mud-typography mud-typography-body1 mud-inherit-text'>col2</p>
            </th>
          </tr>
        </thead>
        <tbody>
          <tr>
            <td>
              <p class='mud-typography mud-typography-body1 mud-inherit-text'>cell1</p>
            </td>
            <td>
              <p class='mud-typography mud-typography-body1 mud-inherit-text'>cell2</p>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
  <p class='mud-typography mud-typography-body1 mud-inherit-text'>text after</p>
</article>";

            using var fixture = CreateFixture(value);
			fixture.MarkupMatches(expectedValue);
		}
	}
}
