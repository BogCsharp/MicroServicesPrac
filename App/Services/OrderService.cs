using App.Abstrations;
using App.Mappers;
using App.Models.Orders;
using Domain.Context;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Services
{
    public class OrderService(OrdersDbContext context, ICartsService cartsService) : IOrdersService
    {
        public async Task<OrderDTO> Create(CreateOrderDTO order)
        {
            var orderByOrderNumber=await context.Orders.FirstOrDefaultAsync(x=>x.OrderNumber==order.OrderNumber && x.MerchantId==order.MerchantId);
            if (orderByOrderNumber != null)
            {
                throw new DuplicateEntityException($"Order with {order.OrderNumber} is exist for merchant {order.MerchantId}");
            }
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
                CartId = cart.Id,
                Status = OrderStatus.Created,
                MerchantId = order.MerchantId
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

        public async Task Reject(long orderId)
        {
            var order = await context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
            if(order == null)
            {
                throw new EntityNotFound($"Order with Id:{orderId} not found");
            }
            order.Status=OrderStatus.Reject;
            await context.SaveChangesAsync();
        }
    }
}
