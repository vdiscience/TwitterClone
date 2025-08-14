using Apache.NMS;
using Moq;
using TwitterCloneClient.Producer.Producer;
using Xunit;

namespace TwitterCloneBackend.Tests.Producer
{
    public class ProducerTests
    {
        [Fact]
        public void Producer_SendsMessage_WithCorrectProperties()
        {
            // Arrange
            var tweetJson = "{\"text\":\"Hello World!\"}";

            var infraMock = new Mock<IProducerInfrastructure>(MockBehavior.Strict);
            var connectionMock = new Mock<IConnection>();
            var sessionMock = new Mock<ISession>();
            var queueMock = new Mock<IQueue>();
            var producerMock = new Mock<IMessageProducer>();
            var textMessageMock = new Mock<ITextMessage>();
            var primitiveMapMock = new Mock<IPrimitiveMap>();

            // Connection / session / queue / producer creation
            infraMock.Setup(i => i.CreateConnection(It.IsAny<string>())).Returns(connectionMock.Object);
            infraMock.Setup(i => i.CreateSession(connectionMock.Object, AcknowledgementMode.AutoAcknowledge)).Returns(sessionMock.Object);
            infraMock.Setup(i => i.GetQueue(sessionMock.Object, "Tweet")).Returns(queueMock.Object);
            infraMock.Setup(i => i.CreateProducer(sessionMock.Object, queueMock.Object)).Returns(producerMock.Object);
            infraMock.Setup(i => i.CreateTextMessage(sessionMock.Object, tweetJson)).Returns(textMessageMock.Object);

            // Message property map
            textMessageMock.Setup(m => m.Properties).Returns(primitiveMapMock.Object);

            // Capture correlation id assignment
            textMessageMock.SetupSet(m => m.NMSCorrelationID = "Tweet").Verifiable();

            // Delivery mode set
            producerMock.SetupSet(p => p.DeliveryMode = MsgDeliveryMode.Persistent).Verifiable();

            // Send
            producerMock.Setup(p => p.Send(textMessageMock.Object));

            // Act
            ProducerHandler.Producer(tweetJson, infraMock.Object, "Tweet");

            // Assert - verify interactions
            infraMock.Verify(i => i.CreateConnection(It.IsAny<string>()), Times.Once);
            infraMock.Verify(i => i.CreateSession(connectionMock.Object, AcknowledgementMode.AutoAcknowledge), Times.Once);
            infraMock.Verify(i => i.GetQueue(sessionMock.Object, "Tweet"), Times.Once);
            infraMock.Verify(i => i.CreateProducer(sessionMock.Object, queueMock.Object), Times.Once);
            infraMock.Verify(i => i.CreateTextMessage(sessionMock.Object, tweetJson), Times.Once);
            producerMock.VerifySet(p => p.DeliveryMode = MsgDeliveryMode.Persistent, Times.Once);
            textMessageMock.VerifySet(m => m.NMSCorrelationID = "Tweet", Times.Once);
            primitiveMapMock.VerifySet(p => p["NMSXGroupID"] = "cheese", Times.Once);
            primitiveMapMock.VerifySet(p => p["myHeader"] = "Cheddar", Times.Once);
            producerMock.Verify(p => p.Send(textMessageMock.Object), Times.Once);
        }

        [Fact]
        public void Producer_DoesNotThrow_WhenInfrastructureFails()
        {
            // Arrange
            var infraMock = new Mock<IProducerInfrastructure>();
            infraMock.Setup(i => i.CreateConnection(It.IsAny<string>()))
                     .Throws(new InvalidOperationException("Connection failed"));

            // Act & Assert (should swallow exception per implementation)
            ProducerHandler.Producer("{}", infraMock.Object, "Tweet");
        }
    }
}
