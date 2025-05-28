namespace MudBlazor;

internal sealed class MudMarkdownHeadingTree
{
	private readonly List<Item> _items = [];
	private MudTableOfContentsNavMenu? _mudTableOfContentsNavMenu;

	public bool Append(in Typo typo, in HeadingContent? content)
	{
		if (typo < Typo.h1 || typo > Typo.h3 || content is null)
			return false;

		var newItem = new Item(typo, content.Value);
		_items.Add(newItem);

		_mudTableOfContentsNavMenu?.InvokeRenderNavMenu(_items);
		return true;
	}

	public void SetNavMenuReference(in MudTableOfContentsNavMenu mudTableOfContentsNavMenu)
	{
		if (mudTableOfContentsNavMenu == _mudTableOfContentsNavMenu)
			return;

		_mudTableOfContentsNavMenu = mudTableOfContentsNavMenu;
		_mudTableOfContentsNavMenu.InvokeRenderNavMenu(_items);
	}

	public sealed class Item
	{
		public readonly Typo Typo;
		public readonly string Id, Text;

		public Item(in Typo typo, in HeadingContent content)
		{
			Id = content.Id;
			Text = content.Text;
			Typo = typo;
		}

		public override string ToString() =>
			$"`Typo`:`{Typo}`,`Id`:`{Id}`,`Text`:`{Text}`";
	}
}
