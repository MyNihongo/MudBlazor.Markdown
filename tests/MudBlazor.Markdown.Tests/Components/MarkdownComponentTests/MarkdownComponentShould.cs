using Markdig;
using MudBlazor.Markdown.Tests.Services;

namespace MudBlazor.Markdown.Tests.Components.MarkdownComponentTests;

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
			<article id:ignore class='mud-markdown-body'>
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
			<article id:ignore class='mud-markdown-body'>
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

	[Theory]
	[InlineData("^")] // superscript
	[InlineData("~")] // subscript
	[InlineData("++")] // inserted
	[InlineData("==")] // marked
	public void RenderInvalidEmphasis(string emphasisDelimiter)
	{
		var value = $"I expect that {emphasisDelimiter}emphasis{emphasisDelimiter} will be rendered as escaped. {emphasisDelimiter}**Nested markdown is also escaped**{emphasisDelimiter}.";
		var expected =
			$"""
			 <article id:ignore class='mud-markdown-body'>
			 	<p class="mud-typography mud-typography-body1">
			 		I expect that <span class="mud-markdown-error">{emphasisDelimiter}emphasis{emphasisDelimiter}</span> will be rendered as escaped. <span class="mud-markdown-error">{emphasisDelimiter}**Nested markdown is also escaped**{emphasisDelimiter}</span>.
			 	</p>
			 </article>
			 """;

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Theory]
	[InlineData("\r\n")]
	[InlineData("\n")]
	public void RenderBreakSoft(string newLine)
	{
		var value = "line1" + newLine + "line2";
		const string expected =
			"""
			<article id:ignore class='mud-markdown-body'>
				<p class='mud-typography mud-typography-body1'>
					line1 line2
				</p>
			</article>
			""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Theory]
	[InlineData("\r\n")]
	[InlineData("\n")]
	public void RenderBreakHard(string newLine)
	{
		var value = "line1" + newLine + newLine + "line2";
		const string expected =
			"""
			<article id:ignore class='mud-markdown-body'>
				<p class='mud-typography mud-typography-body1'>line1</p>
				<p class='mud-typography mud-typography-body1'>line2</p>
			</article>
			""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderBreakHardWithSpaces()
	{
		const string value =
			"""
			line1  
			line2
			""";

		const string expected =
			"""
			<article id:ignore class='mud-markdown-body'>
				<p class='mud-typography mud-typography-body1'>
					line1<br />line2
				</p>
			</article>
			""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Theory]
	[InlineData("\r\n")]
	[InlineData("\n")]
	public void RenderBreakHardWithPipeline(string newLine)
	{
		var value = "line1" + newLine + "line2";
		const string expected =
			"""
			<article id:ignore class='mud-markdown-body'>
				<p class='mud-typography mud-typography-body1'>
					line1<br />line2
				</p>
			</article>
			""";

		var markdownPipeline = new MarkdownPipelineBuilder()
			.UseSoftlineBreakAsHardlineBreak()
			.Build();

		using var fixture = CreateFixture(value, markdownPipeline: markdownPipeline);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderExternalLink()
	{
		const string value = "[link display](https://www.google.co.jp/)";
		const string expected =
			"""
			<article id:ignore class='mud-markdown-body'>
				<p class='mud-typography mud-typography-body1'>
					<a rel='noopener noreferrer' href='https://www.google.co.jp/' target='_blank' class='mud-typography mud-link mud-primary-text mud-link-underline-hover'>
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
			<article id:ignore class='mud-markdown-body'>
				<p class='mud-typography mud-typography-body1'>
					<a href='http://localhost:1234/' class='mud-typography mud-link mud-primary-text mud-link-underline-hover'>
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
			<article id:ignore class='mud-markdown-body'>
				<p class='mud-typography mud-typography-body1'>
					text before 
					<a href='123' class='mud-typography mud-link mud-primary-text mud-link-underline-hover'>link display</a>
					text after
				</p>
			</article>
			""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void PreventDefaultIfNavigatesToId()
	{
		const string value = "[link](#id)";
		const string expected =
			"""
			<article id:ignore class='mud-markdown-body'>
				<p class='mud-typography mud-typography-body1'>
					<a href='#id' role='button' blazor:onclick:preventDefault blazor:onclick='1' class='mud-typography mud-link mud-primary-text mud-link-underline-hover'>
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
			<article id:ignore class='mud-markdown-body'>
				<p class='mud-typography mud-typography-body1'>
					<a blazor:onclick='2' href='tokyo/#id' class='mud-typography mud-link mud-primary-text mud-link-underline-hover'>
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
			<article id:ignore class='mud-markdown-body'>
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
			<article id:ignore class='mud-markdown-body'>
				<p class='mud-typography mud-typography-body1'>
					<a rel='noopener noreferrer' href='https://www.google.co.jp/' target='_blank' class='mud-typography mud-link mud-primary-text mud-link-underline-hover'>
						<img src='extra/emw.png' alt='emw-banner' class='mud-image object-fill object-center mud-elevation-25 rounded-lg'>
					</a>
				</p>
			</article>
			""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expectedResult);
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
			<article id:ignore class='mud-markdown-body'>
				<p class='mud-typography mud-typography-body1'>first line</p>
				<hr class='mud-divider mud-divider-fullwidth'/>
				<p class='mud-typography mud-typography-body1'>second line</p>
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

	[Fact]
	public void OverrideBodyTypo()
	{
		const string value = "text *it**a**lic* and **b*o*ld** and ~~strikethrough~~ and `code`";

		const string expected =
			"""
			<article id:ignore class="mud-markdown-body">
				<h3 class="mud-typography mud-typography-h3">
					text <i>it<b>a</b>lic</i> and <b>b<i>o</i>ld</b> and <del>strikethrough</del> and <code>code</code>
				</h3>
			</article>
			""";

		var props = new MudMarkdownProps
		{
			Body =
			{
				OverrideTypo = Typo.h3,
			},
		};

		using var fixture = CreateFixture(value, props: props);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void OverrideBodyTypoForQuote()
	{
		const string value = "> some quite text";

		const string expected =
			"""
			<article id:ignore class="mud-markdown-body">
				<blockquote>
					<h3 class="mud-typography mud-typography-h3">some quite text</h3>
				</blockquote>
			</article>
			""";

		var props = new MudMarkdownProps
		{
			Body =
			{
				OverrideTypo = Typo.h3,
			},
		};

		using var fixture = CreateFixture(value, props: props);
		fixture.MarkupMatches(expected);
	}
}
