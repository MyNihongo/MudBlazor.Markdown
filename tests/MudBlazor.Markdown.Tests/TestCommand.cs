using System;
using System.Windows.Input;

namespace MudBlazor.Markdown.Tests
{
	internal sealed class TestCommand : ICommand
	{
		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter) =>
			true;

		public void Execute(object parameter) =>
			Console.WriteLine("test");
	}
}
