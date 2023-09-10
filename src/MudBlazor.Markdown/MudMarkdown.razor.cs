﻿using Markdig.Extensions.Mathematics;
using Markdig.Extensions.Tables;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;

namespace MudBlazor;

public class MudMarkdown : ComponentBase, IDisposable
{
	private IMudMarkdownThemeService? _themeService;

	private MarkdownPipeline? _pipeline;
	private bool _enableLinkNavigation;
	private int _elementIndex;

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
	/// Override default styling of the markdown component
	/// </summary>
	[Parameter]
	public MudMarkdownStyling Styling { get; set; } = new();

	[Parameter]
	public MarkdownPipeline? MarkdownPipeline { get; set; }

	[Inject]
	protected NavigationManager? NavigationManager { get; init; }

	[Inject]
	protected IJSRuntime JsRuntime { get; init; } = default!;

	[Inject]
	protected IServiceProvider? ServiceProvider { get; init; }

	public virtual void Dispose()
	{
		if (NavigationManager != null)
			NavigationManager.LocationChanged -= NavigationManagerOnLocationChanged;

		if (_themeService != null)
			_themeService.CodeBlockThemeChanged -= OnCodeBlockThemeChanged;

		GC.SuppressFinalize(this);
	}

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		if (string.IsNullOrEmpty(Value))
			return;

		_elementIndex = 0;

		var pipeline = GetMarkdownPipeLine();
		var parsedText = Markdown.Parse(Value, pipeline);
		if (parsedText.Count == 0)
			return;

