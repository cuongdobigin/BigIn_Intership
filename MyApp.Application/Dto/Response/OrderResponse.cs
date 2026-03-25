namespace MyApp.Application.Dto.Response;

public class OrderResponse
{
    public int Id { get; set; }
    public decimal Tax { get; set; }
    public decimal SubTotal { get; set; }
    public decimal PaymentTotal { get; set; }
    public string PaymentStatus { get; set; } = "";
    public decimal DiscountPercent { get; set; }
    public bool IsActive { get; set; }
    public List<ShoppingCartResponse> ShoppingCarts { get; set; } = new ();
}