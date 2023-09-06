using MqttBrokerLib.Models;
using System;
using System.Configuration;

namespace MqttBrokerLib.Services.Implementations
{
    public class MqttConfiguration : IMqttConfiguration
    {
        public string BrokerUrl { get; set; }
        public int Port { get; set; }

        public MqttConfiguration()
        {
            BrokerUrl = ConfigurationManager.AppSettings["MqttBrokerUrl"] ?? "DEFAULT_BROKER_URL";
            Port = Convert.ToInt32(ConfigurationManager.AppSettings["MqttPort"] ?? "0");
        }
        public MqttConfiguration(MqttCustomConfig config)
        {
            BrokerUrl = config.BrokerUrl;
            Port = Convert.ToInt32(config.Port);
        }
    }
}
