namespace MyApp.Application.Dto.Request;

public class TypeBookUpdateRequest
{
    public string Name { get; set; }
    
    public string Description { get; set; }
    public bool IsActive { get; set; } 
}