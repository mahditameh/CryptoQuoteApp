using CryptoQuoteApp.Helpers.Middleware;
using InfrastructureConfig;
using Serilog;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// Registering HttpClient
builder.Services.AddHttpClient(); // Register HttpClient

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var isDevelopment = environment == Environments.Development;

var configurationBuilder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();//we can set the keys in Environment variable too for more securities

var configuration = configurationBuilder.Build();
var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .CreateLogger();
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
builder.Host.UseSerilog((ctx, lc)
              => lc.ReadFrom.Configuration(ctx.Configuration));
builder.Logging.AddSerilog(logger);
var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
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