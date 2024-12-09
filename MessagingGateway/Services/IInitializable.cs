using RabbitMQ.Client;

namespace MessagingGateway.Services;

public interface IInitializable
{
    Task<IMessageProducer> Initialize(IChannel channel);
}
