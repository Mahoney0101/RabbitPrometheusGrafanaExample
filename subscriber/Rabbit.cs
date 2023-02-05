namespace Subscriber;

public class Rabbit
{
    ConnectionFactory? factory { get; set; }
    IConnection? connection { get; set; }
    IModel? channel { get; set; }

    public void Register()
    {
        channel!.ExchangeDeclare(exchange: "myexchange", type: ExchangeType.Fanout, durable: false, autoDelete: false, arguments: null);
        channel!.QueueDeclare(queue: "myque", durable: false, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body.ToArray());
        };
        channel.BasicConsume(queue: "myque", autoAck: true, consumer: consumer);
    }

    public void Deregister()
    {
        this.connection!.Close();
    }

    public Rabbit()
    {
        this.factory = new ConnectionFactory() { HostName = "rabbitmq" };
        this.connection = factory.CreateConnection();
        this.channel = connection.CreateModel();
    }
}