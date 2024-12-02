using CryptoQuoteApp.Helpers.Middleware;
using Domain.Services;
using Infrastructure.Validators;
using InfrastructureConfig;
using Serilog;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// Registering HttpClient
builder.Services.AddHttpClient();

// Setting up environment and configuration
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var isDevelopment = environment == Environments.Development;

var configurationBuilder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

var configuration = configurationBuilder.Build();
var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

// Adding services
builder.Services.AddControllers();
builder.Services.RegisterRepositories(configuration);
builder.Services.RegisterServices(configuration);

// Register CryptoValidator with DI
builder.Services.AddScoped<ICryptoValidator, CryptoValidator>();
builder.Services.AddMemoryCache();
// Preload CryptoCache
var serviceProvider = builder.Services.BuildServiceProvider();
var cryptoValidator = serviceProvider.GetRequiredService<ICryptoValidator>();
var cryptoMap = await cryptoValidator.GetCryptoMapAsync(); // Preload the cache

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
builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));
builder.Logging.AddSerilog(logger);

// Build application
var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline
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
