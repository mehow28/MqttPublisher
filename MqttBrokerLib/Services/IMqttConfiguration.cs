namespace MqttBrokerLib.Services
{
    public interface IMqttConfiguration
    {
        string BrokerUrl { get; set; }
        int Port { get; set; }
    }
}
