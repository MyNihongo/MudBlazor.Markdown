using System;
using System.Windows.Input;
using Bunit;

namespace MudBlazor.Markdown.Tests
{
	public abstract class MudMarkdownTestsBase : IDisposable
	{
		private readonly TestContext _ctx = new();

		protected IRenderedComponent<MudMarkdown> CreateFixture(string value, ICommand command = null, int? tableCellMinWidth = null,
			Typo h1Typo = Typo.h1, Typo h2Typo = Typo.h2, Typo h3Typo = Typo.h3, Typo h4Typo = Typo.h4, Typo h5Typo = Typo.h5,
			Typo h6Typo = Typo.h6) =>
			_ctx.RenderComponent<MudMarkdown>(@params =>
				@params.Add(static x => x.Value, value)
					.Add(static x => x.LinkCommand, command)
					.Add(static x => x.TableCellMinWidth, tableCellMinWidth)
					.Add(static x => x.H1Typo, h1Typo)
					.Add(static x => x.H2Typo, h2Typo)
					.Add(static x => x.H3Typo, h3Typo)
					.Add(static x => x.H4Typo, h4Typo)
					.Add(static x => x.H5Typo, h5Typo)
					.Add(static x => x.H6Typo, h6Typo));

		public void Dispose()
		{
			GC.SuppressFinalize(this);
			_ctx.Dispose();
		}
	}
}
