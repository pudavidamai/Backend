using Microsoft.EntityFrameworkCore;
using OrdersApi.Data;
using OrdersApi.Models;
using OrdersApi.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace OrdersApi.Tests
{
    public class OrderRepositoryTests
    {
        private readonly DbContextOptions<OrderDbContext> _dbContextOptions;

        public OrderRepositoryTests()
        {
            // Configure the in-memory database
            _dbContextOptions = new DbContextOptionsBuilder<OrderDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldAddOrderToDatabase()
        {
            // Arrange
            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                CustomerName = "Test Customer",
                CreatedAt = DateTime.UtcNow,
                Items = new List<OrderItem>
                {
                    new OrderItem { ProductId = "product-1", Quantity = 2 },
                    new OrderItem { ProductId = "product-2", Quantity = 1 }
                }
            };

            // Act
            using (var context = new OrderDbContext(_dbContextOptions))
            {
                var repository = new OrderRepository(context);
                await repository.CreateOrderAsync(order);
            }

            // Assert
            using (var context = new OrderDbContext(_dbContextOptions))
            {
                var savedOrder = await context.Orders
                    .Include(o => o.Items)
                    .FirstOrDefaultAsync(o => o.OrderId == order.OrderId);
                
                Assert.NotNull(savedOrder);
                Assert.Equal(order.OrderId, savedOrder.OrderId);
                Assert.Equal(order.CustomerName, savedOrder.CustomerName);
                Assert.Equal(order.CreatedAt, savedOrder.CreatedAt);
                Assert.Equal(2, savedOrder.Items.Count);
            }
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
                    new OrderItem { ProductId = "product-1", Quantity = 2 }
                }
            };

            using (var context = new OrderDbContext(_dbContextOptions))
            {
                await context.Orders.AddAsync(order);
                await context.SaveChangesAsync();
            }

            // Act
            Order result;
            using (var context = new OrderDbContext(_dbContextOptions))
            {
                var repository = new OrderRepository(context);
                result = await repository.GetOrderByIdAsync(orderId);
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderId, result.OrderId);
            Assert.Equal(order.CustomerName, result.CustomerName);
            Assert.Equal(order.CreatedAt, result.CreatedAt);
            Assert.Single(result.Items);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ShouldReturnNull_WhenOrderDoesNotExist()
        {
            // Arrange
            var nonExistentOrderId = Guid.NewGuid();

            // Act
            Order result;
            using (var context = new OrderDbContext(_dbContextOptions))
            {
                var repository = new OrderRepository(context);
                result = await repository.GetOrderByIdAsync(nonExistentOrderId);
            }

            // Assert
            Assert.Null(result);
        }
    }
}