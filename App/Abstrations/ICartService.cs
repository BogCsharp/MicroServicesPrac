using App.Models.Carts;

namespace App.Abstrations
{
    public interface ICartsService
    {
        Task<CartDTO> Create(CartDTO cart);
    }
}
