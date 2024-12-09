using System.IO;

namespace MessagingGateway.Services;

public interface IMessageProducer
{
    /// <summary>
    /// Sends a message to the appropriate location.
    /// </summary>
    /// <exception cref="IOException">is thrown in the event that underlying infrastructure has failed</exception>
    public Task Send(string message);
}
