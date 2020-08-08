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
    }
}
