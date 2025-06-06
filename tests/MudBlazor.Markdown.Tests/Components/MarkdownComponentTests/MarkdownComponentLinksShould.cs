using Markdig.Syntax.Inlines;
using MyNihongo.Option;

namespace MudBlazor.Markdown.Tests.Components.MarkdownComponentTests;

public sealed class MarkdownComponentLinksShould : MarkdownComponentTestsBase
{
	private const string MethodIdentifier = "MudBlazorMarkdown.scrollToElementId";

	[Fact]
	public void InvokeNavigationIfHasId()
	{
		Uri = "#tokyo";
		const string value = "## some text";

		using (CreateFixture(value))
		{
		}

		MockJsRuntime
			.Verify(x => x.InvokeAsync<object>(MethodIdentifier, new object?[] { "tokyo", null }), Times.Once);
	}

	[Fact]
	public void NotInvokeNavigationIfNoId()
	{
		Uri = "/tokyo";
		const string value = "## some text";

		using (CreateFixture(value))
		{
		}

		MockJsRuntime
			.Verify(x => x.InvokeAsync<object>(MethodIdentifier, It.IsAny<object[]>()), Times.Never);
	}

	[Fact]
	public void NavigateWhenLinkClicked()
	{
		const string value =
			"""
			[東京](#tokyo)
			[札幌](#sapporo)
			# Tokyo
			## Sapporo
			""";

		using var fixture = CreateFixture(value);

		MockJsRuntime
			.Verify(x => x.InvokeAsync<object>(MethodIdentifier, It.IsAny<object[]>()), Times.Never);

		// Navigate to Tokyo
		fixture.Find("a[href$='#tokyo']").Click();
		MockNavigationManager.NavigateTo("#tokyo");

		MockJsRuntime
			.Verify(x => x.InvokeAsync<object>(MethodIdentifier, new object?[] { "tokyo", null }), Times.Once);

		// Navigate to Sapporo
		fixture.Find("a[href$='#sapporo']").Click();
		MockNavigationManager.NavigateTo("#sapporo");

		MockJsRuntime
			.Verify(x => x.InvokeAsync<object>(MethodIdentifier, new object?[] { "sapporo", null }), Times.Once);
	}

	[Fact]
	public void OverrideAllLinks()
	{
		const string value =
			"""
			[absolute](https://www.google.co.jp/)
			[relative](/tokyo)
			[id](#edogawa)
			""";

		const string expected =
			"""
			<article id:ignore class='mud-markdown-body'>
				<p class='mud-typography mud-typography-body1'>
					<a rel='noopener noreferrer' href='overriddenhttps://www.google.co.jp/' target='_blank' class='mud-typography mud-link mud-primary-text mud-link-underline-hover mud-typography-body1'>
						absolute
					</a>
					<br />
					<a href='overridden/tokyo' class='mud-typography mud-link mud-primary-text mud-link-underline-hover mud-typography-body1'>
						relative
					</a>
					<br />
					<a href='overridden#edogawa' class='mud-typography mud-link mud-primary-text mud-link-underline-hover mud-typography-body1'>
						id
					</a>
				</p>
			</article>
			""";

		var @override = Optional<Func<LinkInline, string?>?>.Of(Override);

		using var fixture = CreateFixture(value, overrideLinkUrl: @override);
		fixture.MarkupMatches(expected);
		return;

		static string Override(LinkInline x) =>
			"overridden" + x.Url;
	}

	[Fact]
	public void OverrideImageLink()
	{
		const string value = @"![img](/tokyo/sky-tree.png)";
		const string expected =
			"""
			<article id:ignore class='mud-markdown-body'>
				<p class='mud-typography mud-typography-body1'>
					<img src='overridden/tokyo/sky-tree.png' alt='img' class='mud-image object-fill object-center mud-elevation-25 rounded-lg'>
				</p>
			</article>
			""";

		var @override = Optional<Func<LinkInline, string?>?>.Of(Override);

		using var fixture = CreateFixture(value, overrideLinkUrl: @override);
		fixture.MarkupMatches(expected);
		return;

		static string Override(LinkInline x) =>
			"overridden" + x.Url;
	}
}
