
using LocalizedWebApp.Controllers;
using LocalizeMessagesAndErrors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region REGISTERING .NET Localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
#endregion

#region REGISTERING Net.LocalizeMessagesAndError
//This version registers both the DefaultLocalizer and the SimpleLocalizer
builder.Services.RegisterLocalizeDefault(options => options.DefaultCulture = "en");
builder.Services.RegisterSimpleLocalizer<HomeController>(options => options.DefaultCulture = "en");
#endregion

var app = builder.Build();

#region SETUP locatization
var supportedCultures = new[] { "en", "fr" };
var localizationOptions =
    new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);

//This allows one action to pass on the culture to the next action 
//This is needed when using the QueryStringRequestCultureProvider in an ASP.NET Core MVC app.
//thanks to https://www.codeproject.com/Articles/5324504/Localization-in-ASP-NET-Core-Web-API
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
