using App.Models.Orders;

namespace App.Abstrations
{
    public interface IOrdersService
    {
        Task<OrderDTO> Create(CreateOrderDTO order);
        Task<OrderDTO> GetById(long orderId);
        Task<List<OrderDTO>> GetByUser(long customerId);
        Task<List<OrderDTO>> GetAll();
        Task Reject(long orderId);

    }
}
