using Bunit;
using Moq;
using Xunit;

namespace MudBlazor.Markdown.Tests
{
	public sealed class MudMarkdownTestsLinkNav : MudMarkdownTestsBase
	{
		private const string MethodIdentifier = "scrollToElementId";

		[Fact]
		public void InvokeNavigationIfHasId()
		{
			Uri = "#tokyo";
			const string value = "## some text";

			using (CreateFixture(value)) { }

			MockJsRuntime
				.Verify(x => x.InvokeAsync<object>(MethodIdentifier, new object[] { "tokyo" }), Times.Once);
		}

		[Fact]
		public void NotInvokeNavigationIfNoId()
		{
			Uri = "/tokyo";
			const string value = "## some text";

			using (CreateFixture(value)) { }

			MockJsRuntime
				.Verify(x => x.InvokeAsync<object>(MethodIdentifier, It.IsAny<object[]>()), Times.Never);
		}

		[Fact]
		public void NavigateWhenLinkClicked()
		{
			const string value =
@"[東京](#tokyo)
# Tokyo";

			using var fixture = CreateFixture(value);

			MockJsRuntime
				.Verify(x => x.InvokeAsync<object>(MethodIdentifier, It.IsAny<object[]>()), Times.Never);

			fixture.Find("a[href$='#tokyo']").Click();

			MockJsRuntime
				.Verify(x => x.InvokeAsync<object>(MethodIdentifier, new object[] { "tokyo" }), Times.Once);
		}
	}
}
