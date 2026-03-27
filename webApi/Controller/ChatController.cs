using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;
using MyApp.Application.Interface.Service;

namespace webApi.Controller;
[Authorize]
[ApiController]
[Route("api/chat")]
public class ChatController(IChatService chatService) :ControllerBase
{
    [HttpPost]
    public async Task<ApiResponse<ChatResponse>> ChatAsync([FromBody] ChatHistoryRequest request)
    {
        var reponse = await chatService.ChatAsync(request.CurrentMessage, request.History);
        return ApiResponse<ChatResponse>.Success(reponse);
    }
}