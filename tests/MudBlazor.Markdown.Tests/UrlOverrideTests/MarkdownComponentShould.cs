using Bunit;
using Xunit;

namespace MudBlazor.Markdown.Tests.UrlOverrideTests
{
	public sealed class MarkdownComponentShould : UrlOverrideTestsBase
	{
		[Fact]
		public void OverrideAllLinks()
		{
			const string value =
@"[absolute](https://www.google.co.jp/)
[relative](/tokyo)
[id](#edogawa)";

			const string expectedValue =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1 mud-inherit-text'>
		<a rel='noopener noreferrer' href='overriddenhttps://www.google.co.jp/' target='_blank' class='mud-typography mud-link mud-primary-text mud-link-underline-hover mud-typography-body1'>
			absolute
		</a>
		<br />
		<a href='overridden/tokyo' class='mud-typography mud-link mud-primary-text mud-link-underline-hover mud-typography-body1'>
			relative
		</a>
		<br />
		<a href='overridden#edogawa' class='mud-typography mud-link mud-primary-text mud-link-underline-hover mud-typography-body1'>
			id
		</a>
	</p>
</article>";

			using var fixture = CreateFixture(value);
			fixture.MarkupMatches(expectedValue);
		}

		[Fact]
		public void OverrideImageLink()
		{
			const string value = @"![img](/tokyo/sky-tree.png)";
			const string expectedValue =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1 mud-inherit-text'>
		<img src='overridden/tokyo/sky-tree.png' alt='img' />
	</p>
</article>";

			using var fixture = CreateFixture(value);
			fixture.MarkupMatches(expectedValue);
		}
	}
}
