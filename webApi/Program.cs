using MyApp.Domain.Config;
using webApi.configuration;
using webApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container from extensions
builder.Services.AddAppConfig(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
var openAiConfig = builder.Configuration
    .GetSection(OpenAiConfig.SectionName)
    .Get<OpenAiConfig>()!;
builder.Services.AddApplicationServices(openAiConfig);

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