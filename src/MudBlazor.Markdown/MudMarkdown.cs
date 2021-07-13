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
			_i = 0;
			MarkdownDocument parsedText;

			try
			{
				parsedText = Markdig.Markdown.Parse(Value);
			}
			catch
			{
				return;
			}

			for (var i = 0; i < parsedText.Count; i++)
			{
				switch (parsedText[i])
				{
					case ParagraphBlock x:
						ProcessParagraph(x, builder);
						break;
				}
			}
		}

		private void ProcessParagraph(ParagraphBlock paragraph, RenderTreeBuilder builder)
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
