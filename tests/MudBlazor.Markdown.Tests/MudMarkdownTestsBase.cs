using System;
using System.Windows.Input;
using Bunit;

namespace MudBlazor.Markdown.Tests
{
	public abstract class MudMarkdownTestsBase : IDisposable
	{
		private readonly TestContext _ctx = new();

		protected IRenderedComponent<MudMarkdown> CreateFixture(string value, ICommand command = null, int? tableCellMinWidth = null) =>
			_ctx.RenderComponent<MudMarkdown>(@params =>
				@params.Add(static x => x.Value, value)
					.Add(static x => x.LinkCommand, command)
					.Add(static x => x.TableCellMinWidth, tableCellMinWidth));

		public void Dispose()
		{
			GC.SuppressFinalize(this);
			_ctx.Dispose();
		}
	}
}
