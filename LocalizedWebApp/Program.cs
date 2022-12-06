
using LocalizeMessagesAndErrors;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region REGISTERING .NET Localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
#endregion

#region REGISTERING Net.LocalizeMessagesAndError
builder.Services.AddSingleton(typeof(ILocalizeWithDefault<>), typeof(LocalizeWithDefault<>));
#endregion

var app = builder.Build();

#region SETUP
var supportedCultures = new[] { "en", "fr" };
var localizationOptions =
    new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);

localizationOptions.ApplyCurrentCultureToResponseHeaders = true;

app.UseRequestLocalization(localizationOptions);
#endregion

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
