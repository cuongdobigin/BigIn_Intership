using Myapp.Domain.Entity;

namespace MyApp.Application.Interface.Repository;

public interface IPaymentRepository
{
    Task Add(Payment payment);
}
