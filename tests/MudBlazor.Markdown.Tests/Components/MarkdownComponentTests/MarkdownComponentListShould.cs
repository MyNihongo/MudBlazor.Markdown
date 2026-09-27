namespace MudBlazor.Markdown.Tests.Components.MarkdownComponentTests;

public sealed class MarkdownComponentListShould : MarkdownComponentTestsBase
{
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
			<article id:ignore class='mud-markdown-body'>
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
			<article id:ignore class='mud-markdown-body'>
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
			<article id:ignore class='mud-markdown-body'>
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
			<article id:ignore class='mud-markdown-body'>
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
			<article id:ignore class="mud-markdown-body">
			  <ol>
			    <li>
			      <p class="mud-typography mud-typography-body1">
			        Connect to your MySQL server using a MySQL client, such as the <code>mysql</code> command-line tool:
			      </p>
			    </li>
			  </ol>
			  <div class="hljs mud-markdown-code-highlight">
			    <button
			      blazor:onclick="2"
			      type="button"
			      class="mud-button-root mud-icon-button mud-button mud-button-filled mud-button-filled-primary mud-button-filled-size-medium mud-ripple ma-2 mud-markdown-code-highlight-copybtn"
			      blazor:onclick:stopPropagation
			      blazor:elementReference=""
			    >
			      <span class="mud-icon-button-label"
			        ><svg
			          class="mud-icon-root mud-svg-icon mud-icon-size-medium"
			          focusable="false"
			          viewBox="0 0 24 24"
			          aria-hidden="true"
			          role="img"
			        >
			          <g><rect fill="none" height="24" width="24" /></g>
			          <g>
			            <path
			              d="M15,20H5V7c0-0.55-0.45-1-1-1h0C3.45,6,3,6.45,3,7v13c0,1.1,0.9,2,2,2h10c0.55,0,1-0.45,1-1v0C16,20.45,15.55,20,15,20z M20,16V4c0-1.1-0.9-2-2-2H9C7.9,2,7,2.9,7,4v12c0,1.1,0.9,2,2,2h9C19.1,18,20,17.1,20,16z M18,16H9V4h9V16z"
			            />
			          </g></svg
			      ></span>
			    </button>
			    <pre class="mud-typography mud-typography-body1"><code class="hljs language-bash">mysql -u username -p</code></pre>
			  </div>
			</article>
			""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void OverrideTypoUnorderedList()
	{
		const string value =
			"""
			- item1
			- item2
			""";

		const string expected =
			"""
			<article id:ignore class="mud-markdown-body">
				<ul>
					<li class="mud-typography mud-typography-h3">item1</li>
					<li class="mud-typography mud-typography-h3">item2</li>
				</ul>
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
	public void OverrideTypoOrderedList()
	{
		const string value =
			"""
			1. item1
			2. item2
			""";

		const string expected =
			"""
			<article id:ignore class="mud-markdown-body">
				<ol>
					<li class="mud-typography mud-typography-h3">item1</li>
					<li class="mud-typography mud-typography-h3">item2</li>
				</ol>
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
	public void OverrideTypoUnorderedListNested()
	{
		const string value =
			"""
			- item1
			  - item1.1
			  - item1.2
			- item2
			""";

		const string expected =
			"""
			<article id:ignore class="mud-markdown-body">
				<ul>
					<li class="mud-typography mud-typography-h3">
						item1
						<ul>
							<li class="mud-typography mud-typography-h3">item1.1</li>
							<li class="mud-typography mud-typography-h3">item1.2</li>
						</ul>
					</li>
					<li class="mud-typography mud-typography-h3">item2</li>
				</ul>
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
