namespace MudBlazor;

internal sealed class MudMathJax : ComponentBase
{
	private const string SpacingClass = "pl-2",
		DigitElement = "mn", AlphaElement = "mi", OperationElement = "mo";
	private const char BlockStartChar = '{', BlockEndChar = '}';
	private int _elementIndex;

	[Parameter]
	public StringSlice Value { get; set; }

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		_elementIndex = 0;

		builder.OpenElement(_elementIndex++, "mjx-container");
		builder.AddAttribute(_elementIndex++, "tabindex", "0");
		builder.AddAttribute(_elementIndex++, "class", "mud-markdown-mjx-container");

		BuildBlock(builder, Value.AsSpan(), true);

		builder.CloseComponent();
	}

	private void BuildBlock(in RenderTreeBuilder builder, in ReadOnlySpan<char> value, bool applySpacing = false)
	{
		var prependSpacing = false;

		for (var i = 0; i < value.Length; i++)
		{
			if (i + 1 < value.Length && value[i + 1] == '^')
			{
				var powValue = value[(i + 2)..];
				i += BuildElementRow(builder, "msup", value[i], powValue, prependSpacing) + 1;
			}
			else if (i + 1 < value.Length && value[i + 1] == '_')
			{
				var powValue = value[(i + 2)..];
				i += BuildElementRow(builder, "msub", value[i], powValue, prependSpacing) + 1;
			}
			else if (value[i].IsDigit())
			{
				BuildElement(builder, DigitElement, value[i], prependSpacing);
			}
			else if (value[i].IsAlpha())
			{
				BuildElement(builder, AlphaElement, value[i], prependSpacing);
			}
			else if (value[i].IsMathOperation(out var hasSpacing, out var customChar))
			{
				if (hasSpacing)
					prependSpacing = applySpacing;

				BuildElement(builder, OperationElement, customChar ?? value[i], prependSpacing);
				continue;
			}
			else if (value[i] == '\\')
			{
				var expressionString = GetExpressionString(value, i + 1);
				i += expressionString.Length + 1;

				switch (expressionString)
				{
					case "ne":
						{
							BuildElement(builder, OperationElement, '≠');
							prependSpacing = true;
							continue;
						}
					case "le":
						{
							BuildElement(builder, OperationElement, '≤');
							prependSpacing = true;
							continue;
						}
					case "ge":
						{
							BuildElement(builder, OperationElement, '≥');
							prependSpacing = true;
							continue;
						}
					case "overline":
						{
							var overlineValue = value[i..];
							i += BuildElementRow(builder, "mover", '\r', overlineValue, prependSpacing, true) + 1;
							break;
						}

				}
			}
			else
			{
				continue;
			}

			prependSpacing = false;
		}
	}

	private int BuildElementRow(in RenderTreeBuilder builder, in string elementName, in char firstChar, in ReadOnlySpan<char> value, bool prependSpacing = false, bool applySpacing = false)
	{
		builder.OpenElement(_elementIndex++, elementName);
		BuildSymbol(builder, firstChar, prependSpacing);

		var blockLength = 0;
		if (!value.IsEmpty)
		{
			if (value[0] == BlockStartChar)
			{
				var endIndex = value.IndexOf(BlockEndChar);
				if (endIndex != -1)
				{
					var blockValue = value[1..endIndex];

					builder.OpenElement(_elementIndex, "mrow");
					BuildBlock(builder, blockValue, applySpacing);
					builder.CloseElement();

					blockLength = endIndex + 1;
				}
			}
			else
			{
				BuildSymbol(builder, value[0]);
				blockLength = 1;
			}
		}

		builder.CloseElement();
		return blockLength;
	}

	private void BuildSymbol(in RenderTreeBuilder builder, in char @char, bool prependSpacing = false)
	{
		if (@char.IsDigit())
		{
			BuildElement(builder, DigitElement, @char, prependSpacing);
		}
		else if (@char.IsAlpha())
		{
			BuildElement(builder, AlphaElement, @char, prependSpacing);
		}
		else if (@char.IsMathOperation(out _, out var customChar))
		{
			BuildElement(builder, OperationElement, customChar ?? @char, prependSpacing);
		}
	}

	private void BuildElement(in RenderTreeBuilder builder, in string elementName, in char @char, bool prependSpacing = true)
	{
		builder.OpenElement(_elementIndex++, elementName);

		if (prependSpacing)
			builder.AddAttribute(_elementIndex++, "class", SpacingClass);

		builder.AddContent(_elementIndex++, @char);
		builder.CloseElement();
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
