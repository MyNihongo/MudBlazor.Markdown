using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace MudBlazor.Markdown.Server.Pages;

[IgnoreAntiforgeryToken, ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
public class ErrorModel : PageModel
{
	private readonly ILogger<ErrorModel> _logger;
	
	public ErrorModel(ILogger<ErrorModel> logger)
	{
		_logger = logger;
	}

	public string RequestId { get; set; } = string.Empty;

	public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

	public void OnGet()
	{
		RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
	}
}
