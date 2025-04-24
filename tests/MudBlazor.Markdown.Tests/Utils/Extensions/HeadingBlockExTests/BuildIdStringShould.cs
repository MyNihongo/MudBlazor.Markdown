namespace MudBlazor.Markdown.Tests.Utils.Extensions.HeadingBlockExTests;

public sealed class BuildIdStringShould : BuildIdStringShouldTestsBase
{
	[Theory]
	[InlineData(" ")]
	[InlineData("  ")]
	public void EscapeWhitespace(string whitespace)
	{
		const string expected = "some-text";
		var value = $"# some{whitespace}text";

		var result = CreateFixture(value)
			.BuildIdString();

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
			.BuildIdString();

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
		var value = $"# some {inputChar} text";

		var result = CreateFixture(value)
			.BuildIdString();

		result
			.Should()
			.Be(expected);
	}
}