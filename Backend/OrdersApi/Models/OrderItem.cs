using System;
using System.Text.Json.Serialization;

namespace OrdersApi.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public Guid OrderId { get; set; }
        public string ProductId { get; set; } = string.Empty;
        public int Quantity { get; set; }
        
        [JsonIgnore]
        public Order Order { get; set; } = null!;
    }
}
