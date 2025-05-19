namespace MudBlazor.Markdown.Tests.Utils.HeadingBlockExTests;

public sealed class BuildHeadingContentShould : BuildIdStringShouldTestsBase
{
	[Theory]
	[InlineData(" ")]
	[InlineData("  ")]
	public void EscapeWhitespace(string whitespace)
	{
		const string expected = "some-text";
		string expectedText = $"some{whitespace}text",  value = $"# {expectedText}";

		var result = CreateFixture(value)
			.BuildHeadingContent();

		result
			.Should()
			.Be(expected);
	}

	[Fact]
	public void ConvertToLower()
	{
		const string value = "# Some Text",
			expected = "some-text";

		var result = CreateFixture(value)
			.BuildHeadingContent();

		result
			.Should()
			.Be(expected);
	}

	[Theory]
	[InlineData('+')]
	[InlineData(':')]
	[InlineData('&')]
	public void EscapeCharacters(char inputChar)
	{
		const string expected = "some--text";
		string expectedText = $"some {inputChar} text", value = $"# {expectedText}";

		var result = CreateFixture(value)
			.BuildHeadingContent();

		result
			.Should()
			.Be(expected);
	}
}
