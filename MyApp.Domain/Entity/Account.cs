using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Myapp.Domain.Entity;

namespace Myapp.Domain.Entity
{
    
}
[Table("Account")]
public class Account
{
    [Column("Id")]
    public int Id { get; set; }
    [Column("username")]
    public string username { get; set; }
    [Column("password")]
    public string password { get; set; }

    [Column("is_active")] 
    public Boolean isActive { get; set; } = true;

    [Column("is_first_time")]
    public Boolean isFirstTime { get; set; } = true;
    [Column("createdAt")]
    public DateTime CreatedAt { get; set; }= DateTime.UtcNow;
    [Column("updatedAt")]
    public DateTime UpdatedAt { get; set; }= DateTime.UtcNow;
    public ICollection<Role> Roles { get; set; } = new List<Role>();
    public User? User { get; set; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ShoppingCart? ShoppingCart { get; set; } 
    
}