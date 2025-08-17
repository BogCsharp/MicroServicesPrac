using App.Abstrations;
using App.Models.Orders;
using Domain.Context;
using Domain.Entities;

namespace App.Services
{
    public class OrderService(OrdersDbContext context, ICartsService cartsService) : IOrdersService
    {
        public async Task<OrderDTO> Create(CreateOrderDTO order)
        {
            var cart = await cartsService.Create(order.Cart);
            var entity = new OrderEntity
            {
                OrderNumber = order.OrderNumber,
                Name = order.Name,
                CustomerId = order.CustomerId,
                CartId = cart.Id
            };
            var ordersSaveResult = await context.Orders.AddAsync(entity);
            await context.SaveChangesAsync();

            var orderEntityResult = ordersSaveResult.Entity;


            return new OrderDTO
            {
                Id = orderEntityResult.Id,
                CustomerId = orderEntityResult.CustomerId!.Value,
                Cart = cart,
                Name = orderEntityResult.Name,
                OrderNumber = orderEntityResult.OrderNumber
            };
        }

        public Task<List<OrderDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<OrderDTO> GetById(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderDTO>> GetByUser(Guid customerId)
        {
            throw new NotImplementedException();
        }

        public Task Reject(Guid orderId)
        {
            throw new NotImplementedException();
        }
    }
}
