using webApi.Domain.Entity;

namespace webApi.Dto.Response;

public class UserResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string usernameAccount { get; set; }
}