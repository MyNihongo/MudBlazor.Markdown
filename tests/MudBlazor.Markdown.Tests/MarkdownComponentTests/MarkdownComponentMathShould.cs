namespace MudBlazor.Markdown.Tests.MarkdownComponentTests;

public sealed class MarkdownComponentMathShould : MarkdownComponentTestsBase
{
	[Fact]
	public void RenderEquals()
	{
		const string value = "$x = y$";
		const string expected =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>
		<mjx-container tabindex='0' class='mud-markdown-mjx-container'>
			<mi>x</mi>
			<mo class='pl-2'>=</mo>
			<mi class='pl-2'>y</mi>
		</mjx-container>
	</p>
</article>";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderSquireExpression()
	{
		const string value = "$(ax^2 + bx + c = 0)$";
		const string expected =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>
		<mjx-container tabindex='0' class='mud-markdown-mjx-container'>
			<mo>(</mo>
			<mi>a</mi>
			<msup>
				<mi>x</mi>
				<mn>2</mn>
			</msup>
			<mo class='pl-2'>&#x2B;</mo>
			<mi class='pl-2'>b</mi>
			<mi>x</mi>
			<mo class='pl-2'>&#x2B;</mo>
			<mi class='pl-2'>c</mi>
			<mo class='pl-2'>=</mo>
			<mn class='pl-2'>0</mn>
			<mo>)</mo>
		</mjx-container>
	</p>
</article>";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderNotEquals()
	{
		const string value = "$x \\ne y$";
		const string expected =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>
		<mjx-container tabindex='0' class='mud-markdown-mjx-container'>
			<mi>x</mi>
			<mo class='pl-2'>&#x2260;</mo>
			<mi class='pl-2'>y</mi>
		</mjx-container>
	</p>
</article>";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderLessThan()
	{
		const string value = "$x \\le y$";
		const string expected =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>
		<mjx-container tabindex='0' class='mud-markdown-mjx-container'>
			<mi>x</mi>
			<mo class='pl-2'>&#x2264;</mo>
			<mi class='pl-2'>y</mi>
		</mjx-container>
	</p>
</article>";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderGreaterThan()
	{
		const string value = "$x \\ge y$";
		const string expected =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>
		<mjx-container tabindex='0' class='mud-markdown-mjx-container'>
			<mi>x</mi>
			<mo class='pl-2'>&#x2265;</mo>
			<mi class='pl-2'>y</mi>
		</mjx-container>
	</p>
</article>";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderPowerContainer()
	{
		const string value = "$x^{n + 1}$";
		const string expected =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>
		<mjx-container tabindex='0' class='mud-markdown-mjx-container'>
			<msup>
				<mi>x</mi>
				<mrow>
					<mi>n</mi>
					<mo>&#x2B;</mo>
					<mn>1</mn>
				</mrow>
			</msup>
		</mjx-container>
	</p>
</article>";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}
}
