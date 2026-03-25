using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;
using MyApp.Application.Interface.Repository;
using MyApp.Application.Interface.Service;
using Myapp.Domain.Entity;
using MyApp.Domain.Enum;
using Myapp.Domain.Exceptions;

namespace MyApp.Application.Service;

public class OrderService(
    IOrderRepository orderRepository,
    IShoppingCartRepository cartRepository,
    IDiscountRepository discountRepository,
    IAccountRepository accountRepository,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor,IShoppingCartRepository shoppingCartRepository) : IOrderService
{
    private string GetUsername()
    {
        return httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value
               ?? throw new AppException(ErrorCode.UNAUTHORIZED);
    }

    public async Task<OrderResponse> CreateOrder(OrderRequest request)
    {
        var username = GetUsername();
        var account = await accountRepository.FindByUsernameAsync(username)
                      ?? throw new AppException(ErrorCode.ACCOUNT_NOT_FOUND);

        if (request.IdShoppingCart == null || !request.IdShoppingCart.Any())
            throw new ArgumentException("Giỏ hàng trống");
        var carts = new List<ShoppingCart>();
        foreach (var cartId in request.IdShoppingCart)
        {
            var cart = await cartRepository.GetById(cartId)
                       ?? throw new AppException(ErrorCode.CART_NOT_FOUND);

            if (cart.Account.username != username || !cart.isActive)
                throw new ArgumentException($"Cart item {cartId} is invalid or belongs to another user.");
            
            if (cart.Books.quantity < cart.Amount)
                throw new AppException(ErrorCode.INSUFFICIENT_QUANTITY);
            carts.Add(cart);
        }
        
        decimal subTotal = carts.Sum(c => c.Amount * c.Books.Price);
        decimal paymentTotal = subTotal;
        Discount? appliedDiscount = null;
        // 4. Kiểm tra Discount
        if (request.DiscountId.HasValue)
        {
            var discount = await discountRepository.GetById(request.DiscountId.Value)
                           ?? throw new AppException(ErrorCode.DISCOUNT_NOT_FOUND);
            var now = DateTime.UtcNow;
            
            if (!discount.IsActive || now < discount.StartDate || now > discount.EndDate)
                throw new ArgumentException("Mã giảm giá đã hết hạn hoặc không có hiệu lực.");
            Console.Write("max:"+discount.MaxApply);
            Console.Write("min:"+discount.MinApply);
            Console.Write("subTotal:"+subTotal);
            if (subTotal < discount.MinApply || subTotal > discount.MaxApply)
                throw new ArgumentException("Đơn hàng không đủ điều kiện áp dụng mã giảm giá này.");
            appliedDiscount = discount;
            decimal discountAmount = subTotal * (discount.Percent / 100m);
            paymentTotal = subTotal - discountAmount;
        }


        var order = new Order
        {
            Account = account,
            SubTotal = subTotal,
            PaymentTotal = paymentTotal,
            Tax = 0, 
            PaymentStatus =StatusOrder.PENDING.ToString(),
            IsActive = true,
            Discount = appliedDiscount,
            ShoppingCarts = carts
        };
        
        

        await orderRepository.Create(order);
        foreach (var cart in carts)
        {
            cart.isActive = false;
            cart.Order = order;
            await shoppingCartRepository.Update(cart);
        }

        return mapper.Map<OrderResponse>(order);
    }

    public async Task CancelOrder(int id)
    {
        var username = GetUsername();
        var order = await orderRepository.GetById(id)
                    ?? throw new AppException(ErrorCode.ORDER_NOT_FOUND);

        if (order.Account.username != username)
            throw new AppException(ErrorCode.UNAUTHORIZED);
        
        order.IsActive = false;
        await orderRepository.Update(order);
    }

    public async Task<OrderResponse> GetOrderById(int id)
    {
        var username = GetUsername();
        var order = await orderRepository.GetById(id)
                    ?? throw new AppException(ErrorCode.ORDER_NOT_FOUND);

        if (order.Account.username != username)
            throw new AppException(ErrorCode.UNAUTHORIZED);

        return mapper.Map<OrderResponse>(order);
    }

    public async Task<List<OrderResponse>> GetMyOrders()
    {
        var username = GetUsername();
        var orders = await orderRepository.GetMyOrders(username);
        return mapper.Map<List<OrderResponse>>(orders);
    }
}
