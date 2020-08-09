using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Common;
using System;

namespace PublishSubscribe.Subscriber
{
    public class Program
    {
        private const string ExchangeName = "PublishSubscribe";

        static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };
            
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType.Fanout);

                    var queueName = channel.QueueDeclare().QueueName;
                    
                    channel.QueueBind(queue: queueName, exchange: ExchangeName, routingKey: "");

                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var payment = body.Deserialize<Payment>();
                        Console.WriteLine(@$"Received from {payment.Name} card: {payment.CardNumber} ${payment.Amount}");
                    };

                    channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
            
        }
    }
}
