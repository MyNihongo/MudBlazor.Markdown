using System.Linq;
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

		[Parameter]
		public string Value { get; set; } = string.Empty;

		[Parameter]
		public ICommand? LinkCommand { get; set; }

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			if (string.IsNullOrEmpty(Value))
				return;

			_i = 0;

			var parsedText = Markdig.Markdown.Parse(Value, _pipeline);
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
				}
			}
		}

		private void RenderParagraphBlock(ParagraphBlock paragraph, RenderTreeBuilder builder)
		{
			if (paragraph.Inline == null)
				return;

			builder.OpenComponent<MudText>(_i++);
			builder.AddAttribute(_i++, nameof(MudText.Typo), Typo.body1);
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
								if (TryGetEmphasisElement(x, out var elementName))
								{
									var content = (LiteralInline)x.Single();

									contentBuilder.OpenElement(_i++, elementName);
									contentBuilder.AddContent(_i++, content);
									contentBuilder.CloseElement();
								}
								break;
							}
						case LinkInline x:
							{
								var content = (LiteralInline)x.Single();

								if (LinkCommand == null)
								{
									contentBuilder.OpenComponent<MudLink>(_i++);
									contentBuilder.AddAttribute(_i++, nameof(MudLink.Href), x.Url);
									contentBuilder.AddAttribute(_i++, nameof(MudLink.ChildContent), (RenderFragment)(linkBuilder =>
									{
										linkBuilder.AddContent(_i++, content);
									}));
									contentBuilder.CloseComponent();
								}
								else
								{
									contentBuilder.OpenComponent<MudLinkButton>(_i++);
									contentBuilder.AddAttribute(_i++, nameof(MudLinkButton.Command), LinkCommand);
									contentBuilder.AddAttribute(_i++, nameof(MudLinkButton.CommandParameter), x.Url);
									contentBuilder.AddAttribute(_i++, nameof(MudLinkButton.ChildContent), (RenderFragment)(linkBuilder =>
									{
										linkBuilder.AddContent(_i++, content);
									}));
									contentBuilder.CloseComponent();
								}

								break;
							}
					}
				}
			}));

			builder.CloseComponent();
		}

		private void RenderTable(Table table, RenderTreeBuilder builder)
		{
			// First child is columns
			if (table.Count < 2)
				return;

			builder.OpenComponent<MudSimpleTable>(_i++);
			builder.AddAttribute(_i++, nameof(MudSimpleTable.Striped), true);
			builder.AddAttribute(_i++, nameof(MudSimpleTable.Bordered), true);
			builder.AddAttribute(_i++, nameof(MudSimpleTable.ChildContent), (RenderFragment)(contentBuilder =>
			{
				// thread
				contentBuilder.OpenElement(_i++, "thead");
				RenderTableRow((TableRow)table[0], "th", contentBuilder);
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

		private void RenderTableRow(TableRow row, string cellElementName, RenderTreeBuilder builder)
		{
			builder.OpenElement(_i++, "tr");

			for (var j = 0; j < row.Count; j++)
			{
				var cell = (TableCell)row[j];
				builder.OpenElement(_i++, cellElementName);

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

			builder.OpenComponent<MudList>(_i++);
			builder.AddAttribute(_i++, nameof(MudList.DisablePadding), true);
			builder.AddAttribute(_i++, nameof(MudLink.ChildContent), (RenderFragment)(contentBuilder =>
			{
				for (var j = 0; j < list.Count; j++)
					if (list[j] is ListItemBlock {Count: 1} listItem && listItem[0] is ParagraphBlock paragraph)
					{
						contentBuilder.OpenComponent<MudListItem>(_i++);
						RenderParagraphBlock(paragraph, contentBuilder);
						contentBuilder.CloseComponent();
					}
			}));
			builder.CloseComponent();
		}

		private static bool TryGetEmphasisElement(EmphasisInline emphasis, out string value)
		{
			value = emphasis.DelimiterChar switch
			{
				'*' => emphasis.DelimiterCount switch
				{
					1 => "i",
					2 => "b",
					_ => string.Empty
				},
				_ => string.Empty
			};

			return !string.IsNullOrEmpty(value);
		}
	}
}
