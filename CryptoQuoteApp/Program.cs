using Applications;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Registering HttpClient
builder.Services.AddHttpClient(); // Register HttpClient

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var isDevelopment = environment == Environments.Development;

var configurationBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddJsonFile("serilog.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"serilog.{environment}.json", optional: true, reloadOnChange: true);

var configuration = configurationBuilder.Build();

// Adding services
builder.Services.AddControllers();
builder.Services.RegisterRepositories(configuration);
builder.Services.RegisterServices(configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Crypto}/{action=Index}/{id?}");

app.Run();