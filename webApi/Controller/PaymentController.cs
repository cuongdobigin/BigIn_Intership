using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;
using MyApp.Application.Interface.Service;
using MyApp.Application.Service;

namespace webApi.Controller;

[ApiController]
[Route("api/payment")]
public class PaymentController(IMoMoService moMoService) : ControllerBase
{

    [HttpPost("create/{id}")]
    public async Task<ApiResponse<MoMoResponse>> Create(int id)
    {
        string result = await moMoService.CreatePaymentAsync(id, $"Thanh toan don hang {id}");
        
        return ApiResponse<MoMoResponse>.Success(new MoMoResponse { url = result });
    }

    [HttpPost("notify")]
    public async Task<IActionResult> Notify([FromBody] MoMoNotifyDto dto)
    {
        await moMoService.UpdatePaymentAsync(dto);
        return Ok();
    }

    [HttpGet("return")]
    public IActionResult Return([FromQuery] int resultCode)
    {
        return resultCode == 0
            ? Redirect("/order/success")
            : Redirect("/order/failed");
    }
}