using Apache.NMS;
using Apache.NMS.Util;

namespace TwitterCloneClient.Producer.Producer
{
    public class ProducerHandler
    {
        protected static AutoResetEvent Semaphore = new AutoResetEvent(false);
        protected static TimeSpan ReceiveTimeout = TimeSpan.FromSeconds(10);

        public static void Producer(string tweet)
        {
            // Example connection strings:
            //    activemq:tcp://activemqhost:61616
            //    stomp:tcp://activemqhost:61613
            //    ems:tcp://tibcohost:7222
            //    msmq://localhost

            Uri connectUri = new Uri("activemq:tcp://localhost:61616");

            Console.WriteLine("About to connect to " + connectUri);

            // NOTE: ensure the nmsprovider-activemq.config file exists in the executable folder.
            IConnectionFactory factory = new NMSConnectionFactory(connectUri);

            using (IConnection connection = factory.CreateConnection())
            using (ISession session = connection.CreateSession())
            {
                IDestination destination = SessionUtil.GetDestination(session, "queue://Tweet");

                Console.WriteLine("Using destination: " + destination);

                // Create a consumer and producer
                //using (IMessageConsumer consumer = session.CreateConsumer(destination))
                using (IMessageProducer producer = session.CreateProducer(destination))
                {
                    // Start the connection so that messages will be processed.
                    connection.Start();
                    producer.DeliveryMode = MsgDeliveryMode.Persistent;
                    producer.RequestTimeout = ReceiveTimeout;

                    var message = tweet;

                    // Send a message
                    ITextMessage request = session.CreateTextMessage(message);
                    request.NMSCorrelationID = "Tweet";
                    request.Properties["NMSXGroupID"] = "cheese";
                    request.Properties["myHeader"] = "Cheddar";

                    producer.Send(request);

                    // Wait for the message
                    Semaphore.WaitOne((int)ReceiveTimeout.TotalMilliseconds, true);
                }
            }
        }
    }
}
