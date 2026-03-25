namespace MyApp.Application.Dto.Request;

public class OrderRequest
{
    public List<int> IdShoppingCart { get; set; } = new ();
    public int? DiscountId { get; set; }
}