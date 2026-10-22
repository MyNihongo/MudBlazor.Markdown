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
				<p class='mud-typography mud-typography-body1'><span class='mud-math' role='math'><span class='mud-math-sqrt'><span class='mud-math-surd'>√</span><span class='mud-math-radicand'><span class='mud-math-mn'>2</span></span></span></span></p>
			</article>
			""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Theory]
	[InlineData("ax^2", "<span class='mud-math-mi'>a</span><span class='mud-math-scripts'><span class='mud-math-mi'>x</span><span class='mud-math-sup'><span class='mud-math-mn'>2</span></span></span>")]
	[InlineData("x^2_i", "<span class='mud-math-scripts'><span class='mud-math-mi'>x</span><span class='mud-math-subsup'><span class='mud-math-sup'><span class='mud-math-mn'>2</span></span><span class='mud-math-sub'><span class='mud-math-mi'>i</span></span></span></span>")]
	[InlineData("f''", "<span class='mud-math-scripts'><span class='mud-math-mi'>f</span><span class='mud-math-sup'>′′</span></span>")]
	[InlineData("x = \\frac12", "<span class='mud-math-mi'>x</span><span class='mud-math-op'>=</span><span class='mud-math-frac'><span class='mud-math-num'><span class='mud-math-mn'>1</span></span><span class='mud-math-den'><span class='mud-math-mn'>2</span></span></span>")]
	[InlineData("\\sqrt[3]{x}", "<span class='mud-math-sqrt'><span class='mud-math-index'><span class='mud-math-mn'>3</span></span><span class='mud-math-surd'>√</span><span class='mud-math-radicand'><span class='mud-math-mi'>x</span></span></span>")]
	[InlineData(@"\left( a \right)^2", "<span class='mud-math-scripts'><span class='mud-math-fenced'><span class='mud-math-fence'>(</span><span class='mud-math-row'><span class='mud-math-mi'>a</span></span><span class='mud-math-fence'>)</span></span><span class='mud-math-sup'><span class='mud-math-mn'>2</span></span></span>")]
	[InlineData("\\sum_i x", "<span class='mud-math-scripts'><span class='mud-math-largeop'>∑</span><span class='mud-math-sub'><span class='mud-math-mi'>i</span></span></span><span class='mud-math-mi'>x</span>")]
	[InlineData("\\sin x", "<span class='mud-math-fn'>sin</span><span class='mud-math-mi'>x</span>")]
	[InlineData("\\mathbb{R}", "<span class='mud-math-mn'>ℝ</span>")]
	[InlineData("\\text{if } a < b", "<span class='mud-math-text'>if </span><span class='mud-math-mi'>a</span><span class='mud-math-op'>&lt;</span><span class='mud-math-mi'>b</span>")]
	[InlineData("\\unknown", "<span class='mud-math-error'>\\unknown</span>")]
	public void RenderElements(string tex, string html)
	{
		var value = "$" + tex + "$";

		var expected =
			$"""
			<article id:ignore class='mud-markdown-body'>
				<p class='mud-typography mud-typography-body1'><span class='mud-math' role='math'>{html}</span></p>
			</article>
			""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderLimitsInBlock()
	{
		const string value = "$$\\sum_{i=1}^n i$$";

		const string expected =
			"""
			<article id:ignore class='mud-markdown-body'>
				<p class='mud-typography mud-typography-body1'><span class='mud-math mud-math-block' role='math'><span class='mud-math-limits'><span class='mud-math-over'><span class='mud-math-mi'>n</span></span><span class='mud-math-largeop'>∑</span><span class='mud-math-under'><span class='mud-math-mi'>i</span><span class='mud-math-op'>=</span><span class='mud-math-mn'>1</span></span></span><span class='mud-math-mi'>i</span></span></p>
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
				<p class='mud-typography mud-typography-body1'><span class='mud-math mud-math-block' role='math'><span class='mud-math-sqrt'><span class='mud-math-surd'>√</span><span class='mud-math-radicand'><span class='mud-math-mn'>2</span></span></span></span></p>
			</article>
			""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}
}
