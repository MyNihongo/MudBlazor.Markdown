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
}
