
using App.Abstrations;
using App.Models.Orders;
using Domain.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Writers;
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
        private readonly IServiceProvider _serviceProvider;
        public CreateOrderConsumer(IOptions<RabbitMQOptions> options,IServiceProvider serviceProvider)
        {
            _rabbitMQOptions = options.Value;
            _serviceProvider= serviceProvider;
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
                try
                {
                    var createOrdersDto = JsonSerializer.Deserialize<CreateOrderDTO>(message, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    })!;
                    using var scope = _serviceProvider.CreateScope();
                    var ordersService = scope.ServiceProvider.GetRequiredService<IOrdersService>();

                    await ordersService.Create(createOrdersDto);

                    await _channel.BasicAckAsync(eventA.DeliveryTag, false, stoppingToken);

                }
                catch(Exception)
                {
                    await _channel.BasicAckAsync(eventA.DeliveryTag, false, stoppingToken);

                }
               
            };
            await _channel.BasicConsumeAsync(_rabbitMQOptions.CreateOrderQueueName,autoAck:false,consumer,cancellationToken:stoppingToken);

        }
    }
}
