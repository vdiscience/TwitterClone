using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Polly;
using Polly.Retry;
using System.Text.Json;
using TwitterCloneBackend.Entities.Models;

namespace TwitterCloneClient.Consumer.Consumer
{
    public abstract class ConsumerHandler
    {
        private const string BrokerUri = "activemq:tcp://localhost:61616";
        private const string QueueName = "Tweet";
        private const string ErrorQueueName = "Tweet.Error"; // optional error sink
        private static readonly TimeSpan ReceiveTimeout = TimeSpan.FromSeconds(5);

        // Configure retry: 3 attempts (initial + 2 retries) with exponential backoff: 1s, 2s
        private static readonly RetryPolicy ProcessingRetryPolicy =
            Policy.Handle<Exception>()
                  .WaitAndRetry(
                      retryCount: 2,
                      sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)), // 1,2
                      onRetry: (ex, delay, attempt, ctx) =>
                      {
                          Console.WriteLine($"Retry {attempt} after {delay.TotalSeconds:n1}s due to: {ex.GetType().Name}: {ex.Message}");
                      });

        public static void Consumer()
        {
            Console.WriteLine($"Connecting to ActiveMQ broker {BrokerUri}");
            Console.WriteLine("Receiving messages (CTRL+C to exit)...");

            using var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); Console.WriteLine("Cancellation requested..."); };

            try
            {
                IConnectionFactory factory = new ConnectionFactory(BrokerUri);

                using var connection = factory.CreateConnection();
                connection.ExceptionListener += ex => Console.WriteLine("Connection exception: " + ex);
                connection.Start();
                Console.WriteLine("Connection started.");

                using var session = connection.CreateSession(AcknowledgementMode.ClientAcknowledge);
                using IDestination destination = session.GetQueue(QueueName);
                using IDestination errorDestination = session.GetQueue(ErrorQueueName);
                Console.WriteLine("Using queue: " + destination);

                using var consumer = session.CreateConsumer(destination);
                using var errorProducer = session.CreateProducer(errorDestination);
                Console.WriteLine("Consumer created.");

                // Re-enable DB if needed
                // var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
                // using var dataContext = new DataContext(optionsBuilder.Options);

                int processed = 0;
                int emptyPolls = 0;

                while (!cts.IsCancellationRequested)
                {
                    var message = consumer.Receive(ReceiveTimeout);
                    if (message == null)
                    {
                        emptyPolls++;
                        if (emptyPolls % 5 == 0)
                            Console.WriteLine($"Still polling... ({emptyPolls} empty polls)");
                        continue;
                    }
                    emptyPolls = 0;

                    if (message is not ITextMessage msg)
                    {
                        Console.WriteLine("Non-text message received (ignored).");
                        message.Acknowledge();
                        continue;
                    }

                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine($"Message #{++processed}");
                    Console.WriteLine("Destination: " + msg.NMSDestination);
                    Console.WriteLine("Correlation ID: " + msg.NMSCorrelationID);
                    Console.WriteLine("Message ID: " + msg.NMSMessageId);
                    Console.WriteLine("Redelivered: " + msg.NMSRedelivered);
                    Console.WriteLine("Body:");
                    Console.WriteLine(msg.Text);

                    bool success = false;

                    try
                    {
                        ProcessingRetryPolicy.Execute(() =>
                        {
                            // 1. Deserialize
                            var user = Deserialize(msg.Text) ?? throw new InvalidOperationException("Deserialization returned null.");

                            // 2. (Optional) Persist to DB
                            // dataContext.Users.Add(user);
                            // dataContext.SaveChanges();

                            // 3. Any other domain logic here
                            // Throw exceptions to trigger retry.
                        });

                        success = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Processing permanently failed after retries. Error: {ex.GetType().Name}: {ex.Message}");

                        // Forward to error queue (dead-letter pattern)
                        try
                        {
                            var errorCopy = session.CreateTextMessage(msg.Text);
                            // Preserve some headers / metadata
                            errorCopy.Properties.SetString("OriginalMessageId", msg.NMSMessageId ?? "");
                            errorCopy.Properties.SetString("ProcessingError", ex.Message);
                            errorCopy.NMSCorrelationID = msg.NMSCorrelationID;
                            errorProducer.Send(errorCopy);
                            Console.WriteLine("Message forwarded to error queue: " + ErrorQueueName);
                        }
                        catch (Exception forwardEx)
                        {
                            Console.WriteLine("Failed to forward to error queue: " + forwardEx);
                            // If forwarding fails, you might choose to NOT ack so it can be redelivered.
                            // For now we let it fall through and ack to avoid infinite loop.
                        }
                    }
                    finally
                    {
                        // Ack only after success OR after we deliberately move it aside.
                        msg.Acknowledge();
                        Console.WriteLine(success ? "Acked (success)." : "Acked (failed & diverted).");
                    }
                }

                Console.WriteLine($"Exiting. Total messages processed: {processed}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Consumer error: " + ex);
            }

            Console.WriteLine("Consumer stopped.");
        }

        private static User? Deserialize(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<User>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Deserialization error: " + ex.Message);
                return null;
            }
        }
    }
}
