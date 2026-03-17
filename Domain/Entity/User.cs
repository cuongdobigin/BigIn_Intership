namespace webApi.Domain.Entity;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Address { get; set; } = "";
    
    public Account? Account { get; set; }
}