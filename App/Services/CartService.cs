using App.Abstrations;
using App.Mappers;
using App.Models.Carts;
using Domain.Context;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Services
{
    public class CartService(OrdersDbContext context) : ICartsService
    {
        public async Task<CartDTO> Create(CartDTO cart)
        {
            var cartEntity = new CartEntity();
            var cartSaveResult = await context.Carts.AddAsync(cartEntity);
            await context.SaveChangesAsync();
            ///Привязка CartItem к CartEntity
            var cartItems = cart.CartItems.Select(item => new CartItemEntity
            {
                Name=item.Name,
                Price=item.Price,
                Quantity=item.Quantity,
                CartId=cartSaveResult.Entity.Id

            });
            await context.CartsItems.AddRangeAsync(cartItems);
            await context.SaveChangesAsync();
            
            var result= await context.Carts.Include(x => x.CartItems).FirstAsync(x => x.Id == cartSaveResult.Entity.Id);

            return result.ToDto();

           
            
        }
    }
}
