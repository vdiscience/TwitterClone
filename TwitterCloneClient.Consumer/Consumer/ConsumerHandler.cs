using Apache.NMS;
using Apache.NMS.Util;
using System.Text.Json;
using TwitterCloneBackend.DDD;
using TwitterCloneBackend.DDD.Models;

namespace TwitterCloneClient.Consumer.Consumer
{
    public class ConsumerHandler
    {
        protected static AutoResetEvent Semaphore = new AutoResetEvent(false);
        protected static ITextMessage Message = null;
        protected static TimeSpan ReceiveTimeout = TimeSpan.FromSeconds(30);

        public static void Consumer()
        {
            // Example connection strings:
            //    activemq:tcp://activemqhost:61616
            //    stomp:tcp://activemqhost:61616
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
                using (IMessageConsumer consumer = session.CreateConsumer(destination))
                {
                    // Start the connection so that messages will be processed.
                    connection.Start();
                    consumer.Listener += new MessageListener(OnMessage);

                    // Wait for the message
                    Semaphore.WaitOne((int)ReceiveTimeout.TotalMilliseconds, true);

                    if (Message == null)
                    {
                        Console.WriteLine("No message received!");
                    }
                    else
                    {
                        if (Message.NMSCorrelationID == "Tweet")
                        {
                            Console.WriteLine("Destination: " + Message.NMSDestination);
                            Console.WriteLine("Correlation ID: " + Message.NMSCorrelationID);
                            Console.WriteLine("Received message with ID:   " + Message.NMSMessageId);
                            Console.WriteLine("Received message with text: " + Message.Text);
                            Console.WriteLine("Priority: " + Message.NMSPriority);
                            Console.WriteLine("Delivery Mode: " + Message.NMSDeliveryMode);

                            var obj = DeserializeUsingGenericSystemTextJson(Message.Text);

                            Console.WriteLine();
                            Console.WriteLine();
                            Console.WriteLine(value: obj?.UserName);
                            Console.WriteLine();

                            if (obj.Profile != null)
                            {
                                //foreach (var profileTweet in obj.Profile.Tweets)
                                //{
                                //    Console.WriteLine(profileTweet.TweetText);
                                //}

                                Console.WriteLine("LINQ loop start");
                                obj.Profile.Tweets
                                    .ForEach(x =>
                                    {
                                        Console.WriteLine(x);
                                        Console.WriteLine(x.Profile.ProfileName);
                                    });


                                Console.WriteLine("LINQ loop end");
                            }

                            var dataContext = new DataContext();
                            dataContext.Users.Add(obj);
                            dataContext.SaveChanges();
                            Console.WriteLine("Saved into database!");
                        }
                    }
                }
            }

            User? DeserializeUsingGenericSystemTextJson(string json)
            {
                var _message = JsonSerializer.Deserialize<User>(json, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });
                return _message;
            }

            void OnMessage(IMessage receivedMsg)
            {
                Message = receivedMsg as ITextMessage;
                Semaphore.Set();
            }
        }

        void OnMessage(IMessage receivedMsg)
        {
            Message = receivedMsg as ITextMessage;
            Semaphore.Set();
        }
    }
}
