namespace MudBlazor.Markdown.Tests.Components.MarkdownComponentTests;

public sealed class MarkdownComponentTableOfContentsShould : MarkdownComponentTestsBase
{
	[Fact]
	public void RenderDefault()
	{
		const string value =
			"""
			# heading 1
			aaa
			## heading 2
			bbb
			### heading 3
			ccc
			#### heading 4
			ddd
			### heading 5
			eee
			## heading 6
			fff
			### heading 7
			ggg
			# heading 8
			hhh
			""";

		const string expected =
			"""
			<div class="mud-markdown-toc">
				<div class="mud-markdown-toc-drawer">
					<button type="button" class="mud-button-root mud-icon-button mud-button mud-button-filled mud-button-filled-primary mud-button-filled-size-large mud-ripple mud-icon-button-size-large mud-markdown-toc-btn-toggle"	>
						<span class="mud-icon-button-label">
							<svg class="mud-icon-root mud-svg-icon mud-icon-size-large" focusable="false" viewBox="0 0 24 24" aria-hidden="true" role="img">
								<path d="M0 0h24v24H0z" fill="none"></path>
								<path d="M3 18h18v-2H3v2zm0-5h18v-2H3v2zm0-7v2h18V6H3z"></path>
							</svg>
						</span>
					</button>
					<div class="mud-markdown-toc-drawer-content open">
						<nav class="mud-navmenu mud-navmenu-default mud-navmenu-margin-none mud-markdown-toc-nav-menu">
							<nav class="mud-nav-group">
								<div class="mud-nav-item mud-nav-link-h1">
									<div class="mud-nav-link mud-ripple" tabindex="0">
										<div class="mud-nav-link-text">heading 1</div>
									</div>
								</div>
							</nav>
							<nav class="mud-nav-group">
								<div class="mud-nav-item mud-nav-link-h2">
									<div class="mud-nav-link mud-ripple" tabindex="0">
										<div class="mud-nav-link-text">heading 2</div>
									</div>
								</div>
							</nav>
							<nav class="mud-nav-group">
								<div class="mud-nav-item mud-nav-link-h3">
									<div class="mud-nav-link mud-ripple" tabindex="0">
										<div class="mud-nav-link-text">heading 3</div>
									</div>
								</div>
							</nav>
							<nav class="mud-nav-group">
								<div class="mud-nav-item mud-nav-link-h3">
									<div class="mud-nav-link mud-ripple" tabindex="0">
										<div class="mud-nav-link-text">heading 5</div>
									</div>
								</div>
							</nav>
							<nav class="mud-nav-group">
								<div class="mud-nav-item mud-nav-link-h2">
									<div class="mud-nav-link mud-ripple" tabindex="0">
										<div class="mud-nav-link-text">heading 6</div>
									</div>
								</div>
							</nav>
							<nav class="mud-nav-group">
								<div class="mud-nav-item mud-nav-link-h3">
									<div class="mud-nav-link mud-ripple" tabindex="0">
										<div class="mud-nav-link-text">heading 7</div>
									</div>
								</div>
							</nav>
							<nav class="mud-nav-group">
								<div class="mud-nav-item mud-nav-link-h1">
									<div class="mud-nav-link mud-ripple" tabindex="0">
										<div class="mud-nav-link-text">heading 8</div>
									</div>
								</div>
							</nav>
						</nav>
					</div>
				</div>
				<div class="mud-markdown-toc-content open">
					<article id:ignore class="mud-markdown-body">
						<h1 id="heading-1" class="mud-typography mud-typography-h1 mud-markdown-toc-heading">heading 1</h1>
						<p class="mud-typography mud-typography-body1">aaa</p>
						<h2 id="heading-2" class="mud-typography mud-typography-h2 mud-markdown-toc-heading">heading 2</h2>
						<p class="mud-typography mud-typography-body1">bbb</p>
						<h3 id="heading-3" class="mud-typography mud-typography-h3 mud-markdown-toc-heading">heading 3</h3>
						<p class="mud-typography mud-typography-body1">ccc</p>
						<h4 id="heading-4" class="mud-typography mud-typography-h4">heading 4</h4>
						<p class="mud-typography mud-typography-body1">ddd</p>
						<h3 id="heading-5" class="mud-typography mud-typography-h3 mud-markdown-toc-heading">heading 5</h3>
						<p class="mud-typography mud-typography-body1">eee</p>
						<h2 id="heading-6" class="mud-typography mud-typography-h2 mud-markdown-toc-heading">heading 6</h2>
						<p class="mud-typography mud-typography-body1">fff</p>
						<h3 id="heading-7" class="mud-typography mud-typography-h3 mud-markdown-toc-heading">heading 7</h3>
						<p class="mud-typography mud-typography-body1">ggg</p>
						<h1 id="heading-8" class="mud-typography mud-typography-h1 mud-markdown-toc-heading">heading 8</h1>
						<p class="mud-typography mud-typography-body1">hhh</p>
					</article>
				</div>
			</div>
			""";

		using var fixture = CreateFixture(value, hasTableOfContents: true);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderWithCustomHeader()
	{
		const string value =
			"""
			# heading 1
			## heading 2
			### heading 3
			""";

		const string expected =
			"""
			<div class="mud-markdown-toc">
				<div class="mud-markdown-toc-drawer">
					<button type="button" class="mud-button-root mud-icon-button mud-button mud-button-filled mud-button-filled-primary mud-button-filled-size-large mud-ripple mud-icon-button-size-large mud-markdown-toc-btn-toggle"	>
						<span class="mud-icon-button-label">
							<svg class="mud-icon-root mud-svg-icon mud-icon-size-large" focusable="false" viewBox="0 0 24 24" aria-hidden="true" role="img">
								<path d="M0 0h24v24H0z" fill="none"></path>
								<path d="M3 18h18v-2H3v2zm0-5h18v-2H3v2zm0-7v2h18V6H3z"></path>
							</svg>
						</span>
					</button>
					<div class="mud-markdown-toc-drawer-content open">
						<div class="mud-drawer-header">
							<h6 class="mud-typography mud-typography-h6">Cool header</h6>
						</div>
						<nav class="mud-navmenu mud-navmenu-default mud-navmenu-margin-none mud-markdown-toc-nav-menu">
							<nav class="mud-nav-group">
								<div class="mud-nav-item mud-nav-link-h1">
									<div class="mud-nav-link mud-ripple" tabindex="0">
										<div class="mud-nav-link-text">heading 1</div>
									</div>
								</div>
							</nav>
							<nav class="mud-nav-group">
								<div class="mud-nav-item mud-nav-link-h2">
									<div class="mud-nav-link mud-ripple" tabindex="0">
										<div class="mud-nav-link-text">heading 2</div>
									</div>
								</div>
							</nav>
							<nav class="mud-nav-group">
								<div class="mud-nav-item mud-nav-link-h3">
									<div class="mud-nav-link mud-ripple" tabindex="0">
										<div class="mud-nav-link-text">heading 3</div>
									</div>
								</div>
							</nav>
						</nav>
					</div>
				</div>
				<div class="mud-markdown-toc-content open">
					<article id:ignore class="mud-markdown-body">
						<h1 id="heading-1" class="mud-typography mud-typography-h1 mud-markdown-toc-heading">heading 1</h1>
						<h2 id="heading-2" class="mud-typography mud-typography-h2 mud-markdown-toc-heading">heading 2</h2>
						<h3 id="heading-3" class="mud-typography mud-typography-h3 mud-markdown-toc-heading">heading 3</h3>
					</article>
				</div>
			</div>
			""";

		using var fixture = CreateFixture(value, hasTableOfContents: true, tableOfContentsHeader: "Cool header");
		fixture.MarkupMatches(expected);
	}
}
