using webApi.configuration;
using webApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container from extensions
builder.Services.AddAppConfig(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplicationServices();

// Add builtin standard services
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Seed data
await ApplicationInitConfig.InitializeAsync(app.Services);

// Configure the HTTP request pipeline from extension
app.UseAppPipeline();

app.Run();