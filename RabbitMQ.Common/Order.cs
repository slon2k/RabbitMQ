using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Common
{
    public class Order
    {
        public decimal Amount { get; set; }
        public string OrderNumber { get; set; }
        public string CompanyName { get; set; }

        private static Random random = new Random();

        public Order()
        {

        }

        public Order(int i)
        {
            Amount = random.Next(100, 999);
            OrderNumber = $@"{10000 + i}";
            CompanyName = $@"Company {i}";
        }
    }
}
