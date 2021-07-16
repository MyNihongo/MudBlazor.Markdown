using System;
using System.Windows.Input;
using Bunit;

namespace MudBlazor.Markdown.Tests
{
	public abstract class MudMarkdownTestsBase : IDisposable
	{
		private readonly TestContext _ctx = new();

		protected IRenderedComponent<MudMarkdown> CreateFixture(string value, ICommand command = null) =>
			_ctx.RenderComponent<MudMarkdown>(@params =>
				@params.Add(static x => x.Value, value)
					.Add(static x => x.LinkCommand, command));

		public void Dispose()
		{
			GC.SuppressFinalize(this);
			_ctx.Dispose();
		}
	}
}
