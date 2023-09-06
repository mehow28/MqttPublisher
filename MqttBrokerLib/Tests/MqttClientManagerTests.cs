using Moq;
using MqttBrokerLib.Models;
using MqttBrokerLib.Services;
using MqttBrokerLib.Services.Managers;
using NUnit.Framework;
using System;
using System.Configuration;
using uPLibrary.Networking.M2Mqtt;

namespace MqttBrokerLib.Tests
{
    [TestFixture]
    public class MqttClientManagerTests
    {
        [Test]
        public void StartConnection_WithValidConfiguration_CreatesMqttClient()
        {
            var mqttConfiguration = new Mock<IMqttConfiguration>();
            mqttConfiguration.Setup(c => c.BrokerUrl).Returns("broker.hivemq.com");
            mqttConfiguration.Setup(c => c.Port).Returns(8883);

            var manager = new MqttClientManager(mqttConfiguration.Object);
            var credentials = new ConnectionCredentials
            {
                UserName = "username",
                Password = "password"
            };

            manager.StartConnection(credentials);

            Assert.IsNotNull(manager._mqttClient);
            Assert.True(manager._mqttClient.IsConnected);
        }

        [Test]
        public void StartConnection_WithInvalidConfiguration_ThrowsException()
        {
            var mqttConfiguration = new Mock<IMqttConfiguration>();
            mqttConfiguration.Setup(c => c.BrokerUrl).Returns(ConfigurationManager.AppSettings["MqttBrokerUrl"]);
            mqttConfiguration.Setup(c => c.Port).Returns(Convert.ToInt32(ConfigurationManager.AppSettings["MqttPort"]));

            var manager = new MqttClientManager(mqttConfiguration.Object);
            var credentials = new ConnectionCredentials
            {
                UserName = "username",
                Password = "password"
            };

            Assert.Throws<Exception>(() => manager.StartConnection(credentials));
        }

        [Test]
        public void Publish_WithValidMessage_PublishesMessage()
        {
            var mqttConfiguration = new Mock<IMqttConfiguration>();

            var manager = new MqttClientManager(mqttConfiguration.Object);
            manager._mqttClient = new MqttClient("broker.hivemq.com", 8883, true, null, null, MqttSslProtocols.TLSv1_2);
            var message = new MqttMessage
            {
                Topic = "test",
                Message = "msg"
            };

            manager.Publish(message);
            Assert.DoesNotThrow(() => manager.Publish(message));
        }
        [Test]
        public void Publish_WithEmptyMessage_ThrowsException()
        {
            var mqttConfiguration = new Mock<IMqttConfiguration>();
            var manager = new MqttClientManager(mqttConfiguration.Object);
            var message = new MqttMessage
            {
                Topic = "test",
                Message = ""
            };

            Assert.Throws<System.NullReferenceException>(() => manager.Publish(message));
        }

        [Test]
        public void Publish_WithEmptyTopic_ThrowsException()
        {
            var mqttConfiguration = new Mock<IMqttConfiguration>();
            var manager = new MqttClientManager(mqttConfiguration.Object);
            var message = new MqttMessage
            {
                Topic = "", 
                Message = "msg"
            };

            Assert.Throws<System.NullReferenceException>(() => manager.Publish(message));
        }

        [Test]
        public void CloseConnection_WithActiveConnection_ClosesConnection()
        {
            var mqttConfiguration = new Mock<IMqttConfiguration>();
            var manager = new MqttClientManager(mqttConfiguration.Object);

            manager._mqttClient = new MqttClient("broker.hivemq.com", 8883, true, null, null, MqttSslProtocols.TLSv1_2);

            manager.CloseConnection();

            Assert.False(manager._mqttClient.IsConnected);
        }
    }
}