using Bunit;

namespace MudBlazor.Markdown.Tests.UrlOverrideTests
{
	public abstract class UrlOverrideTestsBase : ComponentTestsBase
	{
		protected IRenderedComponent<MudMarkdown> CreateFixture(string value)
		{
			MockNavigationManager.Initialize("tokyo");

			return Ctx.RenderComponent<UrlOverrideComponent>(@params =>
				@params.Add(static x => x.Value, value));
		}

		private sealed class UrlOverrideComponent : MudMarkdown
		{
			protected override string OverrideLinkInlineUrl(string originalUrl) =>
				"overridden" + originalUrl;
		}
	}
}
