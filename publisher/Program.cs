using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace Publisher;

  class Program
  {
    static void Main(string[] args)
    {
    var connectionFactory = new RabbitMQ.Client.ConnectionFactory()
    {
    UserName = "guest",
    Password = "guest",
    HostName = "rabbitmq"
    };
    var connection = connectionFactory.CreateConnection();
    var model = connection.CreateModel();
    var properties = model.CreateBasicProperties();
    properties.Persistent = false;
    // Create Exchange
    model.ExchangeDeclare("myexchange", ExchangeType.Fanout);
    // Console.WriteLine("Creating Exchange");

        // Create Queue
    model.QueueDeclare("myque", false, false, false, null);

    byte[] messagebuffer = Encoding.Default.GetBytes("Message Published");
    while(true){
        model.BasicPublish(exchange: "myexchange", routingKey:"myque", basicProperties: null, body: messagebuffer);
        Thread.Sleep(50);
    }
    }
  }