﻿using System;
using System.Windows.Input;
using Bunit;
using MyNihongo.Option;

namespace MudBlazor.Markdown.Tests
{
	public abstract class MudMarkdownTestsBase : IDisposable
	{
		private readonly TestContext _ctx = new();

		protected IRenderedComponent<MudMarkdown> CreateFixture(
			string value,
			Optional<ICommand> command = default, Optional<int?> tableCellMinWidth = default, string relativeLinkPrefix = default,
			Optional<Typo> h1Typo = default, Optional<Typo> h2Typo = default, Optional<Typo> h3Typo = default, Optional<Typo> h4Typo = default, Optional<Typo> h5Typo = default, Optional<Typo> h6Typo = default)
		{
			return _ctx.RenderComponent<MudMarkdown>(@params =>
				@params.Add(static x => x.Value, value)
					.TryAdd(static x => x.LinkCommand, command)
					.TryAdd(static x => x.TableCellMinWidth, tableCellMinWidth)
					.TryAdd(static x => x.RelativeLinkPrefix, relativeLinkPrefix)
					.TryAdd(static x => x.H1Typo, h1Typo)
					.TryAdd(static x => x.H2Typo, h2Typo)
					.TryAdd(static x => x.H3Typo, h3Typo)
					.TryAdd(static x => x.H4Typo, h4Typo)
					.TryAdd(static x => x.H5Typo, h5Typo)
					.TryAdd(static x => x.H6Typo, h6Typo));
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
			_ctx.Dispose();
		}
	}
}
