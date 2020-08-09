using RabbitMQ.Client;
using RabbitMQ.Common;
using System;
using System.Threading;

namespace PublishSubscribe.Publisher
{
    public class Program
    {
        private const string ExchangeName = "PublishSubscribe";
        
        static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };
            using(var connection = factory.CreateConnection())
            {
                using(var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(ExchangeName, type: ExchangeType.Fanout);
                    
                    for (int i = 1; i < 100; i++)
                    {
                        SendMessage(new Payment(i), channel);
                        Thread.Sleep(1000);
                    }

                }
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        private static void SendMessage(Payment payment, IModel channel)
        {
            channel.BasicPublish(ExchangeName, "", null, payment.Serialize());
            Console.WriteLine($@"Payment sent {payment.Name} card: {payment.CardNumber} ${payment.Amount}");
        }
    }
}
