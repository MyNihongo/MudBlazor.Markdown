using Markdig.Syntax.Inlines;
using MyNihongo.Option;

namespace MudBlazor.Markdown.Tests.MarkdownComponentTests;

public abstract class MarkdownComponentTestsBase : ComponentTestsBase
{
	protected string Uri { get; set; } = string.Empty;

	protected IRenderedComponent<MudMarkdown> CreateFixture(
		string value,
		Optional<ICommand?> command = default, Optional<int?> tableCellMinWidth = default,
		Optional<Func<LinkInline, string?>?> overrideLinkUrl = default, Optional<Func<Typo, Typo>?> overrideHeaderTypo = default)
	{
		MockNavigationManager.Initialize(Uri);

		return Ctx.RenderComponent<MudMarkdown>(@params =>
			@params.Add(static x => x.Value, value)
				.TryAdd(static x => x.LinkCommand, command)
				.TryAdd(static x => x.TableCellMinWidth, tableCellMinWidth)
				.TryAdd(static x => x.OverrideLinkUrl, overrideLinkUrl)
				.TryAdd(static x => x.OverrideHeaderTypo, overrideHeaderTypo));
	}
}