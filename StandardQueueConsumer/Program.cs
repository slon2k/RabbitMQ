using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Common;
using System;

namespace StandardQueueConsumer
{
    public class Program
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _model;
        private const string QueueName = "StandardQueue";

        static void Main()
        {
            CreateConnection();
            
            var consumer = new EventingBasicConsumer(_model);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var payment = body.Deserialize<Payment>();
                Console.WriteLine(@$"Received from {payment.Name} card: {payment.CardNumber} ${payment.Amount}");
            };

            _model.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);


            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();

        }

        private static void CreateConnection()
        {
            _factory = new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest" };
            _connection = _factory.CreateConnection();
            _model = _connection.CreateModel();

            _model.QueueDeclare(QueueName, true, false, false, null);
        }
    }
}
