using OrdersApi.Models;
using System;
using System.Threading.Tasks;

namespace OrdersApi.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<Order?> GetOrderByIdAsync(Guid orderId);
    }
}