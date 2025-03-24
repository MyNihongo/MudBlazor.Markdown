namespace MudBlazor;

internal interface IMudMarkdownValueProvider
{
    ValueTask<string> GetValueAsync(string value, MarkdownSourceType sourceType, CancellationToken ct = default);
}
