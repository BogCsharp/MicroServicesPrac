using App.Models.Carts;
using App.Models.Orders;
using Domain.Entities;

namespace App.Mappers
{
    public static class CartsMapper
    {
        public static CartDTO ToDto(this CartEntity entity)
        {
            return new CartDTO
            {
                Id = entity.Id,
                CartItems = entity.CartItems!.Select(item => new CartItemDTO
                {
                    Id = item.Id,
                    Price = item.Price,
                    Name = item.Name,
                    Quantity = item.Quantity
                }).ToList()
            };
        }
        public static CartEntity ToEntity(this CartDTO dto)
        {
            return new CartEntity
            {
                
                CartItems = dto.CartItems!.Select(item => item.ToEntity()).ToList()

            };
        }
        public static CartItemEntity ToEntity(this CartItemDTO dto)
        {
            return new CartItemEntity
            {
                Name = dto.Name,
                Price= dto.Price,
                Quantity = dto.Quantity

            };
        }
    }
}
