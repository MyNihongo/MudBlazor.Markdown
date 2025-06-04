namespace MudBlazor.Markdown.Tests.Utils.HeadingBlockExTests;

public sealed class BuildHeadingContentShould : BuildIdStringShouldTestsBase
{
	[Theory]
	[InlineData(" ")]
	[InlineData("  ")]
	public void EscapeWhitespace(string whitespace)
	{
		const string expectedId = "some-text";
		string expectedText = $"some{whitespace}text", value = $"# {expectedText}";

		var result = CreateFixture(value)
			.BuildHeadingContent();

		var expected = new HeadingContent(expectedId, expectedText);

		result
			.Should()
			.Be(expected);
	}

	[Fact]
	public void ConvertToLower()
	{
		const string value = "# Some Text",
			expectedId = "some-text",
			expectedText = "Some Text";

		var result = CreateFixture(value)
			.BuildHeadingContent();

		var expected = new HeadingContent(expectedId, expectedText);

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
		const string expectedId = "some--text";
		string expectedText = $"some {inputChar} text", value = $"# {expectedText}";

		var result = CreateFixture(value)
			.BuildHeadingContent();

		var expected = new HeadingContent(expectedId, expectedText);

		result
			.Should()
			.Be(expected);
	}
}
