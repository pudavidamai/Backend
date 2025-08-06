using OrdersApi.DTOs;
using OrdersApi.Models;
using OrdersApi.Repositories;
using System;
using System.Threading.Tasks;

namespace OrdersApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> CreateOrderAsync(CreateOrderDto orderDto)
        {
            var order = new Order
            {
                OrderId = orderDto.OrderId,
                CustomerName = orderDto.CustomerName,
                CreatedAt = orderDto.CreatedAt
            };

            foreach (var itemDto in orderDto.Items)
            {
                order.Items.Add(new OrderItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity
                });
            }

            return await _orderRepository.CreateOrderAsync(order);
        }

        public async Task<Order?> GetOrderByIdAsync(Guid orderId)
        {
            return await _orderRepository.GetOrderByIdAsync(orderId);
        }
    }
}