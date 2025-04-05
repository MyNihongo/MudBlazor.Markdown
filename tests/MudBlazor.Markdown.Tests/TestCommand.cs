namespace MudBlazor.Markdown.Tests;

internal sealed class TestCommand : ICommand
{
#pragma warning disable 67
	public event EventHandler? CanExecuteChanged;
#pragma warning restore 67

	public bool CanExecute(object? parameter) =>
		true;

	public void Execute(object? parameter) =>
		Console.WriteLine("test");
}
