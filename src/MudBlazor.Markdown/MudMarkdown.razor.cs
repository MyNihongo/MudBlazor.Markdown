using System.Windows.Input;
using Markdig;
using Markdig.Extensions.Tables;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

// ReSharper disable once CheckNamespace
namespace MudBlazor
{
	public sealed class MudMarkdown : ComponentBase
	{
		private readonly MarkdownPipeline _pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
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
		/// Typography variant to use for Heading Level 1.<br/>
		/// Default: <see cref="Typo.h1"/>
		/// </summary>
		[Parameter]
		public Typo H1Typo { get; set; } = Typo.h1;

		/// <summary>
		/// Typography variant to use for Heading Level 2.<br/>
		/// Default: <see cref="Typo.h2"/>
		/// </summary>
		[Parameter]
		public Typo H2Typo { get; set; } = Typo.h2;

		/// <summary>
		/// Typography variant to use for Heading Level 3.<br/>
		/// Default: <see cref="Typo.h3"/>
		/// </summary>
		[Parameter]
		public Typo H3Typo { get; set; } = Typo.h3;

		/// <summary>
		/// Typography variant to use for Heading Level 4.<br/>
		/// Default: <see cref="Typo.h4"/>
		/// </summary>
		[Parameter] 
		public Typo H4Typo { get; set; } = Typo.h4;

		/// <summary>
		/// Typography variant to use for Heading Level 5.<br/>
		/// Default: <see cref="Typo.h5"/>
		/// </summary>
		[Parameter]
		public Typo H5Typo { get; set; } = Typo.h5;

		/// <summary>
		/// Typography variant to use for Heading Level 6.<br/>
		/// Default: <see cref="Typo.h6"/>
		/// </summary>
		[Parameter]
		public Typo H6Typo { get; set; } = Typo.h6;

		protected override void BuildRenderTree(RenderTreeBuilder builder)
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
							Typo? typo = heading.Level switch
							{
								1 => H1Typo,
								2 => H2Typo,
								3 => H3Typo,
								4 => H4Typo,
								5 => H5Typo,
								6 => H6Typo,
								_ => null
							};

							if (typo.HasValue)
							{
								RenderParagraphBlock(heading, builder, typo.Value);
							}

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
				}
			}
		}

		private void RenderParagraphBlock(LeafBlock paragraph, RenderTreeBuilder builder, Typo typo = Typo.body1)
		{
			if (paragraph.Inline == null)
				return;

			builder.OpenComponent<MudText>(_i++);
			builder.AddAttribute(_i++, nameof(MudText.Typo), typo);
			builder.AddAttribute(_i++, nameof(MudText.ChildContent), (RenderFragment)(contentBuilder =>
			{
				foreach (var inline in paragraph.Inline)
				{
					switch (inline)
					{
						case LiteralInline x:
							{
								contentBuilder.AddContent(_i++, x.Content);
								break;
							}
						case HtmlInline x:
							{
								contentBuilder.AddMarkupContent(_i++, x.Tag);
								break;
							}
						case LineBreakInline:
							{
								contentBuilder.OpenElement(_i++, "br");
								contentBuilder.CloseElement();
								break;
							}
						case CodeInline x:
							{
								contentBuilder.OpenElement(_i++, "code");
								contentBuilder.AddContent(_i++, x.Content);
								contentBuilder.CloseElement();
								break;
							}
						case EmphasisInline x:
							{
								RenterEmphasis(x, contentBuilder);
								break;
							}
						case LinkInline x:
							{
								if (LinkCommand == null)
								{
									contentBuilder.OpenComponent<MudLink>(_i++);
									contentBuilder.AddAttribute(_i++, nameof(MudLink.Href), x.Url);
									contentBuilder.AddAttribute(_i++, nameof(MudLink.ChildContent), (RenderFragment)(linkBuilder => RenderContent(linkBuilder, x)));
									contentBuilder.CloseComponent();
								}
								else
								{
									contentBuilder.OpenComponent<MudLinkButton>(_i++);
									contentBuilder.AddAttribute(_i++, nameof(MudLinkButton.Command), LinkCommand);
									contentBuilder.AddAttribute(_i++, nameof(MudLinkButton.CommandParameter), x.Url);
									contentBuilder.AddAttribute(_i++, nameof(MudLinkButton.ChildContent), (RenderFragment)(linkBuilder => RenderContent(linkBuilder, x)));
									contentBuilder.CloseComponent();
								}

								break;

								void RenderContent(RenderTreeBuilder linkInlineBuilder, LinkInline linkInline)
								{
									foreach (var item in linkInline)
										linkInlineBuilder.AddContent(_i++, item);
								}
							}
					}
				}
			}));

			builder.CloseComponent();
		}

		private void RenterEmphasis(EmphasisInline emphasis, RenderTreeBuilder builder)
		{
			if (!emphasis.TryGetEmphasisElement(out var elementName))
				return;

			builder.OpenElement(_i++, elementName);

			foreach (var inline in emphasis)
				switch (inline)
				{
					case LiteralInline x:
						{
							builder.AddContent(_i++, x);
							break;
						}
					case EmphasisInline x:
						{
							RenterEmphasis(x, builder);
							break;
						}
				}

			builder.CloseElement();
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

			builder.OpenElement(_i++, "ul");

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
	}
}
