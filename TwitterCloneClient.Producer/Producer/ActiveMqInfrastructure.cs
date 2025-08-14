using Apache.NMS;
using Apache.NMS.ActiveMQ;

namespace TwitterCloneClient.Producer.Producer
{
    public class ActiveMqInfrastructure : IProducerInfrastructure
    {
        public IConnection CreateConnection(string brokerUri)
        {
            IConnectionFactory factory = new ConnectionFactory(brokerUri);
            var connection = factory.CreateConnection();
            connection.Start();
            return connection;
        }

        public ISession CreateSession(IConnection connection, AcknowledgementMode mode) =>
            connection.CreateSession(mode);

        public IQueue GetQueue(ISession session, string queueName) =>
            session.GetQueue(queueName);

        public IMessageProducer CreateProducer(ISession session, IDestination destination) =>
            session.CreateProducer(destination);

        public ITextMessage CreateTextMessage(ISession session, string text) =>
            session.CreateTextMessage(text);
    }
}