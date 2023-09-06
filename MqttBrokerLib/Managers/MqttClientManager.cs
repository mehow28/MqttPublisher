using MqttBrokerLib.Models;
using System;
using System.Text;
using uPLibrary.Networking.M2Mqtt;

namespace MqttBrokerLib.Services.Managers
{
    public class MqttClientManager
    {
        private readonly IMqttConfiguration _mqttConfiguration;
        public MqttClient _mqttClient;

        public MqttClientManager(IMqttConfiguration mqttConfiguration )
        {
            _mqttConfiguration = mqttConfiguration;
        }

        public void StartConnection(ConnectionCredentials credentials)
        {
            if(_mqttConfiguration.BrokerUrl == "DEFAULT_BROKER_URL" || _mqttConfiguration.Port == 0)
            {
                throw new Exception("\nNo broker URL and/or port provided! Please enter these values through CLI or app.config.");
            }
            
            string clientId = Guid.NewGuid().ToString();
            _mqttClient = new MqttClient(_mqttConfiguration.BrokerUrl, _mqttConfiguration.Port, true, null, null, MqttSslProtocols.TLSv1_2);
            
            Console.WriteLine("\nConnecting...");
            try
            {
                _mqttClient.Connect(clientId, credentials.UserName, credentials.Password, false, 3);    
            }
            catch(Exception e)
            {
                Console.WriteLine($"\nError while attempting to establish connection: {e.Message}");
            }
        }
        public void Publish(MqttMessage msg)
        {
            _mqttClient.ConnectionClosed += HandleConnectionLost;
            
            try
            {
                if (String.IsNullOrWhiteSpace(msg.Message) || String.IsNullOrWhiteSpace(msg.Topic))
                {
                    throw new Exception("\nThe message and topic cannot be empty; aborting publish");
                }
                _mqttClient.Publish(msg.Topic, Encoding.UTF8.GetBytes(msg.Message));
            }
            catch(Exception e)
            {
                Console.WriteLine($"\nError while publishing message: {e.Message}");
            }
            finally
            {
                Console.WriteLine("Published succesfully.");
            }
        }
        public void CloseConnection()
        {
            try
            {
                if (_mqttClient == null)
                {
                    throw new Exception("No active connection to close.");
                }
                Console.WriteLine("\nClosing connection...");
                _mqttClient.Disconnect();
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nError while attempting to disconnect: {e.Message}");
            }
            finally
            {
                Console.WriteLine("Connection closed.");
            }
        }
        public void HandleConnectionLost(object sender, EventArgs e)
        {
            Console.WriteLine("\nConnection has been lost, please re-enter your credentials.");
            ConnectionCredentials creds = null;
            Console.WriteLine("\nEnter username or press enter to make it null:");
            creds.UserName = Console.ReadLine() ?? "";
            Console.WriteLine("\nEnter password or press enter to make it null:");
            creds.Password = Console.ReadLine() ?? "";

            StartConnection(creds);
        }
    }
}
