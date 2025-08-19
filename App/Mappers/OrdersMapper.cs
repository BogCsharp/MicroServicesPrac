using App.Models.Carts;
using App.Models.Orders;
using Domain.Entities;

namespace App.Mappers
{
    public static class OrdersMapper
    {
        public static OrderDTO ToDto(this OrderEntity entity, CartEntity? cart=null)
        {
            return new OrderDTO
            {
                Id = entity.Id,
                CustomerId = entity.CustomerId!.Value,
                Cart = cart == null ? entity.Cart?.ToDto() : cart.ToDto(),
                Name = entity.Name,
                OrderNumber = entity.OrderNumber
            };
        }
        public static OrderEntity ToEntity(this CreateOrderDTO entity, CartDTO? cart = null)
        {
            return new OrderEntity
            {
                CustomerId = entity.CustomerId!.Value,
                Cart = cart?.ToEntity(),
                Name = entity.Name,
                OrderNumber = entity.OrderNumber
            };
        }
    }
}
