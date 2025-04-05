namespace MudBlazor.Markdown.Tests.MarkdownComponentTests;

public sealed class MarkdownComponentSourceShould : MarkdownComponentTestsBase
{
	[Fact]
	public void ReadValueFromFile()
	{
		var value = Path.Combine("Resources", "test.md");
		const MarkdownSourceType sourceType = MarkdownSourceType.File;

		const string expected =
			"""
			<article class="mud-markdown-body">
				<h1 id="main-events" class="mud-typography mud-typography-h1">Main events</h1>
				<p class="mud-typography mud-typography-body1">Sakura season in Kawaguchi</p>
			</article>
			""";

		using var fixture = CreateFixture(value, sourceType: sourceType)
			.AwaitElement("article");

		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void ReadValueFromNonExistingFile()
	{
		var value = Path.Combine("Resources", "i dont exist.md");
		const MarkdownSourceType sourceType = MarkdownSourceType.File;

		const string expected =
			"""
			<article class="mud-markdown-body">
				<p class="mud-typography mud-typography-body1">Error while reading from file, path=<code>Resources\i dont exist.md</code>, error=<code>Could not find file '{path}'.</code></p>
				<div class="snippet-clipboard-content overflow-auto">
					<button  type="button" class="mud-button-root mud-icon-button mud-button mud-button-filled mud-button-filled-primary mud-button-filled-size-medium mud-ripple snippet-clipboard-copy-icon ma-2"  >
						<span class="mud-icon-button-label">
							<svg class="mud-icon-root mud-svg-icon mud-icon-size-medium" focusable="false" viewBox="0 0 24 24" aria-hidden="true" role="img">
								<g><rect fill="none" height="24" width="24"></rect></g>
								<g><path d="M15,20H5V7c0-0.55-0.45-1-1-1h0C3.45,6,3,6.45,3,7v13c0,1.1,0.9,2,2,2h10c0.55,0,1-0.45,1-1v0C16,20.45,15.55,20,15,20z M20,16V4c0-1.1-0.9-2-2-2H9C7.9,2,7,2.9,7,4v12c0,1.1,0.9,2,2,2h9C19.1,18,20,17.1,20,16z M18,16H9V4h9V16z"></path></g>
							</svg>
						</span>
					</button>
					<pre><code class="hljs language-txt" ></code></pre>
				</div>
			</article>
			""";

		using var fixture = CreateFixture(value, sourceType: sourceType)
			.AwaitElement("article");

		fixture.Markup
			.EscapePath()
			.MarkupMatches(expected);
	}

	[Fact]
	public void RenderValueFromUrl()
	{
		const string value = "https://raw.githubusercontent.com/MyNihongo/MudBlazor.Markdown/refs/heads/feature/288-markdown-source/tests/MudBlazor.Markdown.Tests/.resources/test.md";
		const MarkdownSourceType sourceType = MarkdownSourceType.Url;

		const string expected =
			"""
			<article class="mud-markdown-body">
				<h1 id="main-events" class="mud-typography mud-typography-h1">Main events</h1>
				<p class="mud-typography mud-typography-body1">Sakura season in Kawaguchi</p>
			</article>
			""";

		using var fixture = CreateFixture(value, sourceType: sourceType)
			.AwaitElement("article", TimeSpan.FromSeconds(2d));

		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderValueFromInvalidUrl()
	{
		const string value = "invalid url";
		const MarkdownSourceType sourceType = MarkdownSourceType.Url;

		const string expected =
			"""
			<article class="mud-markdown-body">
				<p class="mud-typography mud-typography-body1">Error while reading from URL, URL=<code>invalid url</code>, error=<code>An invalid request URI was provided. Either the request URI must be an absolute URI or BaseAddress must be set.</code>
				<div class="snippet-clipboard-content overflow-auto">
					<button  type="button" class="mud-button-root mud-icon-button mud-button mud-button-filled mud-button-filled-primary mud-button-filled-size-medium mud-ripple snippet-clipboard-copy-icon ma-2"  >
						<span class="mud-icon-button-label">
							<svg class="mud-icon-root mud-svg-icon mud-icon-size-medium" focusable="false" viewBox="0 0 24 24" aria-hidden="true" role="img">
								<g><rect fill="none" height="24" width="24"></rect></g>
								<g><path d="M15,20H5V7c0-0.55-0.45-1-1-1h0C3.45,6,3,6.45,3,7v13c0,1.1,0.9,2,2,2h10c0.55,0,1-0.45,1-1v0C16,20.45,15.55,20,15,20z M20,16V4c0-1.1-0.9-2-2-2H9C7.9,2,7,2.9,7,4v12c0,1.1,0.9,2,2,2h9C19.1,18,20,17.1,20,16z M18,16H9V4h9V16z"></path></g>
							</svg>
						</span>
					</button>
					<pre><code class="hljs language-txt" ></code></pre>
				</div>
			</article>
			""";

		using var fixture = CreateFixture(value, sourceType: sourceType)
			.AwaitElement("article");

		fixture.MarkupMatches(expected);
	}
}
