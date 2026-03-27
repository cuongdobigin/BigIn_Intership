using System.ClientModel;
using MyApp.Application.Interface.Repository;
using MyApp.Application.Interface.Service;
using MyApp.Application.Service;
using MyApp.Domain.Config;
using OpenAI;
using webApi.Repository;
using webApi.Service;

namespace webApi.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, OpenAiConfig openAiConfig)
    {
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<PasswordService>();
        services.AddScoped<IAuthenticateService, AuthenticateService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<ITypeBookService, TypeBookService>();
        services.AddScoped<ITypeBookRepository, TypeBookRepository>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IImageService, ImageService>();
        services.AddScoped<IImageRepository, ImageRepository>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IShoppingCartService, ShoppingCartService>();
        services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
        services.AddScoped<IDiscountService, DiscountService>();
        services.AddScoped<IDiscountRepository, DiscountRepository>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddHttpClient<IMoMoService, MoMoService>();
        services.AddSingleton(openAiConfig);
        services.AddSingleton(new OpenAIClient(
            new ApiKeyCredential(openAiConfig.ApiKey),
            new OpenAIClientOptions
            {
                Endpoint = new Uri(openAiConfig.BaseUrl)
            }
        ));
        services.AddScoped<IChatService, ChatService>();
        return services;
    }
}