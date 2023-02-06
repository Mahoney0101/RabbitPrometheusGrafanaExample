using RabbitMQ.Stream.Client;
using RabbitMQ.Stream.Client.Reliable;
using System;
using System.Buffers;
using System.Net;
using System.Text;
using System.Threading;

namespace Publisher;

  class Program
  {
    static async Task Main(string[] args)
    {
                //var addresses = Dns.GetHostAddresses(hostname);

        var config = new StreamSystemConfig
                {
                    UserName = "guest",
                    Password = "guest",
                    VirtualHost = "/",
                    Endpoints = new List<EndPoint> { new IPEndPoint(IPAddress.Parse("10.5.0.5"), 5552) },
        };
                // Connect to the broker and create the system object
            // the entry point for the client.
          // Create it once and reuse it.
        var system = await StreamSystem.Create(config);

        const string stream = "the_stream";

        // Create the stream. It is important to put some retention policy 
        // in this case is 200000 bytes.
        await system.CreateStream(new StreamSpec(stream)
        {
            MaxLengthBytes = 200000,
        });

        try
        {
            var producer = await Producer.Create(
                new ProducerConfig(system, stream)
                {
                    Reference = Guid.NewGuid().ToString(),


                    // Receive the confirmation of the messages sent
                    ConfirmationHandler = confirmation =>
                    {
                        switch (confirmation.Status)
                        {
                            // ConfirmationStatus.Confirmed: The message was successfully sent
                            case ConfirmationStatus.Confirmed:
                                Console.WriteLine($"Message {confirmation.PublishingId} confirmed");
                                break;
                            // There is an error during the sending of the message
                            case ConfirmationStatus.WaitForConfirmation:
                                Console.WriteLine($"WaitForConfirmation");
                                break;
                            case ConfirmationStatus.ClientTimeoutError
                                : // The client didn't receive the confirmation in time.
                                                                   Console.WriteLine($"ClientTimeoutError");
                                break;
                            // but it doesn't mean that the message was not sent
                            // maybe the broker needs more time to confirm the message
                            // see TimeoutMessageAfter in the ProducerConfig
                            case ConfirmationStatus.StreamNotAvailable:
                                Console.WriteLine($"StreamNotAvailable");
                                break;
                            case ConfirmationStatus.InternalError:
                                Console.WriteLine($"InternalError");
                                break;
                            case ConfirmationStatus.AccessRefused:
                                Console.WriteLine($"AccessRefused");
                                break;
                            case ConfirmationStatus.PreconditionFailed:
                                Console.WriteLine($"PreconditionFailed");
                                break;
                            case ConfirmationStatus.PublisherDoesNotExist:
                                Console.WriteLine($"PublisherDoesNotExist");
                                break;
                            case ConfirmationStatus.UndefinedError:
                            default:
                                Console.WriteLine(
                                    $"Message  {confirmation.PublishingId} not confirmed. Error {confirmation.Status}");
                                break;
                        }

                        return Task.CompletedTask;
                    }
                });
        // Publish the messages
        while (true)
        {
            var message = new Message(Encoding.UTF8.GetBytes($"hello"));
            await producer.Send(message);
            Thread.Sleep(100);
        }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
      }
}