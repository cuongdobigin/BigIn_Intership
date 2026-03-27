using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;

namespace MyApp.Application.Interface.Service;

public interface IChatService
{
    Task<ChatResponse> ChatAsync(string userQuestion, List<MessageDto> history);
}