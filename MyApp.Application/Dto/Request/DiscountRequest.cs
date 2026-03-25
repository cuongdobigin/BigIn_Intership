using System.ComponentModel.DataAnnotations;
using MyApp.Application.Validator;

namespace MyApp.Application.Dto.Request;

public class DiscountRequest
{
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "MIN_VALUE")]
    public decimal MinApply { get; set; }

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "MAX_VALUE")]
    public decimal MaxApply { get; set; }

    [Required]
    [Range(0.01, 1, ErrorMessage = "INCORRECT_PERCENT")]
    public decimal Percent { get; set; }

    [NotPastDate(ErrorMessage = "START_DATE")]
    public DateTime StartDate { get; set; }
    
    [NotPastDate(ErrorMessage = "END_DATE")]
    public DateTime EndDate { get; set; }
}
