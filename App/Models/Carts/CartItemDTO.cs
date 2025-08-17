namespace App.Models.Carts
{
    public class CartItemDTO
    {
        public long? Id { get; set; }
        public string Name { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
