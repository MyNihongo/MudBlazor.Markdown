namespace MudBlazor.Markdown.Tests.Extensions.SourceSpanExTests;

public sealed class TryGetTextShould
{
	[Fact]
	public void ReturnEmptyIfStartGreaterThanString()
	{
		const string value = nameof(value);
		var fixture = new SourceSpan(value.Length, value.Length);

		var result = fixture.TryGetText(value);

		result
			.Should()
			.BeEmpty();
	}

	[Fact]
	public void ReturnSubstringIfLengthTooGreat()
	{
		const string value = nameof(value), expected = "e";
		var fixture = new SourceSpan(value.Length - 1, value.Length);

		var result = fixture.TryGetText(value);

		result
			.Should()
			.Be(expected);
	}

	[Fact]
	public void ReturnStringWithin()
	{
		const string value = nameof(value), expected = "alu";
		var fixture = new SourceSpan(1, 3);

		var result = fixture.TryGetText(value);

		result
			.Should()
			.Be(expected);
	}

	[Fact]
	public void ReturnStringStartsWith()
	{
		const string value = nameof(value), expected = "valu";
		var fixture = new SourceSpan(0, 3);

		var result = fixture.TryGetText(value);

		result
			.Should()
			.Be(expected);
	}

	[Fact]
	public void ReturnStringEndsWith()
	{
		const string value = nameof(value), expected = "alue";
		var fixture = new SourceSpan(1, value.Length - 1);

		var result = fixture.TryGetText(value);

		result
			.Should()
			.Be(expected);
	}
}
