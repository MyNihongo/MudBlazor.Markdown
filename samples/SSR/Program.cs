using MudBlazor;
using MudBlazor.Markdown.Core.Utils.ServiceRegistration;
using MudBlazor.Markdown.SSR.Components;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCoreServices();
builder.Services.AddMudServices();
builder.Services.AddMudMarkdownServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
