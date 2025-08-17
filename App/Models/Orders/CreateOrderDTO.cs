using App.Models.Carts;

namespace App.Models.Orders
{
    public class CreateOrderDTO
    {
        public string? Name { get; set; }
        public long OrderNumber { get; set; }
        public long? CustomerId { get; set; }
        public CartDTO Cart { get; set; } = null!;
    }
}
