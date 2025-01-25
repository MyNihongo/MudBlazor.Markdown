namespace MudBlazor.Markdown.Tests.MarkdownComponentTests;

public sealed class MarkdownComponentDetailsShould : MarkdownComponentTestsBase
{
	[Fact]
	public void RenderDetailsMultiline()
	{
		const string value =
"""
<details>
	<summary>Header</summary>
	Some hidden text
	Another text
</details>
""";

		const string expected =
"""
<article class='mud-markdown-body'>
	<div class='mud-expand-panel mud-elevation-1 mud-expand-panel-border'>
		<div class='mud-expand-panel-header mud-ripple' blazor:onclick='1'>
			<div class='mud-expand-panel-text'>
				<p class='mud-typography mud-typography-body1'>Header</p>
			</div>
			<svg class='mud-icon-root mud-svg-icon mud-icon-size-medium mud-expand-panel-icon' focusable='false' viewBox='0 0 24 24' aria-hidden='true' role='img'>
				<path d='M0 0h24v24H0z' fill='none'/>
				<path d='M16.59 8.59L12 13.17 7.41 8.59 6 10l6 6 6-6z'/>
			</svg>
		</div>
		<div blazor:onanimationend='2' class='mud-collapse-container' style=''>
			<div class='mud-collapse-wrapper' blazor:elementReference='f0339543-23a6-43d6-92b9-b8b55ec88622'>
				<div class='mud-collapse-wrapper-inner'>
					<div class='mud-expand-panel-content'>
						<p class='mud-typography mud-typography-body1'>Some hidden text<br />Another text</p>
					</div>
				</div>
			</div>
		</div>
	</div>
</article>
""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderDetailsSummaryMultiLine()
	{
		const string value =
"""
<details>
	<summary>
		Header
	</summary>
	Some hidden text
	Another text
</details>
""";

		const string expected =
"""
<article class='mud-markdown-body'>
	<div class='mud-expand-panel mud-elevation-1 mud-expand-panel-border'>
		<div class='mud-expand-panel-header mud-ripple' blazor:onclick='1'>
			<div class='mud-expand-panel-text'>
				<p class='mud-typography mud-typography-body1'>Header</p>
			</div>
			<svg class='mud-icon-root mud-svg-icon mud-icon-size-medium mud-expand-panel-icon' focusable='false' viewBox='0 0 24 24' aria-hidden='true' role='img'>
				<path d='M0 0h24v24H0z' fill='none'/>
				<path d='M16.59 8.59L12 13.17 7.41 8.59 6 10l6 6 6-6z'/>
			</svg>
		</div>
		<div blazor:onanimationend='2' class='mud-collapse-container' style=''>
			<div class='mud-collapse-wrapper' blazor:elementReference='99a17eaa-605d-411d-968b-457a412ddd48'>
				<div class='mud-collapse-wrapper-inner'>
					<div class='mud-expand-panel-content'>
						<p class='mud-typography mud-typography-body1'>Some hidden text<br />Another text</p>
					</div>
				</div>
			</div>
		</div>
	</div>
</article>
""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderDetailsInline()
	{
		const string value =
"<details><summary>Header</summary>Some hidden text</details>";

		const string expected =
"""
<article class='mud-markdown-body'>
	<div class='mud-expand-panel mud-elevation-1 mud-expand-panel-border'>
		<div class='mud-expand-panel-header mud-ripple' blazor:onclick='1'>
			<div class='mud-expand-panel-text'>
				<p class='mud-typography mud-typography-body1'>Header</p>
			</div>
			<svg class='mud-icon-root mud-svg-icon mud-icon-size-medium mud-expand-panel-icon' focusable='false' viewBox='0 0 24 24' aria-hidden='true' role='img'>
				<path d='M0 0h24v24H0z' fill='none'/>
				<path d='M16.59 8.59L12 13.17 7.41 8.59 6 10l6 6 6-6z'/>
			</svg>
		</div>
		<div blazor:onanimationend='2' class='mud-collapse-container' style=''>
			<div class='mud-collapse-wrapper' blazor:elementReference='68fe8a2c-e680-4a80-8e06-e62217d04ef8'>
				<div class='mud-collapse-wrapper-inner'>
					<div class='mud-expand-panel-content'>
						<p class='mud-typography mud-typography-body1'>Some hidden tex</p>
					</div>
				</div>
			</div>
		</div>
	</div>
</article>
""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderDetailsAsHtml()
	{
		const string value =
"""
<div>
	<details>
		<summary>Header</summary>
		Some hidden text
		Another text
	</details>
</div>
""";

		const string expected =
"""
<article class='mud-markdown-body'>
	<div>
		<details>
			<summary>Header</summary>
			Some hidden text
			Another text
		</details>
	</div>
</article>
""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void BeExpandedOnClick()
	{
		const string value =
"""
<details>
	<summary>Header</summary>
	Some hidden text
	Another text
</details>
""";

		const string expected =
"""
<article class='mud-markdown-body'>
	<div class='mud-expand-panel mud-elevation-1 mud-expand-panel-border'>
		<div class='mud-expand-panel-header mud-ripple' blazor:onclick='1'>
			<div class='mud-expand-panel-text'>
				<p class='mud-typography mud-typography-body1'>Header</p>
			</div>
			<svg class='mud-icon-root mud-svg-icon mud-icon-size-medium mud-expand-panel-icon mud-transform' focusable='false' viewBox='0 0 24 24' aria-hidden='true' role='img'>
				<path d='M0 0h24v24H0z' fill='none'/>
				<path d='M16.59 8.59L12 13.17 7.41 8.59 6 10l6 6 6-6z'/>
			</svg>
		</div>
		<div blazor:onanimationend='4' class='mud-collapse-container mud-collapse-entering' style=''>
			<div class='mud-collapse-wrapper' blazor:elementReference=''>
				<div class='mud-collapse-wrapper-inner'>
					<div class='mud-expand-panel-content'>
						<p class='mud-typography mud-typography-body1'>Some hidden text<br />Another text</p>
					</div>
				</div>
			</div>
		</div>
	</div>
</article>
""";

		using var fixture = CreateFixture(value);
		var header = fixture.Find(".mud-expand-panel-header");
		header.Click();

		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderNestedDetails()
	{
		const string value =
"""
<details>
	<summary>First</summary>
	<details>
		<summary>Second</summary>
		<details>
			<summary>Third</summary>
			Some data
		</details>
	</details>
</details>
""";

		const string expected =
"""
<article class='mud-markdown-body'>
	<div class='mud-expand-panel mud-elevation-1 mud-expand-panel-border'>
		<div class='mud-expand-panel-header mud-ripple' blazor:onclick='1'>
			<div class='mud-expand-panel-text'>
				<p class='mud-typography mud-typography-body1'>First</p>
			</div>
			<svg class='mud-icon-root mud-svg-icon mud-icon-size-medium mud-expand-panel-icon' focusable='false' viewBox='0 0 24 24' aria-hidden='true' role='img'>
				<path d='M0 0h24v24H0z' fill='none'/>
				<path d='M16.59 8.59L12 13.17 7.41 8.59 6 10l6 6 6-6z'/>
			</svg>
		</div>
		<div blazor:onanimationend='2' class='mud-collapse-container' style=''>
			<div class='mud-collapse-wrapper' blazor:elementReference='48c895d6-251f-406a-b7f1-62609e7e2256'>
				<div class='mud-collapse-wrapper-inner'>
					<div class='mud-expand-panel-content'>
						<div class='mud-expand-panel mud-elevation-1 mud-expand-panel-border'>
							<div class='mud-expand-panel-header mud-ripple' blazor:onclick='2'>
								<div class='mud-expand-panel-text'>
									<p class='mud-typography mud-typography-body1'>Second</p>
								</div>
								<svg class='mud-icon-root mud-svg-icon mud-icon-size-medium mud-expand-panel-icon' focusable='false' viewBox='0 0 24 24' aria-hidden='true' role='img'>
									<path d='M0 0h24v24H0z' fill='none'/><path d='M16.59 8.59L12 13.17 7.41 8.59 6 10l6 6 6-6z'/>
								</svg>
							</div>
							<div blazor:onanimationend='2' class='mud-collapse-container' style=''>
								<div class='mud-collapse-wrapper' blazor:elementReference='97064b7c-1652-472e-89cf-ebe6cc63dee9'>
									<div class='mud-collapse-wrapper-inner'>
										<div class='mud-expand-panel-content'>
											<div class='mud-expand-panel mud-elevation-1 mud-expand-panel-border'>
												<div class='mud-expand-panel-header mud-ripple' blazor:onclick='3'>
													<div class='mud-expand-panel-text'>
														<p class='mud-typography mud-typography-body1'>Third</p>
													</div>
													<svg class='mud-icon-root mud-svg-icon mud-icon-size-medium mud-expand-panel-icon' focusable='false' viewBox='0 0 24 24' aria-hidden='true' role='img'>
														<path d='M0 0h24v24H0z' fill='none'/><path d='M16.59 8.59L12 13.17 7.41 8.59 6 10l6 6 6-6z'/>
													</svg>
												</div>
												<div blazor:onanimationend='2' class='mud-collapse-container' style=''>
													<div class='mud-collapse-wrapper' blazor:elementReference='b051f40f-6621-407e-ba8d-c0be30338c31'>
														<div class='mud-collapse-wrapper-inner'>
															<div class='mud-expand-panel-content'>
																<p class='mud-typography mud-typography-body1'>Some data</p>
															</div>
														</div>
													</div>
												</div>
											</div>
										</div>
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
</article>
""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}
}
