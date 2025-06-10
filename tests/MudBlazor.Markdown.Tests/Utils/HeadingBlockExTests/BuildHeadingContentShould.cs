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
	[InlineData('+', "%2b")]
	[InlineData(':', "%3a")]
	[InlineData('&', "%26")]
	public void EncodeSpecialCharacters(char inputChar, string expectedChar)
	{
		var value = $"# some {inputChar} text";
		string expectedId = $"some-{expectedChar}-text", expectedText = $"some {inputChar} text";

		var result = CreateFixture(value)
			.BuildHeadingContent();

		var expected = new HeadingContent(expectedId, expectedText);

		result
			.Should()
			.Be(expected);
	}

	[Fact]
	public void EncodeHtmlCharacters()
	{
		const string value = "# Some text (日本語)";
		const string expectedId = "some-text-(%e6%97%a5%e6%9c%ac%e8%aa%9e)", expectedText = "Some text (日本語)";

		var result = CreateFixture(value)
			.BuildHeadingContent();

		var expected = new HeadingContent(expectedId, expectedText);

		result
			.Should()
			.Be(expected);
	}
}
