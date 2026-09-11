using System.Text;

namespace MudBlazor.Markdown.Tests.Components.MarkdownComponentTests;

public sealed class MarkdownComponentSourceShould : MarkdownComponentTestsBase
{
	private static readonly MudMarkdownStyling Styling = new()
	{
		CodeBlock =
		{
			CopyButton = CodeBlockCopyButton.None,
		},
	};

	public MarkdownComponentSourceShould()
	{
		Ctx.Services
			.AddSingleton<IMudMarkdownExceptionFormatter, MudMarkdownExceptionFormatter>();
	}

	[Fact]
	public void ReadValueFromFile()
	{
		var value = Path.Combine("Resources", "test.md");
		const MarkdownSourceType sourceType = MarkdownSourceType.File;

		const string expected =
			"""
			<article id:ignore class="mud-markdown-body">
				<h1 id="main-events" class="mud-typography mud-typography-h1">Main events</h1>
				<p class="mud-typography mud-typography-body1">Sakura season in Kawaguchi</p>
			</article>
			""";

		using var fixture = CreateFixture(value, sourceType: sourceType)
			.AwaitArticle();

		using var fixtureFromCache = CreateFixture(value, sourceType: sourceType);

		fixture.MarkupMatches(expected);
		fixtureFromCache.MarkupMatches(expected);
	}

	[Fact]
	public void ReadValueFromNonExistingFile()
	{
		var value = Path.Combine("Resources", "i dont exist.md");
		const MarkdownSourceType sourceType = MarkdownSourceType.File;

		const string expected =
			"""
			<article id:ignore class="mud-markdown-body">
			    <p class="mud-typography mud-typography-body1">
			        <code>{message}</code>
			    </p>
			    <div class="hljs mud-markdown-code-highlight">
			        <pre><code class="hljs language-txt">{details}</code></pre>
			    </div>
			</article>
			""";

		using var fixture = CreateFixture(value, sourceType: sourceType, styling: Styling)
			.AwaitElement("article");

		fixture.Markup
			.EscapePath()
			.MarkupMatches(expected);
	}

	[Fact]
	public void RenderValueFromUrl()
	{
		const string value = "https://raw.githubusercontent.com/MyNihongo/MudBlazor.Markdown/refs/heads/main/tests/MudBlazor.Markdown.Tests/.resources/test.md";
		const MarkdownSourceType sourceType = MarkdownSourceType.Url;

		const string expected =
			"""
			<article id:ignore class="mud-markdown-body">
				<h1 id="main-events" class="mud-typography mud-typography-h1">Main events</h1>
				<p class="mud-typography mud-typography-body1">Sakura season in Kawaguchi</p>
			</article>
			""";

		using var fixture = CreateFixture(value, sourceType: sourceType)
			.AwaitElement("article", TimeSpan.FromSeconds(2d));

		using var fixtureFromCache = CreateFixture(value, sourceType: sourceType);

		fixture.MarkupMatches(expected);
		fixtureFromCache.MarkupMatches(expected);
	}

	[Fact]
	public void RenderValueFromInvalidUrl()
	{
		const string value = "invalid url";
		const MarkdownSourceType sourceType = MarkdownSourceType.Url;

		const string expected =
			"""
			<article id:ignore class="mud-markdown-body">
			    <p class="mud-typography mud-typography-body1">
			        <code>{message}</code>
			    </p>
			    <div class="hljs mud-markdown-code-highlight">
			        <pre><code class="hljs language-txt">{details}</code></pre>
			    </div>
			</article>
			""";

		using var fixture = CreateFixture(value, sourceType: sourceType, styling: Styling)
			.AwaitElement("article");

		fixture.MarkupMatches(expected);
	}
}

file sealed class MudMarkdownExceptionFormatter : IMudMarkdownExceptionFormatter
{
	public string Format(Exception ex)
	{
		return new StringBuilder()
			.AppendLine("`{message}`")
			.AppendLine("```txt")
			.AppendLine("{details}")
			.Append("```")
			.ToString();
	}
}
