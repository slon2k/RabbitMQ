using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Common;
using System;

namespace DirectRouting.Subscriber
{
    public class Program
    {
        private const string ExchangeName = "DirectRouting";
        private const string PaymentQueueName = "CardPaymentQueue";
        private const string OrderQueueName = "PurchaseOrderQueue";
        private static string _queueName { get; set; }
        private static string _routingKey { get; set; }

        static void Main()
        {
            int choise = 0;
            while(choise != 1 && choise !=2)
            {
                Console.WriteLine("Choose data to receive:");
                Console.WriteLine("1: Card Payments");
                Console.WriteLine("2: Purchase Orders");
                string input = Console.ReadLine();
                Int32.TryParse(input, out choise);
            }

            if (choise == 1)
            {
                _queueName = PaymentQueueName;
                _routingKey = "CardPayments";
            } else
            {
                _queueName = OrderQueueName;
                _routingKey = "PurchaseOrders";
            }

            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest"};

            using(var connection = factory.CreateConnection())
            {
                using(var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(ExchangeName, type: ExchangeType.Direct);
                    channel.QueueDeclare(_queueName, true, false, false, null);
                    channel.QueueBind(_queueName, ExchangeName, _routingKey);

                    var consumer = new EventingBasicConsumer(channel);
                    if (choise == 1)
                    {
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body.ToArray();
                            var payment = body.Deserialize<Payment>();
                            Console.WriteLine(@$"Received from {payment.Name} card: {payment.CardNumber} ${payment.Amount}");
                            channel.BasicAck(ea.DeliveryTag, multiple: false);
                        };
                    } else
                    {
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body.ToArray();
                            var order = body.Deserialize<Order>();
                            Console.WriteLine(@$"Received order {order.OrderNumber} to {order.CompanyName}, amount ${order.Amount}");
                            channel.BasicAck(ea.DeliveryTag, multiple: false);
                        };
                    }
                    channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }


        }

    }
}
