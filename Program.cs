using System.Reflection;
using Microsoft.EntityFrameworkCore;
using webApi.configuration;
using webApi.Infrastructure.Persistence;
using webApi.Infrastructure.Persistence.Repository;
using webApi.Infrastructure.Middleware;
using webApi.Mapper;
using webApi.Repository;
using webApi.Service;
using webApi.Service.Interface;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddValidationResponse();
builder.Services.AddCorsPolicy(); 
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<IAuthenticateService, AuthenticateService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ITypeBookService, TypeBookService>();    
builder.Services.AddScoped<ITypeBookRepository, TypeBookRepository>(); 
builder.Services.AddAutoMapper(cfg => {}, Assembly.GetExecutingAssembly());
var app = builder.Build();

await ApplicationInitConfig.InitializeAsync(app.Services);

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowVueDev"); 
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();