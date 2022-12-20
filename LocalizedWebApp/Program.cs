
using LocalizedWebApp.Controllers;
using LocalizeMessagesAndErrors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region REGISTERING .NET Localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
#endregion

#region localization - defining the cultures 
//see https://learn.microsoft.com/en-us/aspnet/core/fundamentals/localization#localization-middleware
var supportedCultures = new[] { "en", "fr" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
#endregion

#region REGISTERING Net.LocalizeMessagesAndError
//This registers the DefaultLocalizer with the culture that the messages will use
builder.Services.RegisterDefaultLocalizer("en", supportedCultures);
//This registers the SimpleLocalizer, which is useful for simple localizations,
//such as messages in your views/pages.
builder.Services.RegisterSimpleLocalizer<HomeController>();
#endregion

var app = builder.Build();

#region SETUP locatization

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
