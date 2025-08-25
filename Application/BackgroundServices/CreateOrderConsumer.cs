
using App.Abstrations;
using App.Models.Orders;
using Domain.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Web.BackgroundServices
{
    public class CreateOrderConsumer : BackgroundService
    {
        private readonly RabbitMQOptions _rabbitMQOptions;
        private readonly IChannel _channel;
        private readonly IOrdersService _ordersService;
        public CreateOrderConsumer(IOptions<RabbitMQOptions> options,IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            _ordersService= scope.ServiceProvider.GetRequiredService<IOrdersService>();
            _rabbitMQOptions = options.Value;
            var factory = new ConnectionFactory
            {
                HostName = _rabbitMQOptions.HostName,
                Port = _rabbitMQOptions.Port,
                UserName = _rabbitMQOptions.UserName,
                Password = _rabbitMQOptions.Password,
                VirtualHost = _rabbitMQOptions.VirtualHost

            };
            var connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
            _channel = connection.CreateChannelAsync().GetAwaiter().GetResult();

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (_, eventA) =>
            {
                var body = eventA.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                var createOrdersDto = JsonSerializer.Deserialize<CreateOrderDTO>(message);

                await _ordersService.Create(createOrdersDto);
            };
            await _channel.BasicConsumeAsync(_rabbitMQOptions.CreateOrderQueueName,autoAck:false,consumer,cancellationToken:stoppingToken);

        }
    }
}
