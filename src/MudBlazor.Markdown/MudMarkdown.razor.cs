using Markdig.Extensions.Mathematics;
using Markdig.Extensions.Tables;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;

// ReSharper disable MemberCanBePrivate.Global

namespace MudBlazor;

public class MudMarkdown : ComponentBase, IDisposable
{
	protected IMudMarkdownThemeService? ThemeService;

	protected MarkdownPipeline? Pipeline;
	protected bool EnableLinkNavigation;

	/// <summary>
	/// Markdown text to be rendered in the component.
	/// </summary>
	[Parameter]
	public string Value { get; set; } = string.Empty;

	/// <summary>
	/// Minimum width (in pixels) for a table cell.<br/>
	/// If <see langword="null" /> or negative the minimum width is not applied.
	/// </summary>
	[Parameter]
	public int? TableCellMinWidth { get; set; }

	/// <summary>
	/// Command which is invoked when a link is clicked.<br/>
	/// If <see langword="null" /> a link is opened in the browser.
	/// </summary>
	[Parameter]
	public ICommand? LinkCommand { get; set; }

	/// <summary>
	/// Theme of the code block.<br/>
	/// Browse available themes here: https://highlightjs.org/static/demo/
	/// </summary>
	[Parameter]
	public CodeBlockTheme CodeBlockTheme { get; set; }

	/// <summary>
	/// Override the original URL address of the <see cref="LinkInline"/>.<br/>
	/// If a function is not provided <see cref="LinkInline.Url"/> is used
	/// </summary>
	[Parameter]
	public Func<LinkInline, string?>? OverrideLinkUrl { get; set; }

	/// <summary>
	/// Typography variant to use for Heading Level 1-6.<br/>
	/// If a function is not provided a default typo for each level is set (e.g. for &lt;h1&gt; it will be <see cref="Typo.h1"/>, etc.)
	/// </summary>
	[Parameter]
	public Func<Typo, Typo>? OverrideHeaderTypo { get; set; }

	/// <summary>
    /// Typography variant to use for paragrapgh text.<br/>
    /// If a function is not provided it will use Typo.body1
    /// </summary>
    [Parameter]
    public Typo? ParagraphTypo { get; set; }

    /// <summary>
    /// Color to use for all text.<br/>
    /// If a color is not provided it will use the default.
    /// </summary>
    [Parameter]
    public Color? TextColor { get; set; }

    /// <summary>
	/// Override default styling of the markdown component
	/// </summary>
	[Parameter]
	public MudMarkdownStyling Styling { get; set; } = new();

	/// <summary>
	/// Custom Markdown pipeline to use for rendering.<br/>
	/// If not provided, a default pipeline with advanced extensions will be used.
	/// </summary>
	[Parameter]
	public MarkdownPipeline? MarkdownPipeline { get; set; }

	/// <summary>
	/// The type of source for the markdown content.<br/>
	/// Default value: <see cref="MarkdownSourceType.RawValue"/>
	/// </summary>
	[Parameter]
	public MarkdownSourceType SourceType { get; set; } = MarkdownSourceType.RawValue;

	[Inject]
	protected NavigationManager? NavigationManager { get; init; }

	[Inject]
	protected IJSRuntime JsRuntime { get; init; } = null!;

	[Inject]
	protected IServiceProvider? ServiceProvider { get; init; }

	[Inject]
	private IMudMarkdownValueProvider MudMarkdownValueProvider { get; init; } = null!;

	public virtual void Dispose()
	{
		if (NavigationManager != null)
			NavigationManager.LocationChanged -= NavigationManagerOnLocationChanged;

		if (ThemeService != null)
			ThemeService.CodeBlockThemeChanged -= OnCodeBlockThemeChanged;

		GC.SuppressFinalize(this);
	}

	public override async Task SetParametersAsync(ParameterView parameters)
	{
		if (parameters.TryGetValue<string>(nameof(Value), out var value) && !ReferenceEquals(Value, value))
		{
			var sourceType = parameters.GetValueOrDefault<MarkdownSourceType>(nameof(SourceType));
			var dictionary = parameters.ToMutableDictionary(); // must be before the `await` call

			Value = await MudMarkdownValueProvider.GetValueAsync(value, sourceType);

			dictionary[nameof(Value)] = Value;
			parameters = ParameterView.FromDictionary(dictionary);
		}

		await base.SetParametersAsync(parameters)
			.ConfigureAwait(false);
	}

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		if (string.IsNullOrEmpty(Value))
			return;

