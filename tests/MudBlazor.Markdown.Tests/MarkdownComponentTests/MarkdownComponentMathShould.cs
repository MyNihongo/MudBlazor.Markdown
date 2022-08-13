namespace MudBlazor.Markdown.Tests.MarkdownComponentTests;

public sealed class MarkdownComponentMathShould : MarkdownComponentTestsBase
{
	[Fact]
	public void RenderPowerDigit()
	{
		const string value = "$x^2$";
		const string expected =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>
		<mjx-container tabindex='0' class='mud-markdown-mjx-container'>
			<msup>
				<mi>x</mi>
				<mn>2</mn>
			</msup>
		</mjx-container>
	</p>
</article>";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderPowerAlpha()
	{
		const string value = "$x^y$";
		const string expected =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>
		<mjx-container tabindex='0' class='mud-markdown-mjx-container'>
			<msup>
				<mi>x</mi>
				<mi>y</mi>
			</msup>
		</mjx-container>
	</p>
</article>";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderPowerRow()
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

	[Fact]
	public void RenderPowerParentheses()
	{
		const string value = "$(x + 1)^{n + 1}$";
		const string expected =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>
		<mjx-container tabindex='0' class='mud-markdown-mjx-container'>
			<mo>(</mo>
			<mi>x</mi>
			<mo class=""pl-2"">&#x2B;</mo>
			<mn class=""pl-2"">1</mn>
			<msup>
				<mo>)</mo>
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

	[Fact]
	public void RenderSquireEquation()
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
	public void RenderSquireExpression()
	{
		const string value = "$x^y - y^x = 123$";
		const string expected =
@"<article class='mud-markdown-body'>
		<p class='mud-typography mud-typography-body1'>
			<mjx-container tabindex='0' class='mud-markdown-mjx-container'>
				<msup>
					<mi>x</mi>
					<mi>y</mi>
				</msup>
				<mo class=""pl-2"">&#x2212;</mo>
				<msup>
					<mi class=""pl-2"">y</mi>
					<mi>x</mi>
				</msup>
				<mo class=""pl-2"">=</mo>
				<mn class=""pl-2"">1</mn>
				<mn>2</mn>
				<mn>3</mn>
			</mjx-container>
		</p>
	</article>";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderSubDigit()
	{
		const string value = "$x_2$";
		const string expected =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>
		<mjx-container tabindex='0' class='mud-markdown-mjx-container'>
			<msub>
				<mi>x</mi>
				<mn>2</mn>
			</msub>
		</mjx-container>
	</p>
</article>";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderSubAlpha()
	{
		const string value = "$x_y$";
		const string expected =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>
		<mjx-container tabindex='0' class='mud-markdown-mjx-container'>
			<msub>
				<mi>x</mi>
				<mi>y</mi>
			</msub>
		</mjx-container>
	</p>
</article>";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderSubRow()
	{
		const string value = "$x_{n + 1}$";
		const string expected =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>
		<mjx-container tabindex='0' class='mud-markdown-mjx-container'>
			<msub>
				<mi>x</mi>
				<mrow>
					<mi>n</mi>
					<mo>&#x2B;</mo>
					<mn>1</mn>
				</mrow>
			</msub>
		</mjx-container>
	</p>
</article>";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderSubParentheses()
	{
		const string value = "$(x + 1)_{n + 1}$";
		const string expected =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>
		<mjx-container tabindex='0' class='mud-markdown-mjx-container'>
			<mo>(</mo>
			<mi>x</mi>
			<mo class=""pl-2"">&#x2B;</mo>
			<mn class=""pl-2"">1</mn>
			<msub>
				<mo>)</mo>
				<mrow>
					<mi>n</mi>
					<mo>&#x2B;</mo>
					<mn>1</mn>
				</mrow>
			</msub>
		</mjx-container>
	</p>
</article>";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderSubEquation()
	{
		const string value = "$(ax_2 + bx + c = 0)$";
		const string expected =
@"<article class='mud-markdown-body'>
		<p class='mud-typography mud-typography-body1'>
			<mjx-container tabindex='0' class='mud-markdown-mjx-container'>
				<mo>(</mo>
				<mi>a</mi>
				<msub>
					<mi>x</mi>
					<mn>2</mn>
				</msub>
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
	public void RenderSubExpression()
	{
		const string value = "$x_y - y_x = 123$";
		const string expected =
@"<article class='mud-markdown-body'>
		<p class='mud-typography mud-typography-body1'>
			<mjx-container tabindex='0' class='mud-markdown-mjx-container'>
				<msub>
					<mi>x</mi>
					<mi>y</mi>
				</msub>
				<mo class=""pl-2"">&#x2212;</mo>
				<msub>
					<mi class=""pl-2"">y</mi>
					<mi>x</mi>
				</msub>
				<mo class=""pl-2"">=</mo>
				<mn class=""pl-2"">1</mn>
				<mn>2</mn>
				<mn>3</mn>
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
	public void RenderOverlineBlock()
	{
		const string value = "$\\overline{x + 1}$";
		const string expected =
@"<article class='mud-markdown-body'>
		<p class='mud-typography mud-typography-body1'>
			<mjx-container tabindex='0' class='mud-markdown-mjx-container'>
				<mover>
					<mrow>
						<mi>x</mi>
						<mo class='pl-2'>&#x2B;</mo>
						<mn class='pl-2'>1</mn>
					</mrow>
				</mover>
			</mjx-container>
		</p>
	</article>";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}
}
