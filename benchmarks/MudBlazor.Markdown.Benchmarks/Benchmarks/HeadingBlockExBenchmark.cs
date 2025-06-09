using BenchmarkDotNet.Attributes;
using Markdig.Syntax;

namespace MudBlazor.Markdown.Benchmarks;

[MemoryDiagnoser]
public class HeadingBlockExBenchmark
{
	private HeadingBlock? _headingBlock;

	[GlobalSetup]
	public void Setup()
	{
		const int length = 1_000_000;
		var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ +:&".ToCharArray();

		var value = string.Create(length, chars, static (span, chars) =>
		{
			span[0] = '#';
			span[1] = ' ';

			for (var i = 2; i < length; i++)
			{
				span[i] = chars[Random.Shared.Next(chars.Length)];
			}
		});

		_headingBlock = Markdig.Markdown.Parse(value)
			.OfType<HeadingBlock>()
			.Single();
	}

	[Benchmark]
	public void Run()
	{
		if (_headingBlock is null)
			throw new InvalidOperationException("HeadingBlock is not initialized.");

		_headingBlock.BuildHeadingContent();
	}
}
