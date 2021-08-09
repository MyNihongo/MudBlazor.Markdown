using FluentAssertions;
using Xunit;

namespace MudBlazor.Markdown.Tests.Extensions.HeadingBlockExTests
{
	public sealed class BuildIdStringShould : BuildIdStringShouldTestsBase
	{
		[Theory]
		[InlineData(" ")]
		[InlineData("  ")]
		public void EscapeWhitespace(string whitespace)
		{
			const string expectedValue = "some-text";
			var value = $"# some{whitespace}text";

			var result = CreateFixture(value)
				.BuildIdString();

			result
				.Should()
				.Be(expectedValue);
		}

		[Fact]
		public void ConvertToLower()
		{
			const string value = "# Some Text",
				expectedValue = "some-text";

			var result = CreateFixture(value)
				.BuildIdString();

			result
				.Should()
				.Be(expectedValue);
		}

		[Theory]
		[InlineData('+')]
		[InlineData(':')]
		[InlineData('&')]
		public void EscapeCharacters(char inputChar)
		{
			const string expectedValue = "some--text";
			var value = $"# some {inputChar} text";

			var result = CreateFixture(value)
				.BuildIdString();

			result
				.Should()
				.Be(expectedValue);
		}
	}
}
