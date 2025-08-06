using OrdersApi.DTOs;
using OrdersApi.Models;
using System;
using System.Threading.Tasks;

namespace OrdersApi.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(CreateOrderDto orderDto);
        Task<Order?> GetOrderByIdAsync(Guid orderId);
    }
}