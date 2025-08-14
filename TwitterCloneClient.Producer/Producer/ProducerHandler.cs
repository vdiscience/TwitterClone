using Apache.NMS;
using Apache.NMS.ActiveMQ;

namespace TwitterCloneClient.Producer.Producer
{
    public class ProducerHandler
    {
        private const string BrokerUri = "tcp://localhost:61616";
        private const string QueueName = "Tweet";

        public static void Producer(string tweetJson)
        {
            Console.WriteLine($"Connecting to ActiveMQ broker {BrokerUri}");

            try
            {
                IConnectionFactory factory = new ConnectionFactory(BrokerUri);

                using var connection = factory.CreateConnection();
                connection.Start();

                using var session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge);
                using IDestination destination = session.GetQueue(QueueName);
                using var producer = session.CreateProducer(destination);

                producer.DeliveryMode = MsgDeliveryMode.Persistent;

                ITextMessage msg = session.CreateTextMessage(tweetJson);
                msg.NMSCorrelationID = "Tweet";
                msg.Properties["NMSXGroupID"] = "cheese";
                msg.Properties["myHeader"] = "Cheddar";

                producer.Send(msg);
                Console.WriteLine($"Sent message (ID: {msg.NMSMessageId}) to queue {QueueName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Producer error: " + ex);
            }
        }
    }
}
