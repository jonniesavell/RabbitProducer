using MessagingGateway.Services;
using RabbitMQ.Client;

namespace MessagingGateway.Application;

public class OpenInitializable : IInitializable
{
    public async Task<IMessageProducer> Initialize(IChannel channel)
    {
        await channel.ExchangeDeclareAsync(
            exchange: "open",
            type: ExchangeType.Fanout,
            durable: true,
            autoDelete: false,
            arguments: new Dictionary<string, object?>()
        );

        return new OpenMessageProducer(channel);
    }
}
