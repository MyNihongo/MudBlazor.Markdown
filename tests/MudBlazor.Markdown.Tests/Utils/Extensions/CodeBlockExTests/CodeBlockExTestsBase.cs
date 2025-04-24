using System.Text;

namespace MudBlazor.Markdown.Tests.Utils.Extensions.CodeBlockExTests;

public abstract class CodeBlockExTestsBase
{
	protected static CodeBlock CreateFixture(string code)
	{
		code = new StringBuilder()
			.AppendLine("```")
			.AppendLine(code)
			.Append("```")
			.ToString();

		var document = Markdig.Markdown.Parse(code);
		return document.OfType<CodeBlock>().Single();
	}
}
