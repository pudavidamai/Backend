namespace OrdersApi.DTOs
{
    public class OrderItemDto
    {
        public string ProductId { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}