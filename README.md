# MqttPublisher
A simple library, accompanied by a console app, for publishing Mqtt messages. Utilizes M2Mqtt library from Eclipse.

### Prerequisites
To build the solution, .NET Framework 4.5 is needed. This version was chosen because it's the version targetted by the M2Mqtt library used in this project.

### Configuration
You can configure the MQTT broker URL and port in the app.config file in order for it to be default (makes things easier when testing) or by providing them as command-line arguments when running the application.

Edit the app.config file with your MQTT broker details:

```
<appSettings>
  <add key="MqttBrokerUrl" value="mqtt://your-url" />
  <add key="MqttPort" value="1111" />
</appSettings>
```
This project utilizes the M2Mqtt library, which can be found here:
https://github.com/eclipse/paho.mqtt.m2mqtt
