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
				i += BuildPower(builder, value[i], powValue, prependSpacing) + 1;
			}
			else if (value[i].IsDigit())
			{
				BuildDigit(builder, value[i], prependSpacing);
			}
			else if (value[i].IsAlpha())
			{
				BuildAlpha(builder, value[i], prependSpacing);
			}
			else if (value[i].IsMathOperation(out var hasSpacing, out var customChar))
			{
				if (hasSpacing)
					prependSpacing = applySpacing;

				BuildOperation(builder, customChar ?? value[i], prependSpacing);
				continue;
			}
			else
			{
				continue;
			}

			prependSpacing = false;
		}
	}

	private int BuildPower(in RenderTreeBuilder builder, in char firstChar, in ReadOnlySpan<char> value, bool prependSpacing = false)
	{
		builder.OpenElement(_elementIndex++, "msup");
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
					BuildBlock(builder, blockValue);
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
			BuildDigit(builder, @char, prependSpacing);
		}
		else if (@char.IsAlpha())
		{
			BuildAlpha(builder, @char, prependSpacing);
		}
		else if (@char.IsMathOperation(out _, out var customChar))
		{
			BuildOperation(builder, customChar ?? @char, prependSpacing);
		}
	}

	private void BuildDigit(in RenderTreeBuilder builder, in char @char, bool prependSpacing = false)
	{
		builder.OpenElement(_elementIndex++, "mn");

		if (prependSpacing)
			builder.AddAttribute(_elementIndex++, "class", SpacingClass);

		builder.AddContent(_elementIndex++, @char);
		builder.CloseElement();
	}

	private void BuildAlpha(in RenderTreeBuilder builder, in char @char, bool prependSpacing = false)
	{
		builder.OpenElement(_elementIndex++, "mi");

		if (prependSpacing)
			builder.AddAttribute(_elementIndex++, "class", SpacingClass);

		builder.AddContent(_elementIndex++, @char);
		builder.CloseElement();
	}

	private void BuildOperation(in RenderTreeBuilder builder, in char @char, bool prependSpacing = true)
	{
		builder.OpenElement(_elementIndex++, "mo");

		if (prependSpacing)
			builder.AddAttribute(_elementIndex++, "class", SpacingClass);

		builder.AddContent(_elementIndex++, @char);
		builder.CloseElement();
	}
}
