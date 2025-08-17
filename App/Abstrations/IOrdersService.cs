using App.Models.Orders;

namespace App.Abstrations
{
    public interface IOrdersService
    {
        Task<OrderDTO> Create(CreateOrderDTO order);
        Task<OrderDTO> GetById(Guid orderId);
        Task<List<OrderDTO>> GetByUser(Guid customerId);
        Task<List<OrderDTO>> GetAll();
        Task Reject(Guid orderId);

    }
}
