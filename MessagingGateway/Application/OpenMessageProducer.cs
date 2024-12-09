using MessagingGateway.Services;
using RabbitMQ.Client;
using System.Text;

namespace MessagingGateway.Application;

public class OpenMessageProducer : IMessageProducer
{
    private IChannel _channel;

    public OpenMessageProducer(IChannel channel)
    {
        _channel = channel;
    }

    public async Task Send(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);

        var properties =
            new BasicProperties
            {
                Persistent = true,
                DeliveryMode = DeliveryModes.Persistent
            };

        await _channel.BasicPublishAsync(
            exchange: "open",
            routingKey: "",
            mandatory: false,
            basicProperties: properties,
            body: body
        );
    }
}
