using Markdig;
using MudBlazor.Markdown.Tests.Services;

namespace MudBlazor.Markdown.Tests.MarkdownComponentTests;

public sealed class MarkdownComponentShould : MarkdownComponentTestsBase
{
	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData(" ")]
	public void RenderNothingIfNullOrWhitespace(string? value)
	{
		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(string.Empty);
	}

	[Fact]
	public void RenderEmphasisElements()
	{
		const string value = "Some text `code` again text - *italics* text and also **bold** and ~~strikethrough~~ text.";
		const string expected =
"""
<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>
		Some text <code>code</code> again text - <i>italics</i> text and also <b>bold</b> and <del>strikethrough</del> text.
	</p>
</article>
""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderBlockQuotes()
	{
		const string value = ">Some text `code` again text - *italics* text and also **bold** and ~~strikethrough~~ text.";
		const string expected =
"""
<article class='mud-markdown-body'>
	<blockquote>
		<p class='mud-typography mud-typography-body1'>
			Some text <code>code</code> again text - <i>italics</i> text and also <b>bold</b> and <del>strikethrough</del> text.
		</p>
	</blockquote>
</article>
""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	// Some values might need to be removed once they are implemented
	[Theory]
	[InlineData("^")] // superscript
	[InlineData("~")] // subscript
	[InlineData("++")] // inserted
	[InlineData("==")] // marked
	public void RenderInvalidEmphasis(string emphasisDelimiter)
	{
		string value = $"I expect that {emphasisDelimiter}emphasis{emphasisDelimiter} will be rendered as escaped. {emphasisDelimiter}Even with a trailing space {emphasisDelimiter} or with {emphasisDelimiter}~~nested markdown~~{emphasisDelimiter}.";
		string expected =
$"""
<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>
		I expect that {emphasisDelimiter}emphasis{emphasisDelimiter} will be rendered as escaped. {emphasisDelimiter}Even with a trailing space {emphasisDelimiter} or with {emphasisDelimiter}<del>nested markdown</del>{emphasisDelimiter}.
	</p>
</article>
""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Theory]
	[InlineData("\r\n")]
	[InlineData("\n")]
	public void ReplaceNewLineSymbols(string newLine)
	{
		var value = "line1" + newLine + "line2";
		const string expected =
"""
<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>
		line1<br />line2
	</p>
</article>
""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderExternalLink()
	{
		const string value = "[link display](https://www.google.co.jp/)";
		const string expected =
"""
<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>
		<a rel='noopener noreferrer' href='https://www.google.co.jp/' target='_blank' class='mud-typography mud-link mud-primary-text mud-link-underline-hover mud-typography-body1'>
			link display
		</a>
	</p>
</article>
""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderInternalLink()
	{
		const string value = "[link display](" + TestNavigationManager.TestUrl + ")";
		const string expected =
"""
<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>
		<a href='http://localhost:1234/' class='mud-typography mud-link mud-primary-text mud-link-underline-hover mud-typography-body1'>
			link display
		</a>
	</p>
</article>
""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderLinkAsLink()
	{
		const string value = "text before [link display](123) text after";
		const string expected =
"""
<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>
		text before 
		<a href='123' class='mud-typography mud-link mud-primary-text mud-link-underline-hover mud-typography-body1'>link display</a>
		text after
	</p>
</article>
""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderLinkAsButton()
	{
		const string value = "text before [link display](123) text after";
		const string expected =
"""
<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>
		text before
		<span class='mud-typography mud-link mud-primary-text mud-link-underline-hover mud-typography-body1'>link display</span>
		text after
	</p>
</article>
""";

		using var fixture = CreateFixture(value, new TestCommand());
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void PreventDefaultIfNavigatesToId()
	{
		const string value = "[link](#id)";
		const string expected =
"""
<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>
		<a href='#id' role='button' blazor:onclick:preventDefault blazor:onclick='1' class='mud-typography mud-link mud-primary-text mud-link-underline-hover mud-typography-body1'>
			link
		</a>
	</p>
</article>
""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void NotPreventDefaultIfNavigateToAnotherPage()
	{
		const string value = "[link](tokyo/#id)";
		const string expected =
"""
<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>
		<a blazor:onclick='2' href='tokyo/#id' class='mud-typography mud-link mud-primary-text mud-link-underline-hover mud-typography-body1'>
			link
		</a>
	</p>
</article>
""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderImage()
	{
		const string value = "![emw-banner](extra/emw.png)";
		const string expectedResult =
"""
<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>
		<img src='extra/emw.png' alt='emw-banner' class='mud-image object-fill object-center mud-elevation-25 rounded-lg'>
	</p>
</article>
""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expectedResult);
	}

	[Fact]
	public void RenderImageLink()
	{
		const string value = "[![emw-banner](extra/emw.png)](https://www.google.co.jp/)";
		const string expectedResult =
"""
<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>
		<a rel='noopener noreferrer' href='https://www.google.co.jp/' target='_blank' class='mud-typography mud-link mud-primary-text mud-link-underline-hover mud-typography-body1'>
			<img src='extra/emw.png' alt='emw-banner' class='mud-image object-fill object-center mud-elevation-25 rounded-lg'>
		</a>
	</p>
</article>
""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expectedResult);
	}

	[Fact]
	public void RenderUnorderedList()
	{
		const string value =
"""
some text before
- `item1` - text **bold**
- `item2` - text *italic*
""";

		const string expected =
"""
<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>some text before</p>
	<ul>
		<li><p class='mud-typography mud-typography-body1'><code>item1</code>- text <b>bold</b></p></li>
		<li><p class='mud-typography mud-typography-body1'><code>item2</code> - text <i>italic</i></p></li>
	</ul>
</article>
""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderNestedUnorderedList2()
	{
		const string value =
"""
some text before
- `item1` - text *italic*
  - `item1-1` - text
  - `item1-2` - text
- `item2` - text **bold**
""";

		const string expected =
"""
<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>some text before</p>
	<ul>
		<li><p class='mud-typography mud-typography-body1'><code>item1</code> - text <i>italic</i></p>
			<ul>
				<li><p class='mud-typography mud-typography-body1'><code>item1-1</code> - text</p></li>
				<li><p class='mud-typography mud-typography-body1'><code>item1-2</code> - text</p></li>
			</ul>
		</li>
		<li><p class='mud-typography mud-typography-body1'><code>item2</code> - text <b>bold</b></p></li>
	</ul>
</article>
""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderNestedUnorderedList3()
	{
		const string value =
"""
some text before
- `item1` - text *italic*
  - `item1-1` - text
  - `item1-2` - text
    - `item1-2-1` - text
- `item2` - text **bold**
""";

		const string expected =
"""
<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>some text before</p>
	<ul>
		<li>
			<p class='mud-typography mud-typography-body1'><code>item1</code> - text <i>italic</i></p>
			<ul>
				<li><p class='mud-typography mud-typography-body1'><code>item1-1</code> - text</p></li>
				<li><p class='mud-typography mud-typography-body1'><code>item1-2</code> - text</p>
					<ul>
						<li><p class='mud-typography mud-typography-body1'><code>item1-2-1</code> - text</p></li>
					</ul>
				</li>
			</ul>
		</li>
		<li><p class='mud-typography mud-typography-body1'><code>item2</code> - text <b>bold</b></p></li>
	</ul>
</article>
""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderOrderedList()
	{
		const string value =
"""
1. Do thing 1
2. Do next
3. Go to Sapporo
""";

		const string expected =
"""
<article class='mud-markdown-body'>
	<ol>
		<li>
			<p class='mud-typography mud-typography-body1'>Do thing 1</p>
		</li>
		<li>
			<p class='mud-typography mud-typography-body1'>Do next</p>
		</li>
		<li>
			<p class='mud-typography mud-typography-body1'>Go to Sapporo</p>
		</li>
	</ol>
</article>
""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderOrderedListWithCodeBlock()
	{
		const string value =
"""
1. Connect to your MySQL server using a MySQL client, such as the `mysql` command-line tool:
  ```bash
  mysql -u username -p
  ```
""";

		const string expected =
"""
<article class='mud-markdown-body'>
	<ol>
		<li><p class='mud-typography mud-typography-body1'>Connect to your MySQL server using a MySQL client, such as the <code>mysql</code> command-line tool:</p></li>
	</ol>
	<div class='snippet-clipboard-content overflow-auto'>
		<button blazor:onclick='1' type='button' class='mud-button-root mud-icon-button mud-button mud-button-filled mud-button-filled-primary mud-button-filled-size-medium mud-ripple snippet-clipboard-copy-icon ma-2' blazor:onclick:stopPropagation blazor:elementReference='eb8fdbba-2335-4a91-b2ed-492ac862178c'>
			<span class='mud-icon-button-label'>
				<svg class='mud-icon-root mud-svg-icon mud-icon-size-medium' focusable='false' viewBox='0 0 24 24' aria-hidden='true' role='img'>
					<g><rect fill='none' height='24' width='24'/></g>
					<g><path d='M15,20H5V7c0-0.55-0.45-1-1-1h0C3.45,6,3,6.45,3,7v13c0,1.1,0.9,2,2,2h10c0.55,0,1-0.45,1-1v0C16,20.45,15.55,20,15,20z M20,16V4c0-1.1-0.9-2-2-2H9C7.9,2,7,2.9,7,4v12c0,1.1,0.9,2,2,2h9C19.1,18,20,17.1,20,16z M18,16H9V4h9V16z'/></g>
				</svg>
			</span>
		</button>
		<pre><code class='hljs language-bash' blazor:elementReference='af1911cc-f5ec-4946-ac11-a83b413aa0da'></code></pre>
	</div>
</article>
""";
		
		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderTable()
	{
		const string value =
"""
|Column1|Column2|Column3|
|-|-|-|
|cell1-1|cell1-2|cell1-3|
|cell2-1|cell2-2|cell2-3|
""";

		const string expected =
"""
<article class='mud-markdown-body'>
	<div class='mud-table mud-simple-table mud-table-bordered mud-table-striped mud-elevation-1' style='overflow-x: auto;'>
		<div class='mud-table-container'>
			<table>
				<thead>
					<tr>
						<th><p class='mud-typography mud-typography-body1'>Column1</p></th>
						<th><p class='mud-typography mud-typography-body1'>Column2</p></th>
						<th><p class='mud-typography mud-typography-body1'>Column3</p></th>
					</tr>
				</thead>
				<tbody>
					<tr>
						<td><p class='mud-typography mud-typography-body1'>cell1-1</p></td>
						<td><p class='mud-typography mud-typography-body1'>cell1-2</p></td>
						<td><p class='mud-typography mud-typography-body1'>cell1-3</p></td>
					</tr>
					<tr>
						<td><p class='mud-typography mud-typography-body1'>cell2-1</p></td>
						<td><p class='mud-typography mud-typography-body1'>cell2-2</p></td>
						<td><p class='mud-typography mud-typography-body1'>cell2-3</p></td>
					</tr>
				</tbody>
			</table>
		</div>
	</div>
</article>
""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
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
$"""
 |1|2|
 |-|-|
 |a{newLineChar}b|c
 """;

		var expected =
$"""
 <article class='mud-markdown-body'>
    <div class='mud-table mud-simple-table mud-table-bordered mud-table-striped mud-elevation-1' style='overflow-x: auto;'>
       <div class='mud-table-container'>
          <table>
             <thead>
                <tr>
                   <th>
                      <p class='mud-typography mud-typography-body1'>1</p>
                   </th>
                   <th>
                      <p class='mud-typography mud-typography-body1'>2</p>
                   </th>
                </tr>
             </thead>
             <tbody>
                <tr>
                   <td>
                      <p class='mud-typography mud-typography-body1'>a{newLineChar}b</p>
                   </td>
                   <td>
                      <p class='mud-typography mud-typography-body1'>c</p>
                   </td>
                </tr>
             </tbody>
          </table>
       </div>
    </div>
 </article>
 """;

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderTableMinWidth()
	{
		const string value =
"""
|col1|col2|
|-|-|
|cell1|cell2|
""";

		const string expected =
"""
<article class='mud-markdown-body'>
   <div class='mud-table mud-simple-table mud-table-bordered mud-table-striped mud-elevation-1' style='overflow-x: auto;'>
      <div class='mud-table-container'>
         <table>
            <thead>
               <tr>
                  <th style='min-width:200px'>
                     <p class='mud-typography mud-typography-body1'>col1</p>
                  </th>
                  <th style='min-width:200px'>
                     <p class='mud-typography mud-typography-body1'>col2</p>
                  </th>
               </tr>
            </thead>
            <tbody>
               <tr>
                  <td>
                     <p class='mud-typography mud-typography-body1'>cell1</p>
                  </td>
                  <td>
                     <p class='mud-typography mud-typography-body1'>cell2</p>
                  </td>
               </tr>
            </tbody>
         </table>
      </div>
   </div>
</article>
""";

		using var fixture = CreateFixture(value, tableCellMinWidth: 200);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderLineSeparator()
	{
		const string value =
"""
first line
***
second line
""";

		const string expected =
"""
<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>first line</p>
	<hr class='mud-divider mud-divider-fullwidth'/>
	<p class='mud-typography mud-typography-body1'>second line</p>
</article>
""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderCodeBlock()
	{
		const string value =
"""
```cs
public bool IsMudBlazorCool()
{
	return true;
}
```
""";

		const string expected =
"""
<article class='mud-markdown-body'>
	<div class='snippet-clipboard-content overflow-auto'>
		<button blazor:onclick='1' type='button' class='mud-button-root mud-icon-button mud-button mud-button-filled mud-button-filled-primary mud-button-filled-size-medium mud-ripple snippet-clipboard-copy-icon ma-2' blazor:onclick:stopPropagation blazor:elementReference='af22cf66-3ea5-4899-bd97-91b4fdc35b82'>
			<span class='mud-icon-button-label'>
				<svg class='mud-icon-root mud-svg-icon mud-icon-size-medium' focusable='false' viewBox='0 0 24 24' aria-hidden='true' role='img'>
					<g><rect fill='none' height='24' width='24'/></g>
					<g><path d='M15,20H5V7c0-0.55-0.45-1-1-1h0C3.45,6,3,6.45,3,7v13c0,1.1,0.9,2,2,2h10c0.55,0,1-0.45,1-1v0C16,20.45,15.55,20,15,20z M20,16V4c0-1.1-0.9-2-2-2H9C7.9,2,7,2.9,7,4v12c0,1.1,0.9,2,2,2h9C19.1,18,20,17.1,20,16z M18,16H9V4h9V16z'/></g>
				</svg>
			</span>
		</button>
		<pre><code class='hljs language-cs' blazor:elementReference='3b498767-f59e-4a18-a27d-a828bf3dd0e5'></code></pre>
	</div>
</article>
""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void HaveDefaultMarkdownPipeline()
	{
		const string value = "**bold**";

		using var fixture = CreateFixture(value, markdownPipeline: null);

		GetMarkdownPipeline(fixture.Instance)
			.Should()
			.NotBeNull();
	}

	[Fact]
	public void PassCustomMarkdownPipeline()
	{
		const string value = "**bold**";

		var input = new MarkdownPipelineBuilder()
			.UseAdvancedExtensions()
			.UseAbbreviations()
			.UseMathematics()
			.Build();

		using var fixture = CreateFixture(value, markdownPipeline: input);

		GetMarkdownPipeline(fixture.Instance)
			.Should()
			.BeNull();
	}
}
