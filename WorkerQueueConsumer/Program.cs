using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Common;
using System;

namespace WorkerQueueConsumer
{
    public class Program
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _model;
        private const string QueueName = "WorkerQueue";

        static void Main()
        {
            CreateConnection();

            var consumer = new EventingBasicConsumer(_model);
            consumer.Received += (sender, ea) =>
            {
                var body = ea.Body.ToArray();
                var payment = body.Deserialize<Payment>();
                Console.WriteLine(@$"Received from {payment.Name} card: {payment.CardNumber} ${payment.Amount}");
                _model.BasicAck(ea.DeliveryTag, multiple: false);
            };
            _model.BasicConsume(QueueName, autoAck: false, consumer);
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
