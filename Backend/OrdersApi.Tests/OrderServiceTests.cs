using Moq;
using OrdersApi.DTOs;
using OrdersApi.Models;
using OrdersApi.Repositories;
using OrdersApi.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace OrdersApi.Tests
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _mockRepository;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _mockRepository = new Mock<IOrderRepository>();
            _orderService = new OrderService(_mockRepository.Object);
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldReturnCreatedOrder()
        {
            // Arrange
            var orderDto = new CreateOrderDto
            {
                OrderId = Guid.NewGuid(),
                CustomerName = "Test Customer",
                CreatedAt = DateTime.UtcNow,
                Items = new List<OrderItemDto>
                {
                    new OrderItemDto { ProductId = "product-1", Quantity = 2 },
                    new OrderItemDto { ProductId = "product-2", Quantity = 1 }
                }
            };

            _mockRepository.Setup(repo => repo.CreateOrderAsync(It.IsAny<Order>()))
                .ReturnsAsync((Order order) => order);

            // Act
            var result = await _orderService.CreateOrderAsync(orderDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderDto.OrderId, result.OrderId);
            Assert.Equal(orderDto.CustomerName, result.CustomerName);
            Assert.Equal(orderDto.CreatedAt, result.CreatedAt);
            Assert.Equal(orderDto.Items.Count, result.Items.Count);
            
            _mockRepository.Verify(repo => repo.CreateOrderAsync(It.IsAny<Order>()), Times.Once);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ShouldReturnOrder_WhenOrderExists()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = new Order
            {
                OrderId = orderId,
                CustomerName = "Test Customer",
                CreatedAt = DateTime.UtcNow,
                Items = new List<OrderItem>
                {
                    new OrderItem { ProductId = "product-1", Quantity = 2 },
                    new OrderItem { ProductId = "product-2", Quantity = 1 }
                }
            };

            _mockRepository.Setup(repo => repo.GetOrderByIdAsync(orderId))
                .ReturnsAsync(order);

            // Act
            var result = await _orderService.GetOrderByIdAsync(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderId, result.OrderId);
            Assert.Equal(order.CustomerName, result.CustomerName);
            Assert.Equal(order.Items.Count, result.Items.Count);
            
            _mockRepository.Verify(repo => repo.GetOrderByIdAsync(orderId), Times.Once);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ShouldReturnNull_WhenOrderDoesNotExist()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            
            _mockRepository.Setup(repo => repo.GetOrderByIdAsync(orderId))
                .ReturnsAsync((Order)null);

            // Act
            var result = await _orderService.GetOrderByIdAsync(orderId);

            // Assert
            Assert.Null(result);
            
            _mockRepository.Verify(repo => repo.GetOrderByIdAsync(orderId), Times.Once);
        }
    }
}