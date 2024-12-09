using MessagingGateway.Services;
using RabbitMQ.Client;
using System.Text;

namespace MessagingGateway.Application;

public class DirectToSinkMessageProducer : IMessageProducer
{
    private IChannel _channel;

    public DirectToSinkMessageProducer(IChannel channel)
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
            exchange: "direct-to-sink",
            routingKey: "routing-key",
            mandatory: false,
            basicProperties: properties,
            body: body
        );
    }
}