using RabbitMQ.Client;
using RabbitMQ.Common;
using System;
using System.Threading;

namespace DirectRouting.Publisher
{
    public class Program
    {
        private const string ExchangeName = "DirectRouting";
        private const string PaymentQueueName = "CardPaymentQueue";
        private const string OrderQueueName = "PurchaseOrderQueue";

        static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(ExchangeName, type: ExchangeType.Direct);
                    channel.QueueDeclare(PaymentQueueName, true, false, false, null);
                    channel.QueueDeclare(OrderQueueName, true, false, false, null);
                    channel.QueueBind(PaymentQueueName, ExchangeName, "CardPayments");
                    channel.QueueBind(OrderQueueName, ExchangeName, "PurchaseOrders");

                    for (int i = 1; i < 100; i++)
                    {
                        SendPayment(new Payment(i), channel);
                        SendOrder(new Order(i), channel);
                        Thread.Sleep(1000);
                    }
                }
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        private static void SendPayment(Payment payment, IModel channel)
        {
            SendMessage(payment.Serialize(), "CardPayments", channel);
            Console.WriteLine($@"Payment sent {payment.Name} card: {payment.CardNumber} ${payment.Amount}");
        }

        private static void SendOrder(Order order, IModel channel)
        {
            SendMessage(order.Serialize(), "PurchaseOrders", channel);
            Console.WriteLine($@"Order {order.OrderNumber} sent  to {order.CompanyName}, amount: ${order.Amount}");
        }
        
        private static void SendMessage(byte[] message, string routingKey, IModel channel)
        {
            channel.BasicPublish(ExchangeName, routingKey, null, message);
        }
    }
}
