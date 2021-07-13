using System;
using Bunit;

namespace MudBlazor.Markdown.Tests
{
	public abstract class MudMarkdownTestsBase : IDisposable
	{
		private readonly TestContext _ctx = new();

		protected IRenderedComponent<MudMarkdown> CreateFixture(string value) =>
			_ctx.RenderComponent<MudMarkdown>(@params => @params.Add(static x => x.Value, value));

		public void Dispose()
		{
			GC.SuppressFinalize(this);
			_ctx.Dispose();
		}
	}
}
