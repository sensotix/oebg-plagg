using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using OEBG.PLAGG.Components;
using OEBG.PLAGG.Data;
using OEBG.PLAGG.Security;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options => {
    options.ConfigureEndpointDefaults(listenOptions => {
        listenOptions.Protocols = HttpProtocols.Http1;
    });
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();

var useOfflineDevAuth = builder.Environment.IsDevelopment() &&
    builder.Configuration.GetValue<bool>("Authentication:UseOfflineDevAuth");

if (useOfflineDevAuth) {
    builder.Services.AddAuthentication("DevAuth")
        .AddScheme<AuthenticationSchemeOptions, DevAuthenticationHandler>("DevAuth", _ => { });
}
else {
    // Windows Authentication per Microsoft docs.
    builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
        .AddNegotiate();
}

builder.Services.AddAuthorization();

var titlesConnectionString = builder.Configuration.GetConnectionString("TitlesConnection") ?? throw new InvalidOperationException("Connection string 'TitlesConnection' not found.");
builder.Services.AddDbContextFactory<TitlesDbContext>(options =>
    options.UseSqlServer(titlesConnectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets().AllowAnonymous();
app.MapGet("/favicon.ico", () => Results.Redirect("/favicon.png", permanent: false)).AllowAnonymous();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .RequireAuthorization();

app.Run();
