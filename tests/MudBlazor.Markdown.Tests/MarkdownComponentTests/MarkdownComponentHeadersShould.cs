﻿using System;
using Bunit;
using MyNihongo.Option;
using Xunit;

namespace MudBlazor.Markdown.Tests.MarkdownComponentTests
{
	public sealed class MarkdownComponentHeadersShould : MarkdownComponentTestsBase
	{
		[Theory]
		[InlineData("#", "h1")]
		[InlineData("##", "h2")]
		[InlineData("###", "h3")]
		[InlineData("####", "h4")]
		[InlineData("#####", "h5")]
		[InlineData("######", "h6")]
		public void RenderHeaders(string valueInput, string expected)
		{
			var value = valueInput + " some text";

			var expectedValue =
$@"<article class='mud-markdown-body'>
	<{expected} id='some-text' class='mud-typography mud-typography-{expected} mud-inherit-text'>
		some text
	</{expected}>
</article>";

			using var fixture = CreateFixture(value);
			fixture.MarkupMatches(expectedValue);
		}

		[Theory]
		[InlineData(Typo.h1, "h1")]
		[InlineData(Typo.h2, "h2")]
		[InlineData(Typo.h3, "h3")]
		[InlineData(Typo.h4, "h4")]
		[InlineData(Typo.h5, "h5")]
		[InlineData(Typo.h6, "h6")]
		public void RenderHeadersWithDifferentTypoH1(Typo newTypo, string expected)
		{
			const string value = "# some text";

			var expectedValue =
$@"<article class='mud-markdown-body'>
	<{expected} id='some-text' class='mud-typography mud-typography-{expected} mud-inherit-text'>
		some text
	</{expected}>
</article>";

			var @override = Optional<Func<Typo, Typo>>.Of(Override);

			using var fixture = CreateFixture(value, overrideHeaderTypo: @override);
			fixture.MarkupMatches(expectedValue);

			Typo Override(Typo x) =>
				x == Typo.h1 ? newTypo : x;
		}

		[Theory]
		[InlineData(Typo.h1, "h1")]
		[InlineData(Typo.h2, "h2")]
		[InlineData(Typo.h3, "h3")]
		[InlineData(Typo.h4, "h4")]
		[InlineData(Typo.h5, "h5")]
		[InlineData(Typo.h6, "h6")]
		public void RenderHeadersWithDifferentTypoH2(Typo newTypo, string expected)
		{
			const string value = "## some text";

			var expectedValue =
$@"<article class='mud-markdown-body'>
	<{expected} id='some-text' class='mud-typography mud-typography-{expected} mud-inherit-text'>
		some text
	</{expected}>
</article>";

			var @override = Optional<Func<Typo, Typo>>.Of(Override);

			using var fixture = CreateFixture(value, overrideHeaderTypo: @override);
			fixture.MarkupMatches(expectedValue);

			Typo Override(Typo x) =>
				x == Typo.h2 ? newTypo : x;
		}

		[Theory]
		[InlineData(Typo.h1, "h1")]
		[InlineData(Typo.h2, "h2")]
		[InlineData(Typo.h3, "h3")]
		[InlineData(Typo.h4, "h4")]
		[InlineData(Typo.h5, "h5")]
		[InlineData(Typo.h6, "h6")]
		public void RenderHeadersWithDifferentTypoH3(Typo newTypo, string expected)
		{
			const string value = "### some text";

			var expectedValue =
$@"<article class='mud-markdown-body'>
	<{expected} id='some-text' class='mud-typography mud-typography-{expected} mud-inherit-text'>
		some text
	</{expected}>
</article>";

			var @override = Optional<Func<Typo, Typo>>.Of(Override);

			using var fixture = CreateFixture(value, overrideHeaderTypo: @override);
			fixture.MarkupMatches(expectedValue);

			Typo Override(Typo x) =>
				x == Typo.h3 ? newTypo : x;
		}

		[Theory]
		[InlineData(Typo.h1, "h1")]
		[InlineData(Typo.h2, "h2")]
		[InlineData(Typo.h3, "h3")]
		[InlineData(Typo.h4, "h4")]
		[InlineData(Typo.h5, "h5")]
		[InlineData(Typo.h6, "h6")]
		public void RenderHeadersWithDifferentTypoH4(Typo newTypo, string expected)
		{
			const string value = "#### some text";

			var expectedValue =
$@"<article class='mud-markdown-body'>
	<{expected} id='some-text' class='mud-typography mud-typography-{expected} mud-inherit-text'>
		some text
	</{expected}>
</article>";

			var @override = Optional<Func<Typo, Typo>>.Of(Override);

			using var fixture = CreateFixture(value, overrideHeaderTypo: @override);
			fixture.MarkupMatches(expectedValue);

			Typo Override(Typo x) =>
				x == Typo.h4 ? newTypo : x;
		}

		[Theory]
		[InlineData(Typo.h1, "h1")]
		[InlineData(Typo.h2, "h2")]
		[InlineData(Typo.h3, "h3")]
		[InlineData(Typo.h4, "h4")]
		[InlineData(Typo.h5, "h5")]
		[InlineData(Typo.h6, "h6")]
		public void RenderHeadersWithDifferentTypoH5(Typo newTypo, string expected)
		{
			const string value = "##### some text";

			var expectedValue =
$@"<article class='mud-markdown-body'>
	<{expected} id='some-text' class='mud-typography mud-typography-{expected} mud-inherit-text'>
		some text
	</{expected}>
</article>";

			var @override = Optional<Func<Typo, Typo>>.Of(Override);

			using var fixture = CreateFixture(value, overrideHeaderTypo: @override);
			fixture.MarkupMatches(expectedValue);

			Typo Override(Typo x) =>
				x == Typo.h5 ? newTypo : x;
		}

		[Theory]
		[InlineData(Typo.h1, "h1")]
		[InlineData(Typo.h2, "h2")]
		[InlineData(Typo.h3, "h3")]
		[InlineData(Typo.h4, "h4")]
		[InlineData(Typo.h5, "h5")]
		[InlineData(Typo.h6, "h6")]
		public void RenderHeadersWithDifferentTypoH6(Typo newTypo, string expected)
		{
			const string value = "###### some text";

			var expectedValue =
$@"<article class='mud-markdown-body'>
	<{expected} id='some-text' class='mud-typography mud-typography-{expected} mud-inherit-text'>
		some text
	</{expected}>
</article>";

			var @override = Optional<Func<Typo, Typo>>.Of(Override);

			using var fixture = CreateFixture(value, overrideHeaderTypo: @override);
			fixture.MarkupMatches(expectedValue);

			Typo Override(Typo x) =>
				x == Typo.h6 ? newTypo : x;
		}
	}
}
