using System.Runtime.CompilerServices;

namespace MudBlazor;

internal sealed class MudMathJax : ComponentBase
{
	private const string SpacingClass = "pl-2";
	private int _elementIndex;

	[Parameter]
	public StringSlice Value { get; set; }

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		_elementIndex = 0;

		builder.OpenElement(_elementIndex++, "mjx-container");
		builder.AddAttribute(_elementIndex++, "tabindex", "0");
		builder.AddAttribute(_elementIndex++, "class", "mud-markdown-mjx-container");
		BuildMarkupContent(builder, Value.AsSpan());
		builder.CloseComponent();
	}

	private void BuildMarkupContent(RenderTreeBuilder builder, ReadOnlySpan<char> value)
	{
		var prependSpacing = false;

		for (var i = 0; i < value.Length; i++)
		{
			if (i + 1 < value.Length && value[i + 1] == '^')
			{
				i++;
			}
			else if (value[i].IsAlpha())
			{
				RenderAlpha(builder, value[i], prependSpacing);
			}
			else if (value[i].IsDigit())
			{
				RenderDigit(builder, value[i], prependSpacing);
			}
			else if (value[i].IsMathOperation())
			{
				RenderMathOperation(builder, value[i]);
				prependSpacing = true;
				continue;
			}
			else
			{
				continue;
			}

			prependSpacing = false;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void RenderAlpha(in RenderTreeBuilder builder, in char value, in bool prependSpacing = false)
	{
		builder.OpenElement(_elementIndex++, "mi");

		if (prependSpacing)
			builder.AddAttribute(_elementIndex++, "class", SpacingClass);

		builder.AddContent(_elementIndex++, value);
		builder.CloseComponent();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void RenderDigit(in RenderTreeBuilder builder, in char value, in bool prependSpacing = false)
	{
		builder.OpenElement(_elementIndex++, "mn");

		if (prependSpacing)
			builder.AddAttribute(_elementIndex++, "class", SpacingClass);

		builder.AddContent(_elementIndex++, value);
		builder.CloseComponent();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void RenderMathOperation(in RenderTreeBuilder builder, in char value)
	{
		builder.OpenElement(_elementIndex++, "mo");
		builder.AddAttribute(_elementIndex++, "class", $"mud-markdown-mjx-expression {SpacingClass}");
		builder.AddContent(_elementIndex++, value);
		builder.CloseComponent();
	}
}
