using System.Reflection;
using Markdig;

namespace MudBlazor.Markdown.Tests.Components.MarkdownComponentTests;

public abstract class MarkdownComponentTestsBase : ComponentTestsBase
{
	protected string Uri { get; set; } = string.Empty;

	protected IRenderedComponent<MudMarkdown> CreateFixture(
		string? value,
		Optional<MudMarkdownProps> props = default, Optional<MudMarkdownStyling> styling = default,
		Optional<MarkdownPipeline?> markdownPipeline = default, Optional<MarkdownSourceType> sourceType = default,
		Optional<bool> hasTableOfContents = default, Optional<string?> tableOfContentsHeader = default)
	{
		MockNavigationManager.Initialize(Uri);

		return Ctx.Render<MudMarkdown>(@params =>
			@params.Add(static x => x.Value, value!)
				.TryAdd(static x => x.Props, props)
				.TryAdd(static x => x.Styling, styling)
				.TryAdd(static x => x.MarkdownPipeline, markdownPipeline)
				.TryAdd(static x => x.SourceType, sourceType)
				.TryAdd(static x => x.HasTableOfContents, hasTableOfContents)
				.TryAdd(static x => x.TableOfContentsHeader, tableOfContentsHeader)
		);
	}

	protected static MarkdownPipeline? GetMarkdownPipeline(MudMarkdown fixture)
	{
		const string fieldName = "Pipeline";

		var field = typeof(MudMarkdown)
			.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);

		if (field == null)
			throw new NullReferenceException($"Field `{fieldName}` not found");

		return (MarkdownPipeline?)field.GetValue(fixture);
	}
}
