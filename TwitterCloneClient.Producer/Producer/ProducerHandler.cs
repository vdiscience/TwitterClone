using Apache.NMS;

namespace TwitterCloneClient.Producer.Producer
{
    public class ProducerHandler
    {
        private const string BrokerUri = "tcp://localhost:61616";
        private const string QueueName = "Tweet";
        
        public static void Producer(string tweetJson)
        {
            Producer(tweetJson, new ActiveMqInfrastructure(), QueueName);
        }
        
        public static void Producer(string tweetJson, IProducerInfrastructure infra, string queueName)
        {
            Console.WriteLine($"Connecting to ActiveMQ broker {BrokerUri}");

            try
            {
                using var connection = infra.CreateConnection(BrokerUri);
                using var session = infra.CreateSession(connection, AcknowledgementMode.AutoAcknowledge);
                using var destination = infra.GetQueue(session, queueName);
                using var producer = infra.CreateProducer(session, destination);

                producer.DeliveryMode = MsgDeliveryMode.Persistent;

                var msg = infra.CreateTextMessage(session, tweetJson);
                msg.NMSCorrelationID = "Tweet";
                msg.Properties["NMSXGroupID"] = "cheese";
                msg.Properties["myHeader"] = "Cheddar";

                producer.Send(msg);
                Console.WriteLine($"Sent message (ID: {msg.NMSMessageId}) to queue {queueName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Producer error: " + ex);
            }
        }
    }
}
