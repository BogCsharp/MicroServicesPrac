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
    }
}
