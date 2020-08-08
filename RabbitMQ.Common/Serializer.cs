using System.Text;
using System.Text.Json;

namespace RabbitMQ.Common
{
    public static class Serializer
    {
        
        public static byte[] Serialize(this object obj)
        {
            return Encoding.ASCII.GetBytes(JsonSerializer.Serialize(obj));
        }
        
        public static T Deserialize<T>(this byte[] bytes) where T : class
        {
            if (bytes.Length == 0)
            {
                return null;
            }

            return JsonSerializer.Deserialize<T>(Encoding.ASCII.GetString(bytes));
        }

    }
}
