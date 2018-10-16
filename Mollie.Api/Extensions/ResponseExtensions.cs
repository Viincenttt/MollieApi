using System.IO;
using System.Xml.Serialization;

namespace Mollie.Api.Extensions
{
    public static class ResponseExtensions
    {
        public static T Deserialize<T>(this string response)
        {
            T instance;

            using (var stringReader = new StringReader(response))
            {
                var xmlSerializer = new XmlSerializer(typeof(T));

                instance = (T)xmlSerializer.Deserialize(stringReader);
            }

            return instance;
        }
    }
}