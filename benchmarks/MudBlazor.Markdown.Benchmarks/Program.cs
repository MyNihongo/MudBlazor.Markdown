using BenchmarkDotNet.Running;

namespace MudBlazor.Markdown.Benchmarks;

internal static class Program
{
	private static void Main()
	{
		BenchmarkRunner.Run<HeadingBlockExBenchmark>();
	}
}
