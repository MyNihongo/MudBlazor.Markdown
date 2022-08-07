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
		BuildMarkupContent(builder, Value.AsSpan(), true);
		builder.CloseComponent();
	}

	private void BuildMarkupContent(RenderTreeBuilder builder, ReadOnlySpan<char> value, bool withSpacing)
	{
		var prependSpacing = false;

		for (var i = 0; i < value.Length; i++)
		{
			if (i + 2 < value.Length && value[i + 1] == '^')
			{
				RenderCharOrBlock(builder, "msup", value, ref i);
			}
			else if (i + 2 < value.Length && value[i + 1] == '_')
			{
				RenderCharOrBlock(builder, "msub", value, ref i);
			}
			else if (value[i].IsAlpha())
			{
				RenderAlpha(builder, value[i], prependSpacing && withSpacing);
			}
			else if (value[i].IsDigit())
			{
				RenderDigit(builder, value[i], prependSpacing && withSpacing);
			}
			else if (value[i].IsMathOperation(out var hasSpacing, out var customChar))
			{
				RenderMathOperation(builder, customChar ?? value[i], hasSpacing && withSpacing);

				if (hasSpacing)
				{
					prependSpacing = true;
					continue;
				}
			}
			else if (value[i] == '\\')
			{
				var expressionString = GetExpressionString(value, i + 1);
				i += expressionString.Length + 1;

				switch (expressionString)
				{
					case "ne":
						{
							RenderMathOperation(builder, '≠');
							prependSpacing = true;
							break;
						}
					case "le":
						{
							RenderMathOperation(builder, '≤');
							prependSpacing = true;
							break;
						}
					case "ge":
					{
						RenderMathOperation(builder, '≥');
						prependSpacing = true;
						break;
					}
					case "overline":
					{
						TryRenderBlock(builder, "mover", value, ref i, true);
						continue;
					}
				}

				continue;
			}
			else
			{
				continue;
			}

			prependSpacing = false;
		}
	}

	private void RenderCharOrBlock(in RenderTreeBuilder builder, in string elementName, in ReadOnlySpan<char> value, ref int i)
	{
		var firstChar = value[i];

		if (value[i + 2] == '{')
		{
			var startIndex = i + 3;
			var endIndex = value.IndexOf('}', startIndex);
			if (endIndex != -1)
			{
				builder.OpenElement(_elementIndex++, elementName);

				RenderAlphaOrDigit(builder, firstChar);

				builder.OpenElement(_elementIndex++, "mrow");
				BuildMarkupContent(builder, value.Slice(startIndex, endIndex - startIndex), false);
				builder.CloseElement();

				builder.CloseElement();

				i = endIndex + 1;
			}
		}
		else
		{
			// For the power sign and next symbol
			i += 2;
			if (i < value.Length)
			{
				var lastChar = value[i];
				builder.OpenElement(_elementIndex++, elementName);

				RenderAlphaOrDigit(builder, firstChar);
				RenderAlphaOrDigit(builder, lastChar);

				builder.CloseElement();
			}
		}
	}

	private void TryRenderBlock(in RenderTreeBuilder builder, in string elementName, in ReadOnlySpan<char> value, ref int i, in bool withSpacing = false)
	{
		if (value[i] != '{')
			return;

		var startIndex = i + 1;
		var endIndex = value.IndexOf('}', startIndex);
		if (endIndex != -1)
		{
			builder.OpenElement(_elementIndex++, elementName);
			BuildMarkupContent(builder, value.Slice(startIndex, endIndex - startIndex), withSpacing);
			builder.CloseElement();

			i = endIndex + 1;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void RenderAlphaOrDigit(in RenderTreeBuilder builder, in char value, in bool prependSpacing = false)
	{
		if (value.IsAlpha())
			RenderAlpha(builder, value, prependSpacing);
		else if (value.IsDigit())
			RenderDigit(builder, value, prependSpacing);
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
	private void RenderMathOperation(in RenderTreeBuilder builder, in char value, in bool prependSpacing = true)
	{
		builder.OpenElement(_elementIndex++, "mo");

		if (prependSpacing)
			builder.AddAttribute(_elementIndex++, "class", SpacingClass);

		builder.AddContent(_elementIndex++, value);
		builder.CloseComponent();
	}

	private static string GetExpressionString(in ReadOnlySpan<char> value, in int i)
	{
		var j = i;
		for (; j < value.Length; j++)
			if (!value[j].IsAlpha())
				break;

		return value.Slice(i, j - i).ToString();
	}
}
