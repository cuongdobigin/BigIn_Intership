using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using MyApp.Application.Dto.Request;
using MyApp.Application.Interface.Repository;
using MyApp.Application.Interface.Service;
using Myapp.Domain.Exceptions;
using MyApp.Domain.Config;
using Myapp.Domain.Entity;
using MyApp.Domain.Enum;

namespace MyApp.Application.Service;

public class MoMoService(IOrderRepository orderRepository, IPaymentRepository paymentRepository, HttpClient httpClient, IOptions<MoMoOptionConfig> options) : IMoMoService
{
    public async Task<string> CreatePaymentAsync(int orderId,string orderInfo)
    {
        var order = await orderRepository.GetById(orderId);
        if(order == null)
            throw  new AppException(ErrorCode.ORDER_NOT_FOUND);
        var requestId   = Guid.NewGuid().ToString();
        var extraData   = "";
        var requestType = "payWithMethod";
        orderInfo = orderInfo.Trim();
        var amount = ((long)order.PaymentTotal).ToString();
        var orderIdMoMo = $"{orderId}_{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
        var rawHash = $"accessKey={options.Value.AccessKey}" +
                      $"&amount={amount}" +
                      $"&extraData={extraData}" +
                      $"&ipnUrl={options.Value.IpnUrl}" +
                      $"&orderId={orderIdMoMo}" +
                      $"&orderInfo={orderInfo}" +
                      $"&partnerCode={options.Value.PartnerCode}" +
                      $"&redirectUrl={options.Value.RedirectUrl}" +
                      $"&requestId={requestId}" +
                      $"&requestType={requestType}";

        var signature = HmacSha256(rawHash, options.Value.SecretKey);
        
        var body = new
        {
            partnerCode = options.Value.PartnerCode,
            accessKey   = options.Value.AccessKey,
            requestId,
            amount=amount,
            orderId=orderIdMoMo,
            orderInfo,
            redirectUrl = options.Value.RedirectUrl,
            ipnUrl      = options.Value.IpnUrl,
            extraData,
            requestType,
            signature,
            lang = "vi"
        };

        var response = await httpClient.PostAsJsonAsync(options.Value.Endpoint, body);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
             throw new Exception($"MoMo API error: {response.StatusCode} - {responseContent}");
        }

        var json = JsonSerializer.Deserialize<JsonElement>(responseContent);
        
        if (json.TryGetProperty("payUrl", out var payUrl))
        {
            return payUrl.GetString() ?? "";
        }

        throw new Exception($"MoMo API success but payUrl missing. Response: {responseContent}");
    }

    public async Task UpdatePaymentAsync(MoMoNotifyDto dto)
    {
        if (dto.ResultCode != 0)
        {
            return;
        }

        var orderId = int.Parse(dto.OrderId);
        var order = await orderRepository.GetById(orderId);
        
        if (order == null)
            throw new AppException(ErrorCode.ORDER_NOT_FOUND);

        order.PaymentStatus = StatusOrder.PAID.ToString();
        await orderRepository.Update(order);

        var payment = new Payment
        {
            RequestId = dto.RequestId,
            TransId = dto.TransId,
            OrderId = orderId
        };

        await paymentRepository.Add(payment);
    }

    private string HmacSha256(string message, string key)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }
    
    
    
}