namespace Domain.Options
{
    public class RabbitMQOptions
    {
        public required  string HostName { get; set; }
        public required  int Port { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string VirtualHost { get; set; }
        public required string CreateOrderQueueName { get; set; }

    }
}
