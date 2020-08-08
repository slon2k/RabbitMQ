using RabbitMQ.Client;
using RabbitMQ.Common;
using System;
using System.Text;

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

            for (int i = 1; i <= 10; i++)
            {
                SendMessage(new Payment { Amount = (decimal)i * 5, CardNumber = "0123456789", Name = $@"Client {i}" });
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
            Console.WriteLine($@"Payment sent {payment.Name} {payment.CardNumber} {payment.Amount}");
        }
    }
}
