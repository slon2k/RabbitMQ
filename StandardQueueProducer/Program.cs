using RabbitMQ.Client;
using RabbitMQ.Common;
using System;
using System.Threading;

namespace StandardQueueProducer
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

            for (int i = 1; i <= 100; i++)
            {
                SendMessage(new Payment(i));
                Thread.Sleep(1000);
            }
        }

        private static void CreateConnection()
        {
            _factory = new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest" };
            _connection = _factory.CreateConnection();
            _model = _connection.CreateModel();

            _model.QueueDeclare(QueueName, true, false, false, null);
        }

        private static void SendMessage(Payment payment)
        {
            _model.BasicPublish("", QueueName, null, payment.Serialize());
            Console.WriteLine($@"Payment sent {payment.Name} card: {payment.CardNumber} ${payment.Amount}");
        }

    }
}
