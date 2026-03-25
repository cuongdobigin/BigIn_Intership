using MyApp.Application.Dto.Request;

namespace MyApp.Application.Interface.Service;

public interface IMoMoService
{
    Task<string> CreatePaymentAsync(int orderId, string orderInfo);
    Task UpdatePaymentAsync(MoMoNotifyDto dto);
}