using System.Text;

namespace MudBlazor.Markdown.Tests;

public static class StringEx
{
	public static string EscapePath(this string @this)
	{
		var root = Path.GetPathRoot(Environment.CurrentDirectory) ?? throw new Exception($"Cannot get the root path, string=`{@this}`");

		// On linux the solution is build in "/home"
		if (root == "/")
			root += "home";

		const StringComparison stringComparison = StringComparison.OrdinalIgnoreCase;
		var stringBuilder = new StringBuilder(@this.Length);
		var span = @this.AsSpan();

		int endIndex, startIndex = 0;
		while ((endIndex = @this.IndexOf(root, startIndex, stringComparison)) >= 0)
		{
			stringBuilder
				.Append(span[startIndex..endIndex])
				.Append("path");

			const string extension = ".md";
			startIndex = @this.IndexOf(extension, endIndex, stringComparison);
			if (startIndex == -1)
				throw new Exception($"Cannot find the end of the path, string=`{@this}`, index=`{endIndex}`");

			startIndex += extension.Length;
		}

		return stringBuilder
			.Append(span[startIndex..])
			.ToString();
	}
}
