using Markdig.Syntax.Inlines;

namespace MudBlazor.Markdown.Tests.Components.MarkdownComponentTests;

public sealed class MarkdownComponentPropShould : MarkdownComponentTestsBase
{
	[Theory]
	[InlineData(Typo.h1, "h1")]
	[InlineData(Typo.h2, "h2")]
	[InlineData(Typo.h3, "h3")]
	[InlineData(Typo.h4, "h4")]
	[InlineData(Typo.h5, "h5")]
	[InlineData(Typo.h6, "h6")]
	public void RenderHeadersWithDifferentTypoH1(Typo newTypo, string expectedTag)
	{
		const string value = "# some text";

		var expected =
			$"""
			 <article id:ignore class='mud-markdown-body'>
			 	<{expectedTag} id='some-text' class='mud-typography mud-typography-{expectedTag}'>
			 		some text
			 	</{expectedTag}>
			 </article>
			 """;

		var props = new MudMarkdownProps
		{
			Heading =
			{
				OverrideTypo = Override,
			},
		};

		using var fixture = CreateFixture(value, props: props);
		fixture.MarkupMatches(expected);
		return;

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
	public void RenderHeadersWithDifferentTypoH2(Typo newTypo, string expectedTag)
	{
		const string value = "## some text";

		var expected =
			$"""
			 <article id:ignore class='mud-markdown-body'>
			 	<{expectedTag} id='some-text' class='mud-typography mud-typography-{expectedTag}'>
			 		some text
			 	</{expectedTag}>
			 </article>
			 """;

		var props = new MudMarkdownProps
		{
			Heading =
			{
				OverrideTypo = Override,
			},
		};

		using var fixture = CreateFixture(value, props: props);
		fixture.MarkupMatches(expected);
		return;

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
	public void RenderHeadersWithDifferentTypoH3(Typo newTypo, string expectedTag)
	{
		const string value = "### some text";

		var expected =
			$"""
			 <article id:ignore class='mud-markdown-body'>
			 	<{expectedTag} id='some-text' class='mud-typography mud-typography-{expectedTag}'>
			 		some text
			 	</{expectedTag}>
			 </article>
			 """;

		var props = new MudMarkdownProps
		{
			Heading =
			{
				OverrideTypo = Override,
			},
		};

		using var fixture = CreateFixture(value, props: props);
		fixture.MarkupMatches(expected);
		return;

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
	public void RenderHeadersWithDifferentTypoH4(Typo newTypo, string expectedTag)
	{
		const string value = "#### some text";

		var expected =
			$"""
			 <article id:ignore class='mud-markdown-body'>
			 	<{expectedTag} id='some-text' class='mud-typography mud-typography-{expectedTag}'>
			 		some text
			 	</{expectedTag}>
			 </article>
			 """;

		var props = new MudMarkdownProps
		{
			Heading =
			{
				OverrideTypo = Override,
			},
		};

		using var fixture = CreateFixture(value, props: props);
		fixture.MarkupMatches(expected);
		return;

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
	public void RenderHeadersWithDifferentTypoH5(Typo newTypo, string expectedTag)
	{
		const string value = "##### some text";

		var expected =
			$"""
			 <article id:ignore class='mud-markdown-body'>
			 	<{expectedTag} id='some-text' class='mud-typography mud-typography-{expectedTag}'>
			 		some text
			 	</{expectedTag}>
			 </article>
			 """;

		var props = new MudMarkdownProps
		{
			Heading =
			{
				OverrideTypo = Override,
			},
		};

		using var fixture = CreateFixture(value, props: props);
		fixture.MarkupMatches(expected);
		return;

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
	public void RenderHeadersWithDifferentTypoH6(Typo newTypo, string expectedTag)
	{
		const string value = "###### some text";

		var expected =
			$"""
			 <article id:ignore class='mud-markdown-body'>
			 	<{expectedTag} id='some-text' class='mud-typography mud-typography-{expectedTag}'>
			 		some text
			 	</{expectedTag}>
			 </article>
			 """;

		var props = new MudMarkdownProps
		{
			Heading =
			{
				OverrideTypo = Override,
			},
		};

		using var fixture = CreateFixture(value, props: props);
		fixture.MarkupMatches(expected);
		return;

		Typo Override(Typo x) =>
			x == Typo.h6 ? newTypo : x;
	}

	[Fact]
	public void OverrideAllLinks()
	{
		const string value =
			"""
			[absolute](https://www.google.co.jp/)
			[relative](/tokyo)
			[id](#edogawa)
			""";

		const string expected =
			"""
			<article id:ignore class='mud-markdown-body'>
				<p class='mud-typography mud-typography-body1'>
					<a rel='noopener noreferrer' href='overriddenhttps://www.google.co.jp/' target='_blank' class='mud-typography mud-link mud-primary-text mud-link-underline-hover mud-typography-body1'>
						absolute
					</a>
					<a href='overridden/tokyo' class='mud-typography mud-link mud-primary-text mud-link-underline-hover mud-typography-body1'>
						relative
					</a>
					<a href='overridden#edogawa' class='mud-typography mud-link mud-primary-text mud-link-underline-hover mud-typography-body1'>
						id
					</a>
				</p>
			</article>
			""";

		var props = new MudMarkdownProps
		{
			Link =
			{
				OverrideUrl = Override,
			},
		};

		using var fixture = CreateFixture(value, props: props);
		fixture.MarkupMatches(expected);
		return;

		static string Override(LinkInline x) =>
			"overridden" + x.Url;
	}

	[Fact]
	public void OverrideImageLink()
	{
		const string value = @"![img](/tokyo/sky-tree.png)";
		const string expected =
			"""
			<article id:ignore class='mud-markdown-body'>
				<p class='mud-typography mud-typography-body1'>
					<img src='overridden/tokyo/sky-tree.png' alt='img' class='mud-image object-fill object-center mud-elevation-25 rounded-lg'>
				</p>
			</article>
			""";

		var props = new MudMarkdownProps
		{
			Link =
			{
				OverrideUrl = Override,
			},
		};

		using var fixture = CreateFixture(value, props: props);
		fixture.MarkupMatches(expected);
		return;

		static string Override(LinkInline x) =>
			"overridden" + x.Url;
	}

	[Fact]
	public void RenderLinkAsButton()
	{
		const string value = "text before [link display](123) text after";
		const string expected =
			"""
			<article id:ignore class='mud-markdown-body'>
				<p class='mud-typography mud-typography-body1'>
					text before
					<span class='mud-typography mud-link mud-primary-text mud-link-underline-hover mud-typography-body1'>link display</span>
					text after
				</p>
			</article>
			""";

		var props = new MudMarkdownProps
		{
			Link =
			{
				Command = new TestCommand(),
			},
		};

		using var fixture = CreateFixture(value, props: props);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderExternalLinkWithoutTarget()
	{
		const string value = "[link display](https://mudblazor.com/)";
		const string expected =
			"""
			<article id:ignore class="mud-markdown-body">
				<p class="mud-typography mud-typography-body1">
					<a rel="noopener noreferrer" href="https://mudblazor.com/" class="mud-typography mud-link mud-primary-text mud-link-underline-hover mud-typography-body1">
						link display
					</a>
				</p>
			</article>
			""";

		var props = new MudMarkdownProps
		{
			Link =
			{
				DisableTargetBlank = true,
			},
		};

		using var fixture = CreateFixture(value, props: props);
		fixture.MarkupMatches(expected);
	}
}
