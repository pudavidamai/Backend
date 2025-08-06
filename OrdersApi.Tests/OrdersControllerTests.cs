using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using OrdersApi.Controllers;
using OrdersApi.DTOs;
using OrdersApi.Models;
using OrdersApi.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace OrdersApi.Tests
{
    public class OrdersControllerTests
    {
        private readonly Mock<IOrderService> _mockService;
        private readonly Mock<ILogger<OrdersController>> _mockLogger;
        private readonly OrdersController _controller;

        public OrdersControllerTests()
        {
            _mockService = new Mock<IOrderService>();
            _mockLogger = new Mock<ILogger<OrdersController>>();
            _controller = new OrdersController(_mockService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task CreateOrder_ShouldReturnCreatedAtAction_WhenOrderIsCreated()
        {
            // Arrange
            var orderDto = new CreateOrderDto
            {
                OrderId = Guid.NewGuid(),
                CustomerName = "Test Customer",
                CreatedAt = DateTime.UtcNow,
                Items = new List<OrderItemDto>
                {
                    new OrderItemDto { ProductId = "product-1", Quantity = 2 }
                }
            };

            var order = new Order
            {
                OrderId = orderDto.OrderId,
                CustomerName = orderDto.CustomerName,
                CreatedAt = orderDto.CreatedAt,
                Items = new List<OrderItem>
                {
                    new OrderItem { ProductId = "product-1", Quantity = 2 }
                }
            };

            _mockService.Setup(service => service.CreateOrderAsync(It.IsAny<CreateOrderDto>()))
                .ReturnsAsync(order);

            // Act
            var result = await _controller.CreateOrder(orderDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(OrdersController.GetOrder), createdAtActionResult.ActionName);
            Assert.Equal(orderDto.OrderId, createdAtActionResult.Value);
            Assert.Equal(201, createdAtActionResult.StatusCode);

            _mockService.Verify(service => service.CreateOrderAsync(It.IsAny<CreateOrderDto>()), Times.Once);
        }

        [Fact]
        public async Task CreateOrder_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var orderDto = new CreateOrderDto
            {
                OrderId = Guid.NewGuid(),
                CustomerName = "Test Customer",
                CreatedAt = DateTime.UtcNow,
                Items = new List<OrderItemDto>()
            };

            _mockService.Setup(service => service.CreateOrderAsync(It.IsAny<CreateOrderDto>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.CreateOrder(orderDto);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("An error occurred while creating the order", objectResult.Value);

            _mockService.Verify(service => service.CreateOrderAsync(It.IsAny<CreateOrderDto>()), Times.Once);
        }

        [Fact]
        public async Task GetOrder_ShouldReturnOk_WhenOrderExists()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = new Order
            {
                OrderId = orderId,
                CustomerName = "Test Customer",
                CreatedAt = DateTime.UtcNow,
                Items = new List<OrderItem>()
            };

            _mockService.Setup(service => service.GetOrderByIdAsync(orderId))
                .ReturnsAsync(order);

            // Act
            var result = await _controller.GetOrder(orderId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(order, okResult.Value);

            _mockService.Verify(service => service.GetOrderByIdAsync(orderId), Times.Once);
        }

        [Fact]
        public async Task GetOrder_ShouldReturnNotFound_WhenOrderDoesNotExist()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            _mockService.Setup(service => service.GetOrderByIdAsync(orderId))
                .ReturnsAsync((Order)null);

            // Act
            var result = await _controller.GetOrder(orderId);

            // Assert
            Assert.IsType<NotFoundResult>(result);

            _mockService.Verify(service => service.GetOrderByIdAsync(orderId), Times.Once);
        }

        [Fact]
        public async Task GetOrder_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            _mockService.Setup(service => service.GetOrderByIdAsync(orderId))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetOrder(orderId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("An error occurred while retrieving the order", objectResult.Value);

            _mockService.Verify(service => service.GetOrderByIdAsync(orderId), Times.Once);
        }
    }
}