namespace MudBlazor.Markdown.Tests.Components.MarkdownComponentTests;

public sealed class MarkdownComponentCodeBlockShould : MarkdownComponentTestsBase
{
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
			<article id:ignore class='mud-markdown-body'>
				<div class='hljs mud-markdown-code-highlight'>
					<button blazor:onclick='1' type='button' class='mud-button-root mud-icon-button mud-button mud-button-filled mud-button-filled-primary mud-button-filled-size-medium mud-ripple mud-markdown-code-highlight-copybtn ma-2' blazor:onclick:stopPropagation blazor:elementReference='af22cf66-3ea5-4899-bd97-91b4fdc35b82'>
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
	public void RenderCodeBlockIndented()
	{
		const string value =
			"""
			    if (condition)
			    {
			        return;
			    }
			""";

		const string expected =
			"""
			<article id:ignore class="mud-markdown-body">
				<div class="hljs mud-markdown-code-highlight">
					<button blazor:onclick="2" type="button" class="mud-button-root mud-icon-button mud-button mud-button-filled mud-button-filled-primary mud-button-filled-size-medium mud-ripple mud-markdown-code-highlight-copybtn ma-2" blazor:onclick:stopPropagation blazor:elementReference="">
						<span class="mud-icon-button-label">
							<svg class="mud-icon-root mud-svg-icon mud-icon-size-medium" focusable="false" viewBox="0 0 24 24" aria-hidden="true" role="img">
								<g><rect fill="none" height="24" width="24"/></g>
								<g><path d="M15,20H5V7c0-0.55-0.45-1-1-1h0C3.45,6,3,6.45,3,7v13c0,1.1,0.9,2,2,2h10c0.55,0,1-0.45,1-1v0C16,20.45,15.55,20,15,20z M20,16V4c0-1.1-0.9-2-2-2H9C7.9,2,7,2.9,7,4v12c0,1.1,0.9,2,2,2h9C19.1,18,20,17.1,20,16z M18,16H9V4h9V16z"/></g>
							</svg>
						</span>
					</button>
					<pre><code class="hljs" blazor:elementReference="1dc2b260-8556-4dee-8c4f-05fc6f08b05a"></code></pre>
					</div>
			</article>
			""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderCodeBlockWithoutCopyButton()
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

		var styling = new MudMarkdownStyling
		{
			CodeBlock =
			{
				CopyButton = CodeBlockCopyButton.None,
			},
		};

		const string expected =
			"""
			<article id:ignore class='mud-markdown-body'>
				<div class='hljs mud-markdown-code-highlight'>
					<pre><code class='hljs language-cs' blazor:elementReference='3b498767-f59e-4a18-a27d-a828bf3dd0e5'></code></pre>
				</div>
			</article>
			""";

		using var fixture = CreateFixture(value, styling: styling);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderCodeBlockWithStickyButton()
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

		var styling = new MudMarkdownStyling
		{
			CodeBlock =
			{
				CopyButton = CodeBlockCopyButton.Sticky,
			},
		};

		const string expected =
			"""
			<article id:ignore class='mud-markdown-body'>
				<div class='hljs mud-markdown-code-highlight-sticky'>
					<button  type="button" class="mud-button-root mud-icon-button mud-button mud-button-filled mud-button-filled-primary mud-button-filled-size-medium mud-ripple ma-2 mud-markdown-code-highlight-copybtn-sticky"  >
						<span class="mud-icon-button-label">
							<svg class="mud-icon-root mud-svg-icon mud-icon-size-medium" focusable="false" viewBox="0 0 24 24" aria-hidden="true" role="img">
								<g><rect fill="none" height="24" width="24"></rect></g>
								<g><path d="M15,20H5V7c0-0.55-0.45-1-1-1h0C3.45,6,3,6.45,3,7v13c0,1.1,0.9,2,2,2h10c0.55,0,1-0.45,1-1v0C16,20.45,15.55,20,15,20z M20,16V4c0-1.1-0.9-2-2-2H9C7.9,2,7,2.9,7,4v12c0,1.1,0.9,2,2,2h9C19.1,18,20,17.1,20,16z M18,16H9V4h9V16z"></path></g>
							</svg>
						</span>
					</button>
					<pre><code class='hljs language-cs' blazor:elementReference='3b498767-f59e-4a18-a27d-a828bf3dd0e5'></code></pre>
				</div>
			</article>
			""";

		using var fixture = CreateFixture(value, styling: styling);
		fixture.MarkupMatches(expected);
	}
}
