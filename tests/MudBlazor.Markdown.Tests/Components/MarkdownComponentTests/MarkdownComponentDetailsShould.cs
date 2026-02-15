namespace MudBlazor.Markdown.Tests.Components.MarkdownComponentTests;

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
			<article id:ignore class="mud-markdown-body">
				<div class="mud-expand-panel mud-elevation-1 mud-expand-panel-border">
					<div class="mud-expand-panel-header mud-ripple" >
						<div class="mud-expand-panel-text">
							<p class="mud-typography mud-typography-body1">Header</p>
						</div>
						<svg class="mud-icon-root mud-svg-icon mud-icon-size-medium mud-expand-panel-icon" focusable="false" viewBox="0 0 24 24" aria-hidden="true" role="img">
							<path d="M0 0h24v24H0z" fill="none"></path>
							<path d="M16.59 8.59L12 13.17 7.41 8.59 6 10l6 6 6-6z"></path>
						</svg>
					</div>
					<div class="mud-collapse-container invisible" style="">
						<div class="mud-collapse-wrapper">
							<div class="mud-collapse-wrapper-inner">
								<div class="mud-expand-panel-content">
									<p class="mud-typography mud-typography-body1">Some hidden text Another text</p>
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
	public void RenderDetailsSummaryMultiline()
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
			 <article id:ignore class="mud-markdown-body">
				<div class="mud-expand-panel mud-elevation-1 mud-expand-panel-border">
					<div class="mud-expand-panel-header mud-ripple" >
						<div class="mud-expand-panel-text">
							<p class="mud-typography mud-typography-body1">Header</p>
						</div>
						<svg class="mud-icon-root mud-svg-icon mud-icon-size-medium mud-expand-panel-icon" focusable="false" viewBox="0 0 24 24" aria-hidden="true" role="img">
							<path d="M0 0h24v24H0z" fill="none"></path>
							<path d="M16.59 8.59L12 13.17 7.41 8.59 6 10l6 6 6-6z"></path>
						</svg>
					</div>
					<div	class="mud-collapse-container invisible" style="">
						<div class="mud-collapse-wrapper">
							<div class="mud-collapse-wrapper-inner">
								<div class="mud-expand-panel-content">
									<p class="mud-typography mud-typography-body1">Some hidden text Another text</p>
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
			<article id:ignore class="mud-markdown-body">
				<div class="mud-expand-panel mud-elevation-1 mud-expand-panel-border">
					<div class="mud-expand-panel-header mud-ripple" >
						<div class="mud-expand-panel-text">
							<p class="mud-typography mud-typography-body1">Header</p>
						</div>
						<svg class="mud-icon-root mud-svg-icon mud-icon-size-medium mud-expand-panel-icon" focusable="false" viewBox="0 0 24 24" aria-hidden="true" role="img">
							<path d="M0 0h24v24H0z" fill="none"></path>
							<path d="M16.59 8.59L12 13.17 7.41 8.59 6 10l6 6 6-6z"></path>
						</svg>
					</div>
					<div class="mud-collapse-container invisible" style="">
						<div class="mud-collapse-wrapper">
							<div class="mud-collapse-wrapper-inner">
								<div class="mud-expand-panel-content">
									<p class="mud-typography mud-typography-body1">Some hidden tex</p>
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
			<article id:ignore class='mud-markdown-body'>
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
			<article id:ignore class='mud-markdown-body'>
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
									<p class='mud-typography mud-typography-body1'>Some hidden text Another text</p>
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
			<article id:ignore class="mud-markdown-body">
				<div class="mud-expand-panel mud-elevation-1 mud-expand-panel-border">
					<div class="mud-expand-panel-header mud-ripple" >
						<div class="mud-expand-panel-text">
							<p class="mud-typography mud-typography-body1">First</p>
						</div>
						<svg class="mud-icon-root mud-svg-icon mud-icon-size-medium mud-expand-panel-icon" focusable="false" viewBox="0 0 24 24" aria-hidden="true" role="img">
							<path d="M0 0h24v24H0z" fill="none"></path>
							<path d="M16.59 8.59L12 13.17 7.41 8.59 6 10l6 6 6-6z"></path>
						</svg>
					</div>
					<div class="mud-collapse-container invisible" style="">
						<div class="mud-collapse-wrapper">
							<div class="mud-collapse-wrapper-inner">
								<div class="mud-expand-panel-content">
									<div class="mud-expand-panel mud-elevation-1 mud-expand-panel-border">
										<div class="mud-expand-panel-header mud-ripple" >
											<div class="mud-expand-panel-text">
												<p class="mud-typography mud-typography-body1">Second</p>
											</div>
											<svg class="mud-icon-root mud-svg-icon mud-icon-size-medium mud-expand-panel-icon" focusable="false" viewBox="0 0 24 24" aria-hidden="true" role="img">
												<path d="M0 0h24v24H0z" fill="none"></path>
												<path d="M16.59 8.59L12 13.17 7.41 8.59 6 10l6 6 6-6z"></path>
											</svg>
										</div>
										<div class="mud-collapse-container invisible" style="">
											<div class="mud-collapse-wrapper">
												<div class="mud-collapse-wrapper-inner">
													<div class="mud-expand-panel-content">
														<div class="mud-expand-panel mud-elevation-1 mud-expand-panel-border">
															<div class="mud-expand-panel-header mud-ripple" >
																<div class="mud-expand-panel-text">
																	<p class="mud-typography mud-typography-body1">Third</p>
																</div>
																<svg class="mud-icon-root mud-svg-icon mud-icon-size-medium mud-expand-panel-icon" focusable="false" viewBox="0 0 24 24" aria-hidden="true" role="img">
																	<path d="M0 0h24v24H0z" fill="none"></path>
																	<path d="M16.59 8.59L12 13.17 7.41 8.59 6 10l6 6 6-6z"></path>
																</svg>
															</div>
															<div class="mud-collapse-container invisible" style="">
																<div class="mud-collapse-wrapper">
																	<div class="mud-collapse-wrapper-inner">
																		<div class="mud-expand-panel-content">
																			<p class="mud-typography mud-typography-body1">Some data</p>
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
