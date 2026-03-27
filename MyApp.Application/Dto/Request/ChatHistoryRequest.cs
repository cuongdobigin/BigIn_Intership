namespace MyApp.Application.Dto.Request;

public class ChatHistoryRequest
{
    public string CurrentMessage { get; set; } = "";
    public List<MessageDto> History { get; set; } = new();
}