using System.ComponentModel.DataAnnotations;

namespace MyApp.Application.Validator;

public class NotPastDateAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is DateTime date)
        {
            return date.Date >= DateTime.Today;
        }
        return false;
    }
}