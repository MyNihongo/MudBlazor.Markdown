using System.Reflection;
using Markdig;
using Markdig.Syntax.Inlines;
using MyNihongo.Option;

namespace MudBlazor.Markdown.Tests.MarkdownComponentTests;

public abstract class MarkdownComponentTestsBase : ComponentTestsBase
{
	protected string Uri { get; set; } = string.Empty;

	protected IRenderedComponent<MudMarkdown> CreateFixture(
		string? value,
		Optional<ICommand?> command = default, Optional<int?> tableCellMinWidth = default,
		Optional<Func<LinkInline, string?>?> overrideLinkUrl = default, Optional<Func<Typo, Typo>?> overrideHeaderTypo = default,
        Optional<Typo?> paragraphTypo = default, Optional<Color?> textColor = default,
		Optional<MudMarkdownStyling> styling = default, Optional<MarkdownPipeline?> markdownPipeline = default)
	{
		MockNavigationManager.Initialize(Uri);

		return Ctx.RenderComponent<MudMarkdown>(@params =>
			@params.Add(static x => x.Value, value!)
				.TryAdd(static x => x.LinkCommand, command)
				.TryAdd(static x => x.TableCellMinWidth, tableCellMinWidth)
				.TryAdd(static x => x.OverrideLinkUrl, overrideLinkUrl)
				.TryAdd(static x => x.OverrideHeaderTypo, overrideHeaderTypo)
                .TryAdd(static x => x.ParagraphTypo, paragraphTypo)
                .TryAdd(static x => x.TextColor, textColor)
				.TryAdd(static x => x.Styling, styling)
				.TryAdd(static x => x.MarkdownPipeline, markdownPipeline));
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
