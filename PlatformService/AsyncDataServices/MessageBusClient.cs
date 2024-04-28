using System.Text;
using System.Text.Json;
using PlatformService.Dto;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection? _connection;
        private readonly IModel? _channel;

        public MessageBusClient(IConfiguration configuration)
        {


            _configuration = configuration;
            var factory = new ConnectionFactory()
            {
                HostName = _configuration.GetValue<string>("RabbitMQHost"),
                Port = _configuration.GetValue<int>("RabbitMQPort"),
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
                Console.WriteLine("->> Connected to MessageBus");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"->> Coudln't connect to the message bus, {ex.Message}");
            }
        }


        public void PublishNewPlatform(PlatformPublishedDto plat)
        {
            if (_connection is null)
            {
                Console.WriteLine("->> Coulnd't send message, _connection is null");
                return;
            }

            var message = JsonSerializer.Serialize(plat);

            if (_connection.IsOpen)
            {
                Console.WriteLine("->> Sending message to the Message Bus");
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("->> Coulnd't send message to the Message Bus, RabbitMQ is not open");
            }
        }

        private void SendMessage(string message)
        {
            if (_channel is null)
            {
                Console.WriteLine("->> Coudn't send message to the Message Bus, _channel is null");
                return;
            }
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "trigger", routingKey: "", basicProperties: null, body: body);

            Console.WriteLine($"->> This message has been sent to the Message Bus: {message}");

        }

        private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs? e)
        {
            Console.WriteLine("->> RabbitMQ connection shutdown");

        }

        public void Dispose()
        {
            Console.WriteLine("->> Message Bus disposed");
            if (_channel is not null && _channel.IsOpen)
            {
                _channel.Close();
            }

            if (_connection is not null && _connection.IsOpen)
            {
                _connection.Close();
            }
        }
    }
}