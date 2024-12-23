using MessagingGateway.Services;
using RabbitMQ.Client;

namespace MessagingGateway.Application;

public class DirectToHoleInitializable : IInitializable
{
    public async Task<IMessageProducer> Initialize(IChannel channel)
    {
        await channel.ExchangeDeclareAsync(
            exchange: "direct-to-hole",
            type: ExchangeType.Direct,
            durable: true,
            autoDelete: false,
            arguments: new Dictionary<string, object?>()
        );

        return new DirectToHoleMessageProducer(channel);
    }
}
