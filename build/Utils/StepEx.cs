using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MudBlazor.Markdown.Build.Models;
using MudBlazor.Markdown.Build.Steps.Interfaces;

namespace MudBlazor.Markdown.Build.Utils
{
	internal static class StepEx
	{
		public static async Task ProcessAsync(this IStep[] @this, ProjectDirs dirs)
		{
			await @this.ProcessAsync(dirs.CodeStyleDir, dirs)
				.ConfigureAwait(false);

			foreach (var dir in Directory.EnumerateDirectories(dirs.CodeStyleDir))
				await @this.ProcessAsync(dir, dirs)
					.ConfigureAwait(false);

			for (var i = 0; i < @this.Length; i++)
			{
				await @this[i].CompleteAsync(dirs)
					.ConfigureAwait(false);

				Console.WriteLine("Finished: {0}", @this[i].GetType().Name);
			}
		}

		private static async Task ProcessAsync(this IReadOnlyList<IStep> @this, string path, ProjectDirs dirs)
		{
			// hack??
			path = "/" + path;

			foreach (var file in Directory.EnumerateFiles(path))
				for (var i = 0; i < @this.Count; i++)
					await @this[i].ProcessFileAsync(file, dirs)
						.ConfigureAwait(false);
		}
	}
}