		builder.OpenElement(_elementIndex++, "article");
		builder.AddAttribute(_elementIndex++, "class", "mud-markdown-body");
		RenderMarkdown(parsedText, builder);
		builder.CloseElement();
	}

	protected override void OnInitialized()
	{
		base.OnInitialized();

		_themeService = ServiceProvider?.GetService<IMudMarkdownThemeService>();

		if (_themeService != null)
			_themeService.CodeBlockThemeChanged += OnCodeBlockThemeChanged;
	}

	protected override void OnAfterRender(bool firstRender)
	{
		if (!firstRender || !_enableLinkNavigation || NavigationManager == null)
			return;

		var args = new LocationChangedEventArgs(NavigationManager.Uri, true);
		NavigationManagerOnLocationChanged(NavigationManager, args);
		NavigationManager.LocationChanged += NavigationManagerOnLocationChanged;
	}

	protected virtual void RenderMarkdown(ContainerBlock container, RenderTreeBuilder builder)
	{
		for (var i = 0; i < container.Count; i++)
		{
			switch (container[i])
			{
				case ParagraphBlock paragraph:
				{
					RenderParagraphBlock(paragraph, builder);
					break;
				}
				case HeadingBlock heading:
				{
					var typo = (Typo)heading.Level;
					typo = OverrideHeaderTypo?.Invoke(typo) ?? typo;

					_enableLinkNavigation = true;

					var id = heading.BuildIdString();
					RenderParagraphBlock(heading, builder, typo, id);

					break;
				}
				case QuoteBlock quote:
				{
					builder.OpenElement(_elementIndex++, "blockquote");
					RenderMarkdown(quote, builder);
					builder.CloseElement();
					break;
				}
				case Table table:
				{
					RenderTable(table, builder);
					break;
				}
				case ListBlock list:
				{
					RenderList(list, builder);
					break;
				}
				case ThematicBreakBlock:
				{
					builder.OpenComponent<MudDivider>(_elementIndex++);
					builder.CloseComponent();
					break;
				}
				case FencedCodeBlock code:
				{
					var text = code.CreateCodeBlockText();

					builder.OpenComponent<MudCodeHighlight>(_elementIndex++);
					builder.AddAttribute(_elementIndex++, nameof(MudCodeHighlight.Text), text);
					builder.AddAttribute(_elementIndex++, nameof(MudCodeHighlight.Language), code.Info ?? string.Empty);
					builder.AddAttribute(_elementIndex++, nameof(MudCodeHighlight.Theme), CodeBlockTheme);
					builder.CloseComponent();

					break;
				}
				case HtmlBlock html:
				{
					if (html.TryGetDetails(out var detailsData))
						RenderDetailsHtml(builder, detailsData.Header, detailsData.Content);
					else
						RenderHtml(builder, html.Lines);

					break;
				}
				default:
					// TODO: add a method
					break;
			}
		}
	}

	protected virtual void RenderParagraphBlock(LeafBlock paragraph, RenderTreeBuilder builder, Typo typo = Typo.body1, string? id = null)
	{
		if (paragraph.Inline == null)
			return;

		builder.OpenComponent<MudText>(_elementIndex++);

		if (!string.IsNullOrEmpty(id))
			builder.AddAttribute(_elementIndex++, "id", id);

		builder.AddAttribute(_elementIndex++, nameof(MudText.Typo), typo);
		builder.AddAttribute(_elementIndex++, nameof(MudText.ChildContent), (RenderFragment)(contentBuilder => RenderInlines(paragraph.Inline, contentBuilder)));
		builder.CloseComponent();
	}

	protected virtual void RenderInlines(ContainerInline inlines, RenderTreeBuilder builder)
	{
		foreach (var inline in inlines)
		{
			switch (inline)
			{
				case LiteralInline x:
				{
					builder.AddContent(_elementIndex++, x.Content);
					break;
				}
				case HtmlInline x:
				{
					builder.AddMarkupContent(_elementIndex++, x.Tag);
					break;
				}
				case LineBreakInline:
				{
					builder.OpenElement(_elementIndex++, "br");
					builder.CloseElement();
					break;
				}
				case CodeInline x:
				{
					builder.OpenElement(_elementIndex++, "code");
					builder.AddContent(_elementIndex++, x.Content);
					builder.CloseElement();
					break;
				}
				case EmphasisInline x:
				{
					if (!x.TryGetEmphasisElement(out var elementName))
						continue;

					builder.OpenElement(_elementIndex++, elementName);
					RenderInlines(x, builder);
					builder.CloseElement();
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

						builder.OpenComponent<MudImage>(_elementIndex++);
						builder.AddAttribute(_elementIndex++, nameof(MudImage.Class), "rounded-lg");
						builder.AddAttribute(_elementIndex++, nameof(MudImage.Src), url);
						builder.AddAttribute(_elementIndex++, nameof(MudImage.Alt), string.Join(null, alt));
						builder.AddAttribute(_elementIndex++, nameof(MudImage.Elevation), 25);
						builder.CloseComponent();
					}
					else if (LinkCommand == null)
					{
						builder.OpenComponent<MudLink>(_elementIndex++);
						builder.AddAttribute(_elementIndex++, nameof(MudLink.Href), url);
						builder.AddAttribute(_elementIndex++, nameof(MudLink.ChildContent), (RenderFragment)(linkBuilder => RenderInlines(x, linkBuilder)));

						if (url.IsExternalUri(NavigationManager?.BaseUri))
						{
							builder.AddAttribute(_elementIndex++, nameof(MudLink.Target), "_blank");
							builder.AddAttribute(_elementIndex++, "rel", "noopener noreferrer");
						}
						// (prevent scrolling to the top of the page)
						// custom implementation only for links on the same page
						else if (url?.StartsWith('#') ?? false)
						{
							builder.AddEventPreventDefaultAttribute(_elementIndex++, "onclick", true);
							builder.AddAttribute(_elementIndex++, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, () =>
							{
								if (NavigationManager == null)
									return;

								var uriBuilder = new UriBuilder(NavigationManager.Uri)
								{
									Fragment = url
								};
								var args = new LocationChangedEventArgs(uriBuilder.Uri.AbsoluteUri, true);
								NavigationManagerOnLocationChanged(NavigationManager, args);
							}));
						}

						builder.CloseComponent();
					}
					else
					{
						builder.OpenComponent<MudLinkButton>(_elementIndex++);
						builder.AddAttribute(_elementIndex++, nameof(MudLinkButton.Command), LinkCommand);
						builder.AddAttribute(_elementIndex++, nameof(MudLinkButton.CommandParameter), url);
						builder.AddAttribute(_elementIndex++, nameof(MudLinkButton.ChildContent), (RenderFragment)(linkBuilder => RenderInlines(x, linkBuilder)));
						builder.CloseComponent();
					}

					break;
				}
				case MathInline x:
				{
					builder.OpenComponent<MudMathJax>(_elementIndex++);
					builder.AddAttribute(_elementIndex++, nameof(MudMathJax.Delimiter), x.GetDelimiter());
					builder.AddAttribute(_elementIndex++, nameof(MudMathJax.Value), x.Content);
					builder.CloseComponent();
					break;
				}
				default:
					// TODO: add a method
					break;
			}
		}
	}

	protected virtual void RenderTable(Table table, RenderTreeBuilder builder)
	{
		// First child is columns
		if (table.Count < 2)
			return;

		builder.OpenComponent<MudSimpleTable>(_elementIndex++);
		builder.AddAttribute(_elementIndex++, nameof(MudSimpleTable.Style), "overflow-x: auto;");
		builder.AddAttribute(_elementIndex++, nameof(MudSimpleTable.Striped), Styling.Table.IsStriped);
		builder.AddAttribute(_elementIndex++, nameof(MudSimpleTable.Bordered), Styling.Table.IsBordered);
		builder.AddAttribute(_elementIndex++, nameof(MudSimpleTable.Elevation), Styling.Table.Elevation);
		builder.AddAttribute(_elementIndex++, nameof(MudSimpleTable.ChildContent), (RenderFragment)(contentBuilder =>
		{
			// thread
			contentBuilder.OpenElement(_elementIndex++, "thead");
			RenderTableRow((TableRow)table[0], "th", contentBuilder, TableCellMinWidth);
			contentBuilder.CloseElement();

			// tbody
			contentBuilder.OpenElement(_elementIndex++, "tbody");
			for (var j = 1; j < table.Count; j++)
			{
				RenderTableRow((TableRow)table[j], "td", contentBuilder);
			}

			contentBuilder.CloseElement();
		}));
		builder.CloseComponent();
	}

	protected virtual void RenderTableRow(TableRow row, string cellElementName, RenderTreeBuilder builder, int? minWidth = null)
	{
		builder.OpenElement(_elementIndex++, "tr");

		for (var j = 0; j < row.Count; j++)
		{
			var cell = (TableCell)row[j];
			builder.OpenElement(_elementIndex++, cellElementName);

			if (minWidth is > 0)
				builder.AddAttribute(_elementIndex++, "style", $"min-width:{minWidth}px");

			if (cell.Count != 0 && cell[0] is ParagraphBlock paragraphBlock)
				RenderParagraphBlock(paragraphBlock, builder);

			builder.CloseElement();
		}

		builder.CloseElement();
	}

	protected virtual void RenderList(ListBlock list, RenderTreeBuilder builder)
	{
		if (list.Count == 0)
			return;

		var elementName = list.IsOrdered ? "ol" : "ul";
		var orderStart = list.OrderedStart.ParseOrDefault();

		builder.OpenElement(_elementIndex++, elementName);

		if (orderStart > 1)
		{
			builder.AddAttribute(_elementIndex++, "start", orderStart);
		}

		for (var i = 0; i < list.Count; i++)
		{
			var block = (ListItemBlock)list[i];

			for (var j = 0; j < block.Count; j++)
			{
				switch (block[j])
				{
					case ListBlock x:
						RenderList(x, builder);
						break;
					case ParagraphBlock x:
						builder.OpenElement(_elementIndex++, "li");
						RenderParagraphBlock(x, builder);
						builder.CloseElement();
						break;
					default:
						// TODO: add a method
						break;
				}
			}
		}

		builder.CloseElement();
	}

	protected virtual void RenderDetailsHtml(in RenderTreeBuilder builder, in string header, in string content)
	{
		var headerHtml = Markdown.Parse(header, _pipeline);
		var contentHtml = Markdown.Parse(content);

		builder.OpenComponent<MudMarkdownDetails>(_elementIndex++);
		builder.AddAttribute(_elementIndex++, nameof(MudMarkdownDetails.TitleContent), (RenderFragment)(titleBuilder => RenderMarkdown(headerHtml, titleBuilder)));
		builder.AddAttribute(_elementIndex++, nameof(MudMarkdownDetails.ChildContent), (RenderFragment)(contentBuilder => RenderMarkdown(contentHtml, contentBuilder)));
		builder.CloseComponent();
	}

	protected virtual void RenderHtml(in RenderTreeBuilder builder, in StringLineGroup lines)
	{
		var markupString = new MarkupString(lines.ToString());
		builder.AddContent(_elementIndex, markupString);
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

		return _pipeline ??= new MarkdownPipelineBuilder()
			.UseAdvancedExtensions()
			.Build();
	}
}
