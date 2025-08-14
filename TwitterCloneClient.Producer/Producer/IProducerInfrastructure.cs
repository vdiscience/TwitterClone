using Apache.NMS;

namespace TwitterCloneClient.Producer.Producer
{
    // Abstraction to allow unit testing without real broker
    public interface IProducerInfrastructure
    {
        IConnection CreateConnection(string brokerUri);
        ISession CreateSession(IConnection connection, AcknowledgementMode mode);
        IQueue GetQueue(ISession session, string queueName);
        IMessageProducer CreateProducer(ISession session, IDestination destination);
        ITextMessage CreateTextMessage(ISession session, string text);
    }
}