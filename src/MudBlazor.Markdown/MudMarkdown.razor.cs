using System.Linq;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

// ReSharper disable once CheckNamespace
namespace MudBlazor
{
	public sealed class MudMarkdown : ComponentBase
	{
		private int _i;

		[Parameter]
		public string Value { get; set; } = string.Empty;

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			if (string.IsNullOrEmpty(Value))
				return;

			_i = 0;

			var parsedText = Markdig.Markdown.Parse(Value);
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
							contentBuilder.AddContent(_i++, x.Content);
							break;
						case LineBreakInline:
							contentBuilder.OpenElement(_i++, "br");
							contentBuilder.CloseElement();
							break;
						case CodeInline x:
							{
								contentBuilder.OpenElement(_i++, "code");
								contentBuilder.AddContent(_i++, x.Content);
								contentBuilder.CloseElement();
							}
							break;
						case EmphasisInline x:
							{
								if (TryGetEmphasisElement(x, out var elementName))
								{
									var content = (LiteralInline)x.Single();

									contentBuilder.OpenElement(_i++, elementName);
									contentBuilder.AddContent(_i++, content);
									contentBuilder.CloseElement();
								}
							}
							break;
					}
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
