using BridgePayload;
using BridgePayload;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Text.Json;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using SparkplugNet.VersionB;
using Google.Protobuf;

var host = Environment.GetEnvironmentVariable("RABBITMQ_MQTT_HOST") ?? "rabbitmq";
var port = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_MQTT_PORT") ?? "1883");
var user = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "user";
var pass = Environment.GetEnvironmentVariable("RABBITMQ_PASS") ?? "password";

var options = new MqttClientOptionsBuilder()
    .WithClientId("sparkplugb-bridge")
    .WithTcpServer(host, port)
    .WithCredentials(user, pass)
    .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V500)
    .Build();

var factory = new MqttFactory();
var client = factory.CreateMqttClient();


client.ApplicationMessageReceivedAsync += async e =>
{
    try
    {
        var payloadBytes = e.ApplicationMessage.PayloadSegment.ToArray();

        var sparkplugPayload = new Payload();
        sparkplugPayload.MergeFrom(payloadBytes);

        var metrics = sparkplugPayload.Metrics.ToDictionary(
            m => m.Name,
            m => m.Value
        );

        var unsTopic = $"UNS/{e.ApplicationMessage.Topic.Replace("/", "_")}";
        var unsPayload = JsonSerializer.Serialize(metrics);

        var unsMessage = new MqttApplicationMessageBuilder()
            .WithTopic(unsTopic)
            .WithPayload(unsPayload)
            .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
	    .WithRetainFlag()
            .Build();

        await client.PublishAsync(unsMessage);
        Console.WriteLine($"Republished Sparkplug B as UNS to {unsTopic}: {unsPayload}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Bridge error: {ex.Message}");
    }
};


await client.ConnectAsync(options);

await client.SubscribeAsync(new MqttClientSubscribeOptionsBuilder()
    .WithTopicFilter("spBv1.0/+/+/NDATA")
    .Build());

Console.WriteLine("Bridge is running. Press Ctrl+C to exit.");
await Task.Delay(-1);
