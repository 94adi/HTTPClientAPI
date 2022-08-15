using HTTPClientAPI.Models.Config;
using HTTPClientAPI.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<YoutubeOptions>(
    builder.Configuration.GetSection(YoutubeOptions.Option));
builder.Services.AddHttpClient();
builder.Services.AddScoped<IYoutubeService, YoutubeService>();
builder.Services.AddScoped<IWikipediaService, WikipediaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
