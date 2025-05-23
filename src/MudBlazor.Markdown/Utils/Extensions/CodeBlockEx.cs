﻿using System.Text;
using Markdig.Syntax;

namespace MudBlazor;

internal static class CodeBlockEx
{
	public static string CreateCodeBlockText(this CodeBlock @this)
	{
		if (@this.Lines.Count == 0)
			return string.Empty;

		var sb = new StringBuilder();

		foreach (var line in @this.Lines)
		{
			var str = line.ToString();

			if (string.IsNullOrEmpty(str))
			{
				var emptyLineString = (line as StringLine?)?.NewLine.AsString();
				sb.Append(emptyLineString);
				continue;
			}

			if (sb.Length != 0)
				sb.AppendLine();

			sb.Append(str);
		}

		return sb.ToString();
	}
}
