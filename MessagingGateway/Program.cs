using MessagingGateway.Application;
using MessagingGateway.Factories;
using MessagingGateway.Services;

namespace MessagingGateway;

public class Program
{
    public static async Task Main(string[] args)
    {
        string? host = Environment.GetEnvironmentVariable("HOST");
        string? port = Environment.GetEnvironmentVariable("PORT");
        int portNumber;
        if (!int.TryParse(port, out portNumber))
        {
            portNumber = 5672;
        }

        string? username = Environment.GetEnvironmentVariable("USERNAME");
        string? password = Environment.GetEnvironmentVariable("PASSWORD");

        IDictionary<string, IInitializable> initializables = new Dictionary<string, IInitializable>();
        initializables.Add("sink", new DirectToSinkInitializable());
        initializables.Add("hole", new DirectToHoleInitializable());
        initializables.Add("all", new OpenInitializable());

        using (MessageProducerFactory factory =
            new MessageProducerFactory(
                host,
                portNumber,
                username,
                password,
                initializables
            ))
        {
            await factory.InitializeAsync();

            // we would normally create the rest of the application and allow the components within the
            //   application to query for the IMessageProducer instance they care about.
            IMessageProducer messageProducer = factory.RetrieveMessageProducer("all");
            await messageProducer.Send("pants");
        }
    }
}