		var pipeline = GetMarkdownPipeLine();
		var parsedText = Markdown.Parse(Value, pipeline);
		if (parsedText.Count == 0)
			return;

		var elementIndex = 0;

		builder.OpenElement(elementIndex++, "article");
		builder.AddAttribute(elementIndex++, AttributeNames.Class, "mud-markdown-body");
		RenderMarkdown(builder, ref elementIndex, parsedText);
		builder.CloseElement();
	}

	protected override void OnInitialized()
	{
		base.OnInitialized();

		ThemeService = ServiceProvider?.GetService<IMudMarkdownThemeService>();

		if (ThemeService != null)
			ThemeService.CodeBlockThemeChanged += OnCodeBlockThemeChanged;
	}

	protected override void OnAfterRender(bool firstRender)
	{
		if (!firstRender || !EnableLinkNavigation || NavigationManager == null)
			return;

		var args = new LocationChangedEventArgs(NavigationManager.Uri, true);
		NavigationManagerOnLocationChanged(NavigationManager, args);
		NavigationManager.LocationChanged += NavigationManagerOnLocationChanged;
	}

	protected virtual void RenderMarkdown(RenderTreeBuilder builder, ref int elementIndex, ContainerBlock container)
	{
		for (var i = 0; i < container.Count; i++)
		{
			bool addBottomMargin = i != (container.Count - 1);
            switch (container[i])
			{
				case ParagraphBlock paragraph:
				{
					RenderParagraphBlock(builder, ref elementIndex, paragraph, ParagraphTypo ?? Typo.body1, addBottomMargin: addBottomMargin);
					break;
				}
				case HeadingBlock heading:
				{
					var typo = (Typo)heading.Level;
					typo = OverrideHeaderTypo?.Invoke(typo) ?? typo;

					EnableLinkNavigation = true;

					var id = heading.BuildIdString();
					RenderParagraphBlock(builder, ref elementIndex, heading, typo, id, addBottomMargin: addBottomMargin);

					break;
				}
				case QuoteBlock quote:
				{
					builder.OpenElement(elementIndex++, "blockquote");
					RenderMarkdown(builder, ref elementIndex, quote);
					builder.CloseElement();
					break;
				}
				case Table table:
				{
					RenderTable(builder, ref elementIndex, table);
					break;
				}
				case ListBlock list:
				{
					RenderList(builder, ref elementIndex, list, addBottomMargin);
					break;
				}
				case ThematicBreakBlock:
				{
					builder.OpenComponent<MudDivider>(elementIndex++);
					builder.CloseComponent();
					break;
				}
				case FencedCodeBlock code:
				{
					RenderCodeBlock(builder, ref elementIndex, code, code.Info);
					break;
				}
				case CodeBlock code:
				{
					RenderCodeBlock(builder, ref elementIndex, code, info: null);
					break;
				}
				case HtmlBlock html:
				{
					if (html.TryGetDetails(out var detailsData))
						RenderDetailsHtml(builder, ref elementIndex, detailsData.Header, detailsData.Content);
					else
						RenderHtml(builder, ref elementIndex, html.Lines);

					break;
				}
				default:
				{
					OnRenderMarkdownBlockDefault(builder, ref elementIndex, container[i]);
					break;
				}
			}
		}
	}

	/// <summary>
	/// Renders a markdown block which is not covered by the switch-case block in <see cref="RenderMarkdown"/> 
	/// </summary>
	protected virtual void OnRenderMarkdownBlockDefault(RenderTreeBuilder builder, ref int elementIndex, Markdig.Syntax.Block block)
	{
	}

	protected virtual void RenderParagraphBlock(RenderTreeBuilder builder1, ref int elementIndex1, LeafBlock paragraph, Typo typo = Typo.body1, string? id = null, bool addBottomMargin = true)
	{
		if (paragraph.Inline == null)
			return;

		builder1.OpenComponent<MudText>(elementIndex1++);

		if (!string.IsNullOrEmpty(id))
			builder1.AddAttribute(elementIndex1++, AttributeNames.Id, id);

		builder1.AddComponentParameter(elementIndex1++, nameof(MudText.Typo), typo);
		if (TextColor != null)
		{
			builder1.AddComponentParameter(elementIndex1++, nameof(MudText.Color), TextColor);
		}
		builder1.AddComponentParameter(elementIndex1++, nameof(MudText.ChildContent), (RenderFragment)(builder2 =>
		{
			var elementIndex2 = 0;
			RenderInlines(builder2, ref elementIndex2, paragraph.Inline);
		}));
		builder1.CloseComponent();
		if (addBottomMargin)
		{
			builder.OpenElement(ElementIndex++, "br");
			builder.CloseElement();
		}
    }

	protected virtual void RenderInlines(RenderTreeBuilder builder1, ref int elementIndex1, ContainerInline inlines)
	{
		foreach (var inline in inlines)
		{
			switch (inline)
			{
				case LiteralInline x:
				{
					builder1.AddContent(elementIndex1++, x.Content);
					break;
				}
				case HtmlInline x:
				{
					builder1.AddMarkupContent(elementIndex1++, x.Tag);
					break;
				}
				case LineBreakInline:
				{
					builder1.OpenElement(elementIndex1++, "br");
					builder1.CloseElement();
					break;
				}
				case CodeInline x:
				{
					builder1.OpenElement(elementIndex1++, "code");
					builder1.AddContent(elementIndex1++, x.Content);
					builder1.CloseElement();
					break;
				}
				case EmphasisInline x:
				{
					if (!x.TryGetEmphasisElement(out var elementName))
					{
						var markdownValue = x.Span.TryGetText(Value);
						TryRenderMarkdownError(builder1, ref elementIndex1, markdownValue, ElementNames.Span);
						continue;
					}

					builder1.OpenElement(elementIndex1++, elementName);
					RenderInlines(builder1, ref elementIndex1, x);
					builder1.CloseElement();
					break;
				}
				case LinkInline x:
				{
					var url = OverrideLinkUrl?.Invoke(x) ?? x.Url;

					if (x.IsImage)
					{
						var alt = x
							.OfType<LiteralInline>()
							.Select(static x => x.Content);

						builder1.OpenComponent<MudImage>(elementIndex1++);
						builder1.AddComponentParameter(elementIndex1++, nameof(MudImage.Class), "rounded-lg");
						builder1.AddComponentParameter(elementIndex1++, nameof(MudImage.Src), url);
						builder1.AddComponentParameter(elementIndex1++, nameof(MudImage.Alt), string.Join(null, alt));
						builder1.AddComponentParameter(elementIndex1++, nameof(MudImage.Elevation), 25);
						builder1.CloseComponent();
					}
					else if (LinkCommand == null)
					{
						builder1.OpenComponent<MudLink>(elementIndex1++);
						builder1.AddComponentParameter(elementIndex1++, nameof(MudLink.Typo), ParagraphTypo ?? Typo.body1);
						builder1.AddComponentParameter(elementIndex1++, nameof(MudLink.Href), url);
						builder1.AddComponentParameter(elementIndex1++, nameof(MudLink.Underline), Styling.Link.Underline);
						builder1.AddComponentParameter(elementIndex1++, nameof(MudLink.ChildContent), (RenderFragment)(builder2 =>
						{
							var  elementIndex2 = 0;
							RenderInlines(builder2, ref elementIndex2, x);
						}));

						if (url.IsExternalUri(NavigationManager?.BaseUri))
						{
							builder1.AddComponentParameter(elementIndex1++, nameof(MudLink.Target), "_blank");
							builder1.AddAttribute(elementIndex1++, AttributeNames.LinkRelation, "noopener noreferrer");
						}
						// (prevent scrolling to the top of the page)
						// custom implementation only for links on the same page
						else if (url?.StartsWith('#') ?? false)
						{
							builder1.AddEventPreventDefaultAttribute(elementIndex1++, AttributeNames.OnClick, true);
							builder1.AddAttribute(elementIndex1++, AttributeNames.OnClick, EventCallback.Factory.Create<MouseEventArgs>(this, () =>
							{
								if (NavigationManager == null)
									return;

								var uriBuilder = new UriBuilder(NavigationManager.Uri)
								{
									Fragment = url,
								};
								var args = new LocationChangedEventArgs(uriBuilder.Uri.AbsoluteUri, true);
								NavigationManagerOnLocationChanged(NavigationManager, args);
							}));
						}

						builder1.CloseComponent();
					}
					else
					{
						builder1.OpenComponent<MudLinkButton>(elementIndex1++);
						builder1.AddComponentParameter(elementIndex1++, nameof(MudLinkButton.Typo), ParagraphTypo ?? Typo.body1);
						builder1.AddComponentParameter(elementIndex1++, nameof(MudLinkButton.Command), LinkCommand);
						builder1.AddComponentParameter(elementIndex1++, nameof(MudLinkButton.CommandParameter), url);
						builder1.AddComponentParameter(elementIndex1++, nameof(MudLinkButton.ChildContent), (RenderFragment)(builder2 =>
						{
							var elementIndex2 = 0;
							RenderInlines(builder2, ref elementIndex2, x);
						}));
						builder1.CloseComponent();
					}

					break;
				}
				case MathInline x:
				{
					builder1.OpenComponent<MudMathJax>(elementIndex1++);
					builder1.AddComponentParameter(elementIndex1++, nameof(MudMathJax.Delimiter), x.GetDelimiter());
					builder1.AddComponentParameter(elementIndex1++, nameof(MudMathJax.Value), x.Content);
					builder1.CloseComponent();
					break;
				}
				case PipeTableDelimiterInline x:
				{
					// It usually indicates that there are some issues with table markdown
					var markdownValue = x.Parent?.ParentBlock?.Span.TryGetText(Value);
					TryRenderMarkdownError(builder1, ref elementIndex1, markdownValue);

					break;
				}
				default:
				{
					OnRenderInlinesDefault(builder1, ref elementIndex1, inline);
					break;
				}
			}
		}
	}

	protected virtual void TryRenderMarkdownError(RenderTreeBuilder builder, ref int elementIndex, string? text, string htmlElement = "div")
	{
		if (string.IsNullOrEmpty(text))
			return;

		builder.OpenElement(elementIndex++, htmlElement);
		builder.AddAttribute(elementIndex++, AttributeNames.Class, "mud-markdown-error");
		builder.AddContent(elementIndex++, text);
		builder.CloseElement();
	}

	/// <summary>
	/// Renders inline block which is not covered by the switch-case block in <see cref="RenderInlines"/> 
	/// </summary>
	protected virtual void OnRenderInlinesDefault(RenderTreeBuilder builder, ref int elementIndex, Inline inline)
	{
	}

	protected virtual void RenderTable(RenderTreeBuilder builder1, ref int elementIndex1, Table table)
	{
		// First child is columns
		if (table.Count < 2)
			return;

		builder1.OpenComponent<MudSimpleTable>(elementIndex1++);
		builder1.AddComponentParameter(elementIndex1++, nameof(MudSimpleTable.Style), "overflow-x: auto;");
		builder1.AddComponentParameter(elementIndex1++, nameof(MudSimpleTable.Striped), Styling.Table.IsStriped);
		builder1.AddComponentParameter(elementIndex1++, nameof(MudSimpleTable.Bordered), Styling.Table.IsBordered);
		builder1.AddComponentParameter(elementIndex1++, nameof(MudSimpleTable.Elevation), Styling.Table.Elevation);
		builder1.AddComponentParameter(elementIndex1++, nameof(MudSimpleTable.ChildContent), (RenderFragment)(builder2 =>
		{
			var elementIndex2 = 0;
			
			// thead
			builder2.OpenElement(elementIndex2++, "thead");
			RenderTableRow(builder2, ref elementIndex2, (TableRow)table[0], "th", TableCellMinWidth);
			builder2.CloseElement();

			// tbody
			builder2.OpenElement(elementIndex2++, "tbody");
			for (var j = 1; j < table.Count; j++)
			{
				RenderTableRow(builder2, ref elementIndex2, (TableRow)table[j], "td");
			}

			builder2.CloseElement();
		}));
		builder1.CloseComponent();
	}

	protected virtual void RenderTableRow(RenderTreeBuilder builder, ref int elementIndex, TableRow row, string cellElementName, int? minWidth = null)
	{
		builder.OpenElement(elementIndex++, "tr");

		for (var j = 0; j < row.Count; j++)
		{
			var cell = (TableCell)row[j];
			builder.OpenElement(elementIndex++, cellElementName);

			if (minWidth is > 0)
				builder.AddAttribute(elementIndex++, AttributeNames.Style, $"min-width:{minWidth}px");

			if (cell.Count != 0 && cell[0] is ParagraphBlock paragraphBlock)
				RenderParagraphBlock(builder, ref elementIndex, paragraphBlock, ParagraphTypo ?? Typo.body1);

			builder.CloseElement();
		}

		builder.CloseElement();
	}

	protected virtual void RenderList(RenderTreeBuilder builder, ref int elementIndex, ListBlock list, bool addBottomMargin = true)
	{
		if (list.Count == 0)
			return;

		var elementName = list.IsOrdered ? "ol" : "ul";
		var orderStart = list.OrderedStart.ParseOrDefault();

		builder.OpenElement(elementIndex++, elementName);

		if (orderStart > 1)
		{
			builder.AddAttribute(elementIndex++, AttributeNames.Start, orderStart);
		}

		for (var i = 0; i < list.Count; i++)
		{
			var block = (ListItemBlock)list[i];
			builder.OpenElement(elementIndex++, "li");

			for (var j = 0; j < block.Count; j++)
			{
				switch (block[j])
				{
					case ListBlock x:
					{
						RenderList(builder, ref elementIndex, x);
						break;
					}
					case ParagraphBlock x:
					{
						if (TextColor != null)
						{
							string color = TextColor.Value.ToString().ToLower();
							builder.AddAttribute(elementIndex++, "class", $"mud-{color}-text");
						}
						RenderParagraphBlock(builder, ref elementIndex, x, ParagraphTypo ?? Typo.body1, addBottomMargin: false);
						break;
					}
					case FencedCodeBlock x:
					{
						RenderCodeBlock(builder, ref elementIndex, x, x.Info);
						break;
					}
					case CodeBlock x:
					{
						RenderCodeBlock(builder, ref elementIndex, x, info: null);
						break;
					}
					default:
					{
						OnRenderListDefault(builder, ref elementIndex, block[j]);
						break;
					}
				}
			}

			// Close </li> 
			builder.CloseElement();
		}

		builder.CloseElement();
        if (addBottomMargin)
        {
            builder.OpenElement(ElementIndex++, "br");
            builder.CloseElement();
        }
    }

	/// <summary>
	/// Renders a markdown block which is not covered by the switch-case block in <see cref="RenderList"/> 
	/// </summary>
	protected virtual void OnRenderListDefault(RenderTreeBuilder builder, ref int elementIndex, Markdig.Syntax.Block block)
	{
	}

	protected virtual void RenderDetailsHtml(in RenderTreeBuilder builder1, ref int elementIndex1, in string header, in string content)
	{
		var headerHtml = Markdown.Parse(header, Pipeline);
		var contentHtml = Markdown.Parse(content);

		builder1.OpenComponent<MudMarkdownDetails>(elementIndex1++);
		builder1.AddComponentParameter(elementIndex1++, nameof(MudMarkdownDetails.TitleContent), (RenderFragment)(builder2 =>
		{
			var elementIndex2 = 0;
			RenderMarkdown(builder2, ref elementIndex2, headerHtml);
		}));
		builder1.AddComponentParameter(elementIndex1++, nameof(MudMarkdownDetails.ChildContent), (RenderFragment)(builder2 =>
		{
			var elementIndex2 = 0;
			RenderMarkdown(builder2, ref elementIndex2, contentHtml);
		}));
		builder1.CloseComponent();
	}

	protected virtual void RenderHtml(in RenderTreeBuilder builder, ref int elementIndex, in StringLineGroup lines)
	{
		var markupString = new MarkupString(lines.ToString());
		builder.AddContent(elementIndex, markupString);
	}

	protected virtual void RenderCodeBlock(in RenderTreeBuilder builder, ref int elementIndex, in CodeBlock code, in string? info)
	{
		var text = code.CreateCodeBlockText();

		builder.OpenComponent<MudCodeHighlight>(elementIndex++);
		builder.AddComponentParameter(elementIndex++, nameof(MudCodeHighlight.Text), text);
		builder.AddComponentParameter(elementIndex++, nameof(MudCodeHighlight.Language), info ?? string.Empty);
		builder.AddComponentParameter(elementIndex++, nameof(MudCodeHighlight.Theme), CodeBlockTheme);
		builder.CloseComponent();
	}

	private async void NavigationManagerOnLocationChanged(object? sender, LocationChangedEventArgs e)
	{
		var idFragment = new Uri(e.Location, UriKind.Absolute).Fragment;
		if (!idFragment.StartsWith('#') || idFragment.Length < 2)
			return;

		idFragment = idFragment[1..];

		await JsRuntime.InvokeVoidAsync("scrollToElementId", idFragment)
			.ConfigureAwait(false);
	}

	private void OnCodeBlockThemeChanged(object? sender, CodeBlockTheme e) =>
		CodeBlockTheme = e;

	private MarkdownPipeline GetMarkdownPipeLine()
	{
		if (MarkdownPipeline != null)
			return MarkdownPipeline;

		return Pipeline ??= new MarkdownPipelineBuilder()
			.UseAdvancedExtensions()
			.Build();
	}
}
