using App.Abstrations;
using App.Models.Orders;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Web.Controllers
{
    [Route("api/orders")]
    public class OrdersController(IOrdersService orders,ILogger<OrdersController>logger) : ApiBaseController
    {
        [HttpPost]
        public async Task<IActionResult>Create(CreateOrderDTO orderDTO)
        {
            logger.LogInformation($"Method api/orders Create start.Request: {JsonSerializer.Serialize(orderDTO)}");
            var result =await orders.Create(orderDTO);
            logger.LogInformation($"Method api/orders Create finished.Request: {JsonSerializer.Serialize(orderDTO)}"+$"Response:{JsonSerializer.Serialize(result)}");

            return Ok(result);


        }
        [HttpGet("{orderId:long}")]
        public async Task<IActionResult> GetById(long orderId)
        {
            logger.LogInformation($"Method api/orders GetById start.");
            var result = await orders.GetById(orderId); 
           
            logger.LogInformation($"Method api/orders GetById finished."+ $"Response:{JsonSerializer.Serialize(result)}");
            return Ok(result);

        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation($"Method api/orders GetAll start.");
            var result = await orders.GetAll();
            logger.LogInformation($"Method api/orders GetAll finished. Count :{result.Count}");
            return Ok(new {orders=result});
        }
        [HttpGet("customers/{customerId:long}")]
        public async Task<IActionResult> GetByUser(long customerId)
        {
            logger.LogInformation($"Method api/orders/customers{customerId} GetByUser start.");
            var result = await orders.GetByUser(customerId);
            logger.LogInformation($"Method api/orders/customers{customerId} GetByUser finished."+ $"Response:{ JsonSerializer.Serialize(result)}");
            return Ok(new { orders = result });
        }
        [HttpPost("{orderId:long}/reject")]
        public async Task<IActionResult> Reject(long orderId)
        {
            await orders.Reject(orderId);
            return Ok();
        }
    }
}
