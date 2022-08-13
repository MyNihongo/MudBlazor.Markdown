namespace MudBlazor;

internal sealed class MudMathJax : ComponentBase
{
	private const string SpacingClass = "pl-2";
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

		BuildBlock(builder, Value.AsSpan());

		builder.CloseComponent();
	}

	private void BuildBlock(in RenderTreeBuilder builder, in ReadOnlySpan<char> value)
	{
		for (var i = 0; i < value.Length; i++)
		{
			if (i + 1 < value.Length && value[i + 1] == '^')
			{
				var powValue = value[(i + 2)..];
				i += BuildPower(builder, value[i], powValue) + 1;
			}
			else if (value[i].IsDigit())
			{
				BuildDigit(builder, value[i]);
			}
			else if (value[i].IsAlpha())
			{
				BuildAlpha(builder, value[i]);
			}
			else if (value[i].IsMathOperation(out _, out var customChar))
			{
				BuildOperation(builder, customChar ?? value[i]);
			}
		}
	}

	private int BuildPower(in RenderTreeBuilder builder, in char firstChar, in ReadOnlySpan<char> value)
	{
		builder.OpenElement(_elementIndex++, "msup");
		BuildMathSymbol(builder, firstChar);

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
					BuildBlock(builder, blockValue);
					builder.CloseElement();

					blockLength = endIndex + 1;
				}
			}
			else
			{
				BuildMathSymbol(builder, value[0]);
				blockLength = 1;
			}
		}

		builder.CloseElement();
		return blockLength;
	}

	private void BuildMathSymbol(in RenderTreeBuilder builder, in char @char)
	{
		if (@char.IsDigit())
		{
			BuildDigit(builder, @char);
		}
		else if (@char.IsAlpha())
		{
			BuildAlpha(builder, @char);
		}
		else if (@char.IsMathOperation(out _, out var customChar))
		{
			BuildOperation(builder, customChar ?? @char);
		}
	}

	private void BuildDigit(in RenderTreeBuilder builder, in char @char)
	{
		builder.OpenElement(_elementIndex++, "mn");
		builder.AddContent(_elementIndex++, @char);
		builder.CloseElement();
	}

	private void BuildAlpha(in RenderTreeBuilder builder, in char @char)
	{
		builder.OpenElement(_elementIndex++, "mi");
		builder.AddContent(_elementIndex++, @char);
		builder.CloseElement();
	}

	private void BuildOperation(in RenderTreeBuilder builder, in char @char)
	{
		builder.OpenElement(_elementIndex++, "mo");
		builder.AddContent(_elementIndex++, @char);
		builder.CloseElement();
	}
}
