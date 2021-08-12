using Bunit;
using MudBlazor.Markdown.Tests.Services;
using Xunit;

namespace MudBlazor.Markdown.Tests.MarkdownComponentTests
{
	public sealed class MarkdownComponentShould : MarkdownComponentTestsBase
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
			const string expectedValue =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1 mud-inherit-text'>
		Some text <code>code</code> again text - <i>italics</i> text and <b>bold</b> text.
	</p>
</article>";

			using var fixture = CreateFixture(value);
			fixture.MarkupMatches(expectedValue);
		}

		[Fact]
		public void RenderBlockQuotes()
		{
			const string value = ">Some text `code` again text - *italics* text and **bold** text.";
			const string expectedValue =
@"<article class='mud-markdown-body'>
	<blockquote>
		<p class='mud-typography mud-typography-body1 mud-inherit-text'>
			Some text <code>code</code> again text - <i>italics</i> text and <b>bold</b> text.
		</p>
	</blockquote>
</article>";

			using var fixture = CreateFixture(value);
			fixture.MarkupMatches(expectedValue);
		}

		[Theory]
		[InlineData("\r\n")]
		[InlineData("\n")]
		public void ReplaceNewLineSymbols(string newLine)
		{
			var value = "line1" + newLine + "line2";
			const string expectedValue =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1 mud-inherit-text'>
		line1<br />line2
	</p>
</article>";

			using var fixture = CreateFixture(value);
			fixture.MarkupMatches(expectedValue);
		}

		[Fact]
		public void RenderExternalLink()
		{
			const string value = "[link display](https://www.google.co.jp/)";
			const string expectedValue =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1 mud-inherit-text'>
		<a rel='noopener noreferrer' href='https://www.google.co.jp/' target='_blank' class='mud-typography mud-link mud-primary-text mud-link-underline-hover mud-typography-body1'>
			link display
		</a>
	</p>
</article>";

			using var fixture = CreateFixture(value);
			fixture.MarkupMatches(expectedValue);
		}

		[Fact]
		public void RenderInternalLink()
		{
			const string value = "[link display](" + TestNavigationManager.TestUrl + ")";
			const string expectedValue =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1 mud-inherit-text'>
		<a href='http://localhost:1234/' class='mud-typography mud-link mud-primary-text mud-link-underline-hover mud-typography-body1'>
			link display
		</a>
	</p>
</article>";

			using var fixture = CreateFixture(value);
			fixture.MarkupMatches(expectedValue);
		}

		[Fact]
		public void RenderLinkAsLink()
		{
			const string value = "text before [link display](123) text after";
			const string expectedValue =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1 mud-inherit-text'>
		text before 
		<a href='123' class='mud-typography mud-link mud-primary-text mud-link-underline-hover mud-typography-body1'>link display</a>
		text after
	</p>
</article>";

			using var fixture = CreateFixture(value);
			fixture.MarkupMatches(expectedValue);
		}

		[Fact]
		public void RenderLinkAsButton()
		{
			const string value = "text before [link display](123) text after";
			const string expectedValue =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1 mud-inherit-text'>
		text before
		<span class='mud-typography mud-link mud-primary-text mud-link-underline-hover mud-typography-body1'>link display</span>
		text after
	</p>
</article>";

			using var fixture = CreateFixture(value, new TestCommand());
			fixture.MarkupMatches(expectedValue);
		}

		[Fact]
		public void PreventDefaultIfNavigatesToId()
		{
			const string value = "[link](#id)";
			const string expectedValue =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1 mud-inherit-text'>
		<a blazor:onclick:preventDefault blazor:onclick='2' href='#id' class='mud-typography mud-link mud-primary-text mud-link-underline-hover mud-typography-body1'>
			link
		</a>
	</p>
</article>";

			using var fixture = CreateFixture(value);
			fixture.MarkupMatches(expectedValue);
		}

		[Fact]
		public void NotPreventDefaultIfNavigateToAnotherPage()
		{
			const string value = "[link](tokyo/#id)";
			const string expectedValue =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1 mud-inherit-text'>
		<a blazor:onclick='2' href='tokyo/#id' class='mud-typography mud-link mud-primary-text mud-link-underline-hover mud-typography-body1'>
			link
		</a>
	</p>
</article>";

			using var fixture = CreateFixture(value);
			fixture.MarkupMatches(expectedValue);
		}

		[Fact]
		public void RenderImage()
		{
			const string value = "![emw-banner](extra/emw.png)";
			const string expectedResult =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1 mud-inherit-text'>
		<img src='extra/emw.png' alt='emw-banner' />
	</p>
</article>";

			using var fixture = CreateFixture(value);
			fixture.MarkupMatches(expectedResult);
		}

		[Fact]
		public void RenderImageLink()
		{
			const string value = "[![emw-banner](extra/emw.png)](https://www.google.co.jp/)";
			const string expectedResult =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1 mud-inherit-text'>
		<a rel='noopener noreferrer' href='https://www.google.co.jp/' target='_blank' class='mud-typography mud-link mud-primary-text mud-link-underline-hover mud-typography-body1'>
			<img src='extra/emw.png' alt='emw-banner' />
		</a>
	</p>
</article>";

			using var fixture = CreateFixture(value);
			fixture.MarkupMatches(expectedResult);
		}

		[Fact]
		public void RenderUnorderedList()
		{
			const string value =
@"some text before
- `item1` - text **bold**
- `item2` - text *italic*";

			const string expectedValue =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1 mud-inherit-text'>some text before</p>
	<ul>
		<li><p class='mud-typography mud-typography-body1 mud-inherit-text'><code>item1</code>- text <b>bold</b></p></li>
		<li><p class='mud-typography mud-typography-body1 mud-inherit-text'><code>item2</code> - text <i>italic</i></p></li>
	</ul>
</article>";

			using var fixture = CreateFixture(value);
			fixture.MarkupMatches(expectedValue);
		}

		[Fact]
		public void RenderNestedUnorderedList2()
		{
			const string value =
@"some text before
- `item1` - text *italic*
  - `item1-1` - text
  - `item1-2` - text
- `item2` - text **bold**";

			const string expectedValue =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1 mud-inherit-text'>some text before</p>
	<ul>
		<li><p class='mud-typography mud-typography-body1 mud-inherit-text'><code>item1</code> - text <i>italic</i></p></li>
		<ul>
			<li><p class='mud-typography mud-typography-body1 mud-inherit-text'><code>item1-1</code> - text</p></li>
			<li><p class='mud-typography mud-typography-body1 mud-inherit-text'><code>item1-2</code> - text</p></li>
		</ul>
		<li><p class='mud-typography mud-typography-body1 mud-inherit-text'><code>item2</code> - text <b>bold</b></p></li>
	</ul>
</article>";

			using var fixture = CreateFixture(value);
			fixture.MarkupMatches(expectedValue);
		}

		[Fact]
		public void RenderNestedUnorderedList3()
		{
			const string value =
@"some text before
- `item1` - text *italic*
  - `item1-1` - text
  - `item1-2` - text
    - `item1-2-1` - text
- `item2` - text **bold**";

			const string expectedValue =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1 mud-inherit-text'>some text before</p>
	<ul>
		<li><p class='mud-typography mud-typography-body1 mud-inherit-text'><code>item1</code> - text <i>italic</i></p></li>
		<ul>
			<li><p class='mud-typography mud-typography-body1 mud-inherit-text'><code>item1-1</code> - text</p></li>
			<li><p class='mud-typography mud-typography-body1 mud-inherit-text'><code>item1-2</code> - text</p></li>
			<ul>
				<li><p class='mud-typography mud-typography-body1 mud-inherit-text'><code>item1-2-1</code> - text</p></li>
			</ul>
		</ul>
		<li><p class='mud-typography mud-typography-body1 mud-inherit-text'><code>item2</code> - text <b>bold</b></p></li>
	</ul>
</article>";

			using var fixture = CreateFixture(value);
			fixture.MarkupMatches(expectedValue);
		}

		[Fact]
		public void RenderOrderedList()
		{
			const string value =
@"1. Do thing 1
2. Do next
3. Go to Sapporo";

			const string expectedValue =
@"<article class='mud-markdown-body'>
	<ol>
		<li>
			<p class='mud-typography mud-typography-body1 mud-inherit-text'>Do thing 1</p>
		</li>
		<li>
			<p class='mud-typography mud-typography-body1 mud-inherit-text'>Do next</p>
		</li>
		<li>
			<p class='mud-typography mud-typography-body1 mud-inherit-text'>Go to Sapporo</p>
		</li>
	</ol>
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
	<div class='mud-table mud-simple-table mud-table-bordered mud-table-striped mud-elevation-1' style='overflow-x: auto;'>
		<div class='mud-table-container'>
			<table>
				<thead>
					<tr>
						<th><p class='mud-typography mud-typography-body1 mud-inherit-text'>Column1</p></th>
						<th><p class='mud-typography mud-typography-body1 mud-inherit-text'>Column2</p></th>
						<th><p class='mud-typography mud-typography-body1 mud-inherit-text'>Column3</p></th>
					</tr>
				</thead>
				<tbody>
					<tr>
						<td><p class='mud-typography mud-typography-body1 mud-inherit-text'>cell1-1</p></td>
						<td><p class='mud-typography mud-typography-body1 mud-inherit-text'>cell1-2</p></td>
						<td><p class='mud-typography mud-typography-body1 mud-inherit-text'>cell1-3</p></td>
					</tr>
					<tr>
						<td><p class='mud-typography mud-typography-body1 mud-inherit-text'>cell2-1</p></td>
						<td><p class='mud-typography mud-typography-body1 mud-inherit-text'>cell2-2</p></td>
						<td><p class='mud-typography mud-typography-body1 mud-inherit-text'>cell2-3</p></td>
					</tr>
				</tbody>
			</table>
		</div>
	</div>
</article>";

			using var fixture = CreateFixture(value);
			fixture.MarkupMatches(expectedValue);
		}

		[Theory]
		[InlineData("<br>")]
		[InlineData("<br/>")]
		[InlineData("<br />")]
		[InlineData("<BR>")]
		[InlineData("<BR/>")]
		[InlineData("<BR />")]
		public void RenderTableWithNewLines(string newLineChar)
		{
			var value =
$@"|1|2|
|-|-|
|a{newLineChar}b|c";

			var expectedValue =
$@"<article class='mud-markdown-body'>
   <div class='mud-table mud-simple-table mud-table-bordered mud-table-striped mud-elevation-1' style='overflow-x: auto;'>
      <div class='mud-table-container'>
         <table>
            <thead>
               <tr>
                  <th>
                     <p class='mud-typography mud-typography-body1 mud-inherit-text'>1</p>
                  </th>
                  <th>
                     <p class='mud-typography mud-typography-body1 mud-inherit-text'>2</p>
                  </th>
               </tr>
            </thead>
            <tbody>
               <tr>
                  <td>
                     <p class='mud-typography mud-typography-body1 mud-inherit-text'>a{newLineChar}b</p>
                  </td>
                  <td>
                     <p class='mud-typography mud-typography-body1 mud-inherit-text'>c</p>
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
		public void RenderTableMinWidth()
		{
			const string value =
@"|col1|col2|
|-|-|
|cell1|cell2|";

			const string expectedValue =
@"<article class='mud-markdown-body'>
   <div class='mud-table mud-simple-table mud-table-bordered mud-table-striped mud-elevation-1' style='overflow-x: auto;'>
      <div class='mud-table-container'>
         <table>
            <thead>
               <tr>
                  <th style='min-width:200px'>
                     <p class='mud-typography mud-typography-body1 mud-inherit-text'>col1</p>
                  </th>
                  <th style='min-width:200px'>
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
</article>";

			using var fixture = CreateFixture(value, tableCellMinWidth: 200);
			fixture.MarkupMatches(expectedValue);
		}

		[Theory]
		[InlineData("#", "h1")]
		[InlineData("##", "h2")]
		[InlineData("###", "h3")]
		[InlineData("####", "h4")]
		[InlineData("#####", "h5")]
		[InlineData("######", "h6")]
		public void RenderHeaders(string valueInput, string expected)
		{
			var value = valueInput + " some text";

			var expectedValue =
$@"<article class='mud-markdown-body'>
	<{expected} id='some-text' class='mud-typography mud-typography-{expected} mud-inherit-text'>
		some text
	</{expected}>
</article>";

			using var fixture = CreateFixture(value);
			fixture.MarkupMatches(expectedValue);
		}

		[Theory]
		[InlineData(Typo.h1, "h1")]
		[InlineData(Typo.h2, "h2")]
		[InlineData(Typo.h3, "h3")]
		[InlineData(Typo.h4, "h4")]
		[InlineData(Typo.h5, "h5")]
		[InlineData(Typo.h6, "h6")]
		public void RenderHeadersWithDifferentTypoH1(Typo newTypo, string expected)
		{
			const string value = "# some text";

			var expectedValue =
$@"<article class='mud-markdown-body'>
	<{expected} id='some-text' class='mud-typography mud-typography-{expected} mud-inherit-text'>
		some text
	</{expected}>
</article>";

			using var fixture = CreateFixture(value, h1Typo: newTypo);
			fixture.MarkupMatches(expectedValue);
		}

		[Theory]
		[InlineData(Typo.h1, "h1")]
		[InlineData(Typo.h2, "h2")]
		[InlineData(Typo.h3, "h3")]
		[InlineData(Typo.h4, "h4")]
		[InlineData(Typo.h5, "h5")]
		[InlineData(Typo.h6, "h6")]
		public void RenderHeadersWithDifferentTypoH2(Typo newTypo, string expected)
		{
			const string value = "## some text";

			var expectedValue =
$@"<article class='mud-markdown-body'>
	<{expected} id='some-text' class='mud-typography mud-typography-{expected} mud-inherit-text'>
		some text
	</{expected}>
</article>";

			using var fixture = CreateFixture(value, h2Typo: newTypo);
			fixture.MarkupMatches(expectedValue);
		}

		[Theory]
		[InlineData(Typo.h1, "h1")]
		[InlineData(Typo.h2, "h2")]
		[InlineData(Typo.h3, "h3")]
		[InlineData(Typo.h4, "h4")]
		[InlineData(Typo.h5, "h5")]
		[InlineData(Typo.h6, "h6")]
		public void RenderHeadersWithDifferentTypoH3(Typo newTypo, string expected)
		{
			const string value = "### some text";

			var expectedValue =
$@"<article class='mud-markdown-body'>
	<{expected} id='some-text' class='mud-typography mud-typography-{expected} mud-inherit-text'>
		some text
	</{expected}>
</article>";

			using var fixture = CreateFixture(value, h3Typo: newTypo);
			fixture.MarkupMatches(expectedValue);
		}

		[Theory]
		[InlineData(Typo.h1, "h1")]
		[InlineData(Typo.h2, "h2")]
		[InlineData(Typo.h3, "h3")]
		[InlineData(Typo.h4, "h4")]
		[InlineData(Typo.h5, "h5")]
		[InlineData(Typo.h6, "h6")]
		public void RenderHeadersWithDifferentTypoH4(Typo newTypo, string expected)
		{
			const string value = "#### some text";

			var expectedValue =
$@"<article class='mud-markdown-body'>
	<{expected} id='some-text' class='mud-typography mud-typography-{expected} mud-inherit-text'>
		some text
	</{expected}>
</article>";

			using var fixture = CreateFixture(value, h4Typo: newTypo);
			fixture.MarkupMatches(expectedValue);
		}

		[Theory]
		[InlineData(Typo.h1, "h1")]
		[InlineData(Typo.h2, "h2")]
		[InlineData(Typo.h3, "h3")]
		[InlineData(Typo.h4, "h4")]
		[InlineData(Typo.h5, "h5")]
		[InlineData(Typo.h6, "h6")]
		public void RenderHeadersWithDifferentTypoH5(Typo newTypo, string expected)
		{
			const string value = "##### some text";

			var expectedValue =
$@"<article class='mud-markdown-body'>
	<{expected} id='some-text' class='mud-typography mud-typography-{expected} mud-inherit-text'>
		some text
	</{expected}>
</article>";

			using var fixture = CreateFixture(value, h5Typo: newTypo);
			fixture.MarkupMatches(expectedValue);
		}

		[Theory]
		[InlineData(Typo.h1, "h1")]
		[InlineData(Typo.h2, "h2")]
		[InlineData(Typo.h3, "h3")]
		[InlineData(Typo.h4, "h4")]
		[InlineData(Typo.h5, "h5")]
		[InlineData(Typo.h6, "h6")]
		public void RenderHeadersWithDifferentTypoH6(Typo newTypo, string expected)
		{
			const string value = "###### some text";

			var expectedValue =
$@"<article class='mud-markdown-body'>
	<{expected} id='some-text' class='mud-typography mud-typography-{expected} mud-inherit-text'>
		some text
	</{expected}>
</article>";

			using var fixture = CreateFixture(value, h6Typo: newTypo);
			fixture.MarkupMatches(expectedValue);
		}

		[Fact]
		public void RenderLineSeparator()
		{
			const string value =
@"first line
***
second line";

			const string expectedValue =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1 mud-inherit-text'>first line</p>
	<hr class='mud-divider'/>
	<p class='mud-typography mud-typography-body1 mud-inherit-text'>second line</p>
</article>";

			using var fixture = CreateFixture(value);
			fixture.MarkupMatches(expectedValue);
		}
	}
}
