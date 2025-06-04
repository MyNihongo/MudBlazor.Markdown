[![Version](https://img.shields.io/nuget/v/MudBlazor.Markdown?style=plastic)](https://www.nuget.org/packages/MudBlazor.Markdown/)
[![Nuget downloads](https://img.shields.io/nuget/dt/MudBlazor.Markdown?label=nuget%20downloads&logo=nuget&style=plastic)](https://www.nuget.org/packages/MudBlazor.Markdown/)  
# Markdown component for [MudBlazor](https://github.com/MudBlazor/MudBlazor)

This README covers configuration steps for Blazor Server and Blazor WebAssembly. For images of how the markup component looks like in the browser go to the [README of samples](/samples).

## Update guide
For guidance with update errors please visit the [wiki page](https://github.com/MyNihongo/MudBlazor.Markdown/wiki/Update-guide).

## Getting started
> NB! MudBlazor does not work well with the static SSR format because some code is executed in `OnAfterRender` or `OnAfterRenderAsync` that is not invoked by default.  
> Specify `@rendermode="InteractiveServer"` on the markdown component to make it work (e.g. `<MudMarkdown @rendermode="InteractiveServer" Value="some markdown here" />`)

Install the NuGet package.
```
dotnet add package MudBlazor.Markdown
```
Add the following using statement in `_Imports.razor`.
```razor
@using MudBlazor
```
Add the following nodes in either `App.razor` or `MainLayout.razor`.
```razor
<MudThemeProvider />
<MudPopoverProvider />
<MudDialogProvider />
<MudSnackbarProvider />
```
Add the following nodes in `Pages/_Host.cstml` (Server) or `wwwroot/index.html` (WebAssembly).  
In the `<head>` node add these CSS stylesheets.
```html
<link href="https://fonts.googleapis.com/css?family=Roboto:300,400,500,700&display=swap" rel="stylesheet" />
<link href="_content/MudBlazor/MudBlazor.min.css" rel="stylesheet" />
<link href="_content/MudBlazor.Markdown/MudBlazor.Markdown.min.css" rel="stylesheet" />
```
At the bottom of the `<body>` node add this JS source.
```html
<script src="_content/MudBlazor/MudBlazor.min.js"></script>
<script src="_content/MudBlazor.Markdown/MudBlazor.Markdown.min.js"></script>
```
Register MudBlazor services in the DI container.  
For the Blazor Server in the `Startup.cs` add this method.
```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddMudServices();
    services.AddMudMarkdownServices();
    // Optionally if the default clipboard functionality fails it is possible to add a custom service
    // NB! MauiClipboardService is just an example
    services.AddMudMarkdownClipboardService<MauiClipboardService>();
}
```
For the Blazor WebAssembly in the `Program.cs` add this method.
```cs
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddMudServices();
builder.Services.AddMudMarkdownServices();
// Optionally if the default clipboard functionality fails it is possible to add a custom service
// NB! MauiClipboardService is just an example
builder.Services.AddMudMarkdownClipboardService<MauiClipboardService>();
```
## Using the component
```razor
<MudText Typo="Typo.h3">My markdown</MudText>
<MudMarkdown Value="@Value" />

@code
{
    private string Value { get; } = "text *italics* and **bold**";
}
```
