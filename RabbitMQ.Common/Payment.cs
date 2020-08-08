using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RabbitMQ.Common
{
    public class Payment
    {
        public decimal Amount { get; set; }
        public string CardNumber { get; set; }
        public string Name { get; set; }

        private static Random random = new Random();

        private static string GenerateCardNumber()
        {
            var list = new List<string>();
            for (int i = 1; i < 5; i++)
            {
                list.Add(random.Next(1000, 9999).ToString());
            }
            return String.Join(" ", list);
        }

        public Payment()
        {

        }

        public Payment(int i)
        {
            Name = @$"Client {i}";
            Amount = random.Next(1, 100);
            CardNumber = GenerateCardNumber();
        }
    }
}
