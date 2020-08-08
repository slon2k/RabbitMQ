using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Common
{
    public class Payment
    {
        public decimal Amount { get; set; }
        public string CardNumber { get; set; }
        public string Name { get; set; }
    }
}
