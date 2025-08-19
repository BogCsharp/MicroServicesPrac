using App.Abstrations;
using App.Mappers;
using App.Models.Orders;
using Domain.Context;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace App.Services
{
    public class OrderService(OrdersDbContext context, ICartsService cartsService) : IOrdersService
    {
        public async Task<OrderDTO> Create(CreateOrderDTO order)
        {
            if (order.Cart == null)
            {
                throw new ArgumentNullException();
            }
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


            return orderEntityResult.ToDto();
        }

        public async Task<List<OrderDTO>> GetAll()
        {
            var entity = await context.Orders.Include(o => o.Cart).ThenInclude(c => c.CartItems).ToListAsync();

            return entity.Select(x => x.ToDto()).ToList();
        }

        public async Task<OrderDTO> GetById(long orderId)
        {
            var entity =await context.Orders.Include(x => x.Cart).ThenInclude(c => c.CartItems).FirstOrDefaultAsync(x=>x.Id==orderId);

            if(entity == null)
            {
                throw new EntityNotFound($"Order Entity with ID{orderId} not found");
            }
            return entity.ToDto();
        }

        public async Task<List<OrderDTO>> GetByUser(long customerId)
        {
            var entity=await context.Orders.Include(o=>o.Cart).ThenInclude(c=>c.CartItems).Where(context=>context.CustomerId==customerId).ToListAsync();

            return entity.Select(x=>x.ToDto()).ToList();
        }

        public Task Reject(long orderId)
        {
            throw new NotImplementedException();
        }
    }
}
