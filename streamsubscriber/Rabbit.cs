using RabbitMQ.Stream.Client;
using RabbitMQ.Stream.Client.Reliable;
using System.Buffers;
using System.IO;
using System.Net;

namespace StreamSubscriber;

public class Rabbit
{
    public StreamSystemConfig config;
    public async Task Register()
    {
        // Connect to the broker and create the system object
        // the entry point for the client.
        // Create it once and reuse it.
        var system = await StreamSystem.Create(this.config);

        const string stream = "the_stream";

        // Create the stream. It is important to put some retention policy 
        // in this case is 200000 bytes.
        await system.CreateStream(new StreamSpec(stream)
        {
            MaxLengthBytes = 200000,
        });
        var consumer = await Consumer.Create(
            new ConsumerConfig(system, stream)
            {
                Reference = "my_consumer",
                MessageHandler = async (sourceStream, consumer, ctx, message) =>
                {
                    Console.WriteLine(
                        $"message: {Encoding.Default.GetString(message.Data.Contents.ToArray())}");
                    await Task.CompletedTask;
                }
            });
    }

    public Rabbit()
    {
        this.config = new StreamSystemConfig
        {
            UserName = "guest",
            Password = "guest",
            VirtualHost = "/",
            Endpoints = new List<EndPoint> { new IPEndPoint(IPAddress.Parse("10.5.0.5"), 5552) },
        };
    }
}