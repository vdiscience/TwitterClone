using Apache.NMS;
using Apache.NMS.Util;
using Newtonsoft.Json;
using TwitterCloneBackend.DDD.enums;
using TwitterCloneBackend.DDD.Models;

namespace TwitterCloneClient.Producer.Producer
{
    public class ProducerHandler
    {
        protected static AutoResetEvent Semaphore = new AutoResetEvent(false);
        protected static TimeSpan ReceiveTimeout = TimeSpan.FromSeconds(10);

        public static void Producer()
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

                    var message = AddOperations();

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

            string AddOperations()
            {
                var root = new User
                {
                    UserName = "vancho",
                    Password = "ana",
                    Profile = new Profile
                    {
                        ProfileName = "Vancho Dimitrov",
                        Biography = "Being working as a software engineer for quite a while now.",
                        Location = new Location
                        {
                            City = new City
                            {
                                CityName = "London"
                            },
                            Country = new Country
                            {
                                CountryName = "United Kingdom"
                            }
                        },
                        Website = "https://angelchodimitrovski.com",
                        BirthDate = DateTime.Now,
                        Tweets = new List<Tweet>
                        {
                            new Tweet
                            {
                                MediaPath = "TextTweet",
                                TweetText = "Buy my book. Learn c# programming",
                                Tags = "#programmingbook",
                                ReplyType = ReplyType.Everyone,
                                Replies = new List<Replies>
                                {
                                    new Replies
                                    {
                                        RepliedComment = "What a lovely book!"
                                    }
                                }
                            },
                            new Tweet
                            {
                                MediaPath = "c:/2.jpg",
                                TweetText = "See my image",
                                Tags = "#pussycat",
                                ReplyType = ReplyType.Everyone,
                                Replies = new List<Replies>
                                {
                                    new Replies
                                    {
                                        RepliedComment = "What a lovely book!"
                                    }
                                }
                            },
                            new Tweet
                            {
                                MediaPath = "c:/dogs2.mpeg",
                                TweetText = "the lovliest dog ever.",
                                Tags = "#dog",
                                ReplyType = ReplyType.Everyone,
                                Replies = new List<Replies>
                                {
                                    new Replies
                                    {
                                        RepliedComment = "What a lovely book!"
                                    }
                                }
                            }
                        }
                    },
                };


                return JsonConvert.SerializeObject(root, Formatting.Indented, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                //return JsonSerializer.Serialize<User>(root, new JsonSerializerOptions()
                //{
                //    WriteIndented = true
                //});

                //return JsonConvert.SerializeObject(new
                //{
                //    root.UserName,
                //    root.Password,
                //    root.Profile.ProfileName,
                //    root.Profile.Biography,
                //    root.Profile.Location.City,
                //    root.Profile.Location.Country,
                //    root.Profile.Website,
                //    root.Profile.BirthDate,
                //    root.Profile.Tweets
                //}, Formatting.Indented,
                //    new JsonSerializerSettings()
                //    {
                //        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                //    });
            }
        }
    }
}
