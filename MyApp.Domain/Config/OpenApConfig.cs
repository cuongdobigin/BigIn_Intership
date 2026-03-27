namespace MyApp.Domain.Config;

public class OpenAiConfig
{
    public const string SectionName = "OpenAI";
    public string ApiKey { get; set; } = "";
    public string Model { get; set; }
    public string BaseUrl { get; set; } = "";
}