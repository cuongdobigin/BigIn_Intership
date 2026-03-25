namespace MyApp.Application.Dto.Response;

public class DiscountResponse
{
    public int Id { get; set; }
    public decimal MinApply { get; set; }
    public decimal MaxApply { get; set; }
    public decimal Percent { get; set; }
    public bool IsActive { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
