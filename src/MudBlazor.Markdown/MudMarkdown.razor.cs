using System;
using System.Linq;
using System.Windows.Input;
using Markdig;
using Markdig.Extensions.Tables;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("MudBlazor.Markdown.Tests")]
// ReSharper disable once CheckNamespace
namespace MudBlazor
{
	public class MudMarkdown : ComponentBase, IDisposable
	{
		private IMudMarkdownThemeService? _themeService;

		private readonly MarkdownPipeline _pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
		private bool _enableLinkNavigation;
		private int _i;

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
		/// Theme of the code block.
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

		[Inject]
		private NavigationManager? NavigationManager { get; init; }

		[Inject]
		private IJSRuntime? JsRuntime { get; init; }

		[Inject]
		private IServiceProvider? ServiceProvider { get; init; }

		public void Dispose()
		{
			if (NavigationManager != null)
				NavigationManager.LocationChanged -= NavigationManagerOnLocationChanged;

			if (_themeService != null)
				_themeService.CodeBlockThemeChanged -= OnCodeBlockThemeChanged;

			GC.SuppressFinalize(this);
		}

		protected sealed override void BuildRenderTree(RenderTreeBuilder builder)
		{
			if (string.IsNullOrEmpty(Value))
				return;

			_i = 0;

			var parsedText = Markdown.Parse(Value, _pipeline);
			if (parsedText.Count == 0)
				return;

			builder.OpenElement(_i++, "article");
			builder.AddAttribute(_i++, "class", "mud-markdown-body");
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

		private async void NavigationManagerOnLocationChanged(object? sender, LocationChangedEventArgs e)
		{
			if (JsRuntime == null)
				return;

			var idFragment = new Uri(e.Location, UriKind.Absolute).Fragment;
			if (!idFragment.StartsWith('#') || idFragment.Length < 2)
				return;

			idFragment = idFragment[1..];

			await JsRuntime.InvokeVoidAsync("scrollToElementId", idFragment)
				.ConfigureAwait(false);
		}

		private void RenderMarkdown(ContainerBlock container, RenderTreeBuilder builder)
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
							builder.OpenElement(_i++, "blockquote");
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
							builder.OpenComponent<MudDivider>(_i++);
							builder.CloseComponent();
							break;
						}
					case FencedCodeBlock code:
						{
							var text = code.CreateCodeBlockText();

							builder.OpenComponent<MudCodeHighlight>(i++);
							builder.AddAttribute(_i++, nameof(MudCodeHighlight.Text), text);
							builder.AddAttribute(_i++, nameof(MudCodeHighlight.Language), code.Info ?? string.Empty);
							builder.AddAttribute(_i++, nameof(MudCodeHighlight.Theme), CodeBlockTheme);
							builder.CloseComponent();

							break;
						}
				}
			}
		}

		private void RenderParagraphBlock(LeafBlock paragraph, RenderTreeBuilder builder, Typo typo = Typo.body1, string? id = null)
		{
			if (paragraph.Inline == null)
				return;

			builder.OpenComponent<MudText>(_i++);

			if (!string.IsNullOrEmpty(id))
				builder.AddAttribute(_i++, "id", id);

			builder.AddAttribute(_i++, nameof(MudText.Typo), typo);
			builder.AddAttribute(_i++, nameof(MudText.ChildContent), (RenderFragment)(contentBuilder => RenderInlines(paragraph.Inline, contentBuilder)));
			builder.CloseComponent();
		}

		private void RenderInlines(ContainerInline inlines, RenderTreeBuilder builder)
		{
			foreach (var inline in inlines)
			{
				switch (inline)
				{
					case LiteralInline x:
						{
							builder.AddContent(_i++, x.Content);
							break;
						}
					case HtmlInline x:
						{
							builder.AddMarkupContent(_i++, x.Tag);
							break;
						}
					case LineBreakInline:
						{
							builder.OpenElement(_i++, "br");
							builder.CloseElement();
							break;
						}
					case CodeInline x:
						{
							builder.OpenElement(_i++, "code");
							builder.AddContent(_i++, x.Content);
							builder.CloseElement();
							break;
						}
					case EmphasisInline x:
						{
							if (!x.TryGetEmphasisElement(out var elementName))
								continue;

							builder.OpenElement(_i++, elementName);
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

								builder.OpenElement(_i++, "img");
								builder.AddAttribute(_i++, "src", url);
								builder.AddAttribute(_i++, "alt", string.Join(null, alt));
								builder.CloseElement();
							}
							else if (LinkCommand == null)
							{
								builder.OpenComponent<MudLink>(_i++);
								builder.AddAttribute(_i++, nameof(MudLink.Href), url);
								builder.AddAttribute(_i++, nameof(MudLink.ChildContent), (RenderFragment)(linkBuilder => RenderInlines(x, linkBuilder)));

								if (url.IsExternalUri(NavigationManager?.BaseUri))
								{
									builder.AddAttribute(_i++, nameof(MudLink.Target), "_blank");
									builder.AddAttribute(_i++, "rel", "noopener noreferrer");
								}
								// (prevent scrolling to the top of the page)
								// custom implementation only for links on the same page
								else if (url?.StartsWith('#') ?? false)
								{
									builder.AddEventPreventDefaultAttribute(_i++, "onclick", true);
									builder.AddAttribute(_i++, "onclick", EventCallback.Factory.Create(this, () =>
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
								builder.OpenComponent<MudLinkButton>(_i++);
								builder.AddAttribute(_i++, nameof(MudLinkButton.Command), LinkCommand);
								builder.AddAttribute(_i++, nameof(MudLinkButton.CommandParameter), url);
								builder.AddAttribute(_i++, nameof(MudLinkButton.ChildContent), (RenderFragment)(linkBuilder => RenderInlines(x, linkBuilder)));
								builder.CloseComponent();
							}
							break;
						}
				}
			}
		}

		private void RenderTable(Table table, RenderTreeBuilder builder)
		{
			// First child is columns
			if (table.Count < 2)
				return;

			builder.OpenComponent<MudSimpleTable>(_i++);
			builder.AddAttribute(_i++, nameof(MudSimpleTable.Style), "overflow-x: auto;");
			builder.AddAttribute(_i++, nameof(MudSimpleTable.Striped), true);
			builder.AddAttribute(_i++, nameof(MudSimpleTable.Bordered), true);
			builder.AddAttribute(_i++, nameof(MudSimpleTable.ChildContent), (RenderFragment)(contentBuilder =>
			{
				// thread
				contentBuilder.OpenElement(_i++, "thead");
				RenderTableRow((TableRow)table[0], "th", contentBuilder, TableCellMinWidth);
				contentBuilder.CloseElement();

				// tbody
				contentBuilder.OpenElement(_i++, "tbody");
				for (var j = 1; j < table.Count; j++)
				{
					RenderTableRow((TableRow)table[j], "td", contentBuilder);
				}

				contentBuilder.CloseElement();
			}));
			builder.CloseComponent();
		}

		private void RenderTableRow(TableRow row, string cellElementName, RenderTreeBuilder builder, int? minWidth = null)
		{
			builder.OpenElement(_i++, "tr");

			for (var j = 0; j < row.Count; j++)
			{
				var cell = (TableCell)row[j];
				builder.OpenElement(_i++, cellElementName);

				if (minWidth is > 0)
					builder.AddAttribute(_i++, "style", $"min-width:{minWidth}px");

				if (cell.Count != 0 && cell[0] is ParagraphBlock paragraphBlock)
					RenderParagraphBlock(paragraphBlock, builder);

				builder.CloseElement();
			}

			builder.CloseElement();
		}

		private void RenderList(ListBlock list, RenderTreeBuilder builder)
		{
			if (list.Count == 0)
				return;

			var elementName = list.IsOrdered ? "ol" : "ul";
			builder.OpenElement(_i++, elementName);

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
							builder.OpenElement(_i++, "li");
							RenderParagraphBlock(x, builder);
							builder.CloseElement();
							break;
					}
				}
			}

			builder.CloseElement();
		}

		private void OnCodeBlockThemeChanged(object? sender, CodeBlockTheme e) =>
			CodeBlockTheme = e;
	}
}
