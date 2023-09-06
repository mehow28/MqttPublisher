using MqttBrokerLib.Models;
using MqttBrokerLib.Services;
using MqttBrokerLib.Services.Implementations;
using MqttBrokerLib.Services.Managers;
using System;

class MqttApp
{
    static void Main()
    {
        MqttCustomConfig config = new MqttCustomConfig();
        ConnectionCredentials creds = new ConnectionCredentials();
        IMqttConfiguration mqttConfiguration = new MqttConfiguration();
        MqttClientManager client = null;
        
        Console.WriteLine("-------------------------------MQTT Publisher-------------------------------");
        while (true)
        {
            Console.WriteLine("\n1 - Configure MQTT Connection");
            Console.WriteLine("2 - Publish Message");
            Console.WriteLine("3 - Exit");
            Console.WriteLine("Select an option:");

            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case "1":
                    client = ConfigureMqttConnection(config, mqttConfiguration, creds);
                    creds = SetupCredentials();
                    break;
                case "2":
                    if (client != null)
                    {
                        CreateAndPublishMessage(client, creds);
                    }
                    else
                    {
                        Console.WriteLine("Please configure MQTT connection first.");
                    }
                    break;
                case "3":
                    if (client != null)
                    {
                        client.CloseConnection();
                    }
                    Console.WriteLine("Closing application...");
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid option. Please select a valid option.");
                    break;
            }
        }
    }
    public static MqttClientManager ConfigureMqttConnection(MqttCustomConfig configSettings, IMqttConfiguration mqttConfiguration, ConnectionCredentials creds)
    {
        Console.WriteLine("\nCurrent MQTT Broker URL: " + mqttConfiguration.BrokerUrl + ":"+ mqttConfiguration.Port.ToString()+"\n");
        Console.WriteLine("Please provide destination broker URL and port. If you want to use the default destination, stored in app.config, skip this step by providing null values (pressing enter).\n");
        Console.WriteLine("Broker URL:");
        configSettings.BrokerUrl = Console.ReadLine();
        Console.WriteLine("Port:");
        configSettings.Port = Console.ReadLine();

        if (!int.TryParse(configSettings.Port, out int port))
        {
            Console.WriteLine("Invalid port provided; using default value");
            configSettings.Port = mqttConfiguration.Port.ToString();
        }

        if(!String.IsNullOrEmpty(configSettings.Port) && !String.IsNullOrEmpty(configSettings.BrokerUrl))
        {
            mqttConfiguration = new MqttConfiguration(configSettings);
        }
        return new MqttClientManager(mqttConfiguration);
    }

    public static ConnectionCredentials SetupCredentials()
    {
        ConnectionCredentials creds = new ConnectionCredentials();
        Console.WriteLine("\nEnter username or press enter to make it null:");
        creds.UserName = Console.ReadLine() ?? "";
        Console.WriteLine("\nEnter password or press enter to make it null:");
        creds.Password = Console.ReadLine() ?? "";
        return creds;
    }

    public static void CreateAndPublishMessage(MqttClientManager client, ConnectionCredentials creds)
    {

        if (client._mqttClient == null)
        {
            client.StartConnection(creds);
        }

        if (client._mqttClient.IsConnected)
        {
            MqttMessage msg = new MqttMessage();
            Console.WriteLine("\nEnter topic:");
            msg.Topic = Console.ReadLine();
            Console.WriteLine("\nEnter message:");
            msg.Message = Console.ReadLine();
            client.Publish(msg);
        }
        else
        {
            Console.WriteLine("Unable to establish connection, please re-enter credentials and try again.");
        }
    }

}

