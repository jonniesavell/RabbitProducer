using MessagingGateway.Services;
using RabbitMQ.Client;

namespace MessagingGateway.Factories;

public class MessageProducerFactory : IDisposable, IAsyncDisposable
{
    public string Host { get; init; }
    public int PortNumber { get; init; }
    public string Username { get; init; }
    public string Password { get; init; }

    public IDictionary<string, IInitializable> Initializables { get; init; }

    public MessageProducerFactory(
        string host,
        int portNumber,
        string username,
        string password,
        IDictionary<string, IInitializable> initializables
    )
    {
        Host = host;
        PortNumber = portNumber;
        Username = username;
        Password = password;
        Initializables = initializables;
    }

    private IConnection? _connection;
    private IChannel? _channel;
    private IDictionary<string, IMessageProducer>? _messageProducers;

    public async Task InitializeAsync()
    {
        var factory = new ConnectionFactory
        {
            HostName = Host,
            Port = PortNumber,
            UserName = Username,
            Password = Password
        };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
        _messageProducers = new Dictionary<string, IMessageProducer>();

        foreach (var stringInitializablePair in Initializables)
        {
            IInitializable initializable = stringInitializablePair.Value;
            IMessageProducer messageProducer = await initializable.Initialize(_channel);
            _messageProducers.Add(stringInitializablePair.Key, messageProducer);
        }
    }

    /// <summary>
    /// Retrieves the appropriate IMessageProducer object from the factory.
    /// </summary>
    /// <exception cref="ArgumentException">in the event that key corresponds to no IMessageProducer</exception>
    /// <exception cref="InvalidOperationException">method InitializeAsync has not yet been called</exception>
    public IMessageProducer RetrieveMessageProducer(string key)
    {
        if (_messageProducers == null)
        {
            throw new InvalidOperationException("method InitializeAsync has not yet been invoked");
        }
        else
        {
            bool present = _messageProducers.TryGetValue(key, out IMessageProducer? messageProducer);

            if (present)
            {
                return messageProducer!;
            }
            else
            {
                throw new ArgumentException("no message producer associated with the given key");
            }
        }
    }

    public void Dispose()
    {
        try
        { 
            _channel?.Dispose();
        }
        finally
        {
            _connection?.Dispose();
        }
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_channel != null)
            {
                await _channel.DisposeAsync();
            }
        }
        finally
        {
            if (_connection != null)
            {
                await _connection.DisposeAsync();
            }
        }
    }
}
