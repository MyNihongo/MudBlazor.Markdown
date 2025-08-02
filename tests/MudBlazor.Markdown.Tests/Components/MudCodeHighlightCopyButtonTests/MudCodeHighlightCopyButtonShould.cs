namespace MudBlazor.Markdown.Tests.Components.MudCodeHighlightCopyButtonTests;

public sealed class MudCodeHighlightCopyButtonShould : MudCodeHighlightCopyButtonTestsBase
{
	[Fact]
	public void RenderComponentForCopied()
	{
		const string expected =
			"""
			<div class="mud-tooltip-root mud-tooltip-inline">
				<button  type="button" class="mud-button-root mud-icon-button mud-button mud-button-filled mud-button-filled-success mud-button-filled-size-medium mud-ripple"  >
					<span class="mud-icon-button-label">
						<svg class="mud-icon-root mud-svg-icon mud-icon-size-medium" focusable="false" viewBox="0 0 24 24" aria-hidden="true" role="img">
							<path d="M0 0h24v24H0V0z" fill="none"></path>
							<path d="M17.3 6.3c-.39-.39-1.02-.39-1.41 0l-5.64 5.64 1.41 1.41L17.3 7.7c.38-.38.38-1.02 0-1.4zm4.24-.01l-9.88 9.88-3.48-3.47c-.39-.39-1.02-.39-1.41 0-.39.39-.39 1.02 0 1.41l4.18 4.18c.39.39 1.02.39 1.41 0L22.95 7.71c.39-.39.39-1.02 0-1.41h-.01c-.38-.4-1.01-.4-1.4-.01zM1.12 14.12L5.3 18.3c.39.39 1.02.39 1.41 0l.7-.7-4.88-4.9c-.39-.39-1.02-.39-1.41 0-.39.39-.39 1.03 0 1.42z"></path>
						</svg>
					</span>
				</button>
				<div id:ignore class="mud-popover-cascading-value"></div>
			</div>
			""";

		using var fixture = CreateFixture();
		fixture.Instance.SetIsCopied(true);
		fixture.Render();

		fixture.MarkupMatches(expected);
	}
}

file static class MudCodeHighlightCopyButtonEx
{
	public static void SetIsCopied(this MudCodeHighlightCopyButton @this, bool isCopied)
	{
		const string fieldName = "_isCopied";

		var isCopiedField = @this.GetType()
			.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);

		if (isCopiedField is null)
			throw new MissingFieldException(fieldName);

		isCopiedField.SetValue(@this, isCopied);
	}
}
