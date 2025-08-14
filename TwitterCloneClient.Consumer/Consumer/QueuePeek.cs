using Apache.NMS;
using Apache.NMS.ActiveMQ;

namespace TwitterCloneClient.Consumer.Consumer
{
    public static class QueuePeek
    {
        private const string BrokerUri = "tcp://localhost:61616";
        private const string QueueName = "Tweet";

        public static void Peek()
        {
            IConnectionFactory factory = new ConnectionFactory(BrokerUri);
            using var connection = factory.CreateConnection();
            connection.Start();

            using var session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge);
            using var destination = session.GetQueue(QueueName);
            using var browser = session.CreateBrowser(destination);

            Console.WriteLine("Browsing queue: " + QueueName);
            var enumerator = browser.GetEnumerator();
            int count = 0;
            while (enumerator.MoveNext())
            {
                if (enumerator.Current is ITextMessage tm)
                {
                    count++;
                    Console.WriteLine($"[{count}] ID={tm.NMSMessageId} CorrelationID={tm.NMSCorrelationID}");
                }
            }
            Console.WriteLine($"Total browsed (not removed): {count}");
        }
    }
}