using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace iCloud.Apis.Util
{
    public class XmlSerializerHelper
    {
        public static T Deserialize<T>(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(xml))
            {
                return (T)(serializer.Deserialize(reader));
            }
        }

        public static string Serialize<T>(T obj)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (var stringWriter = new StringWriter())
            {
                var settings = new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true };
                using (XmlWriter writer = XmlWriter.Create(stringWriter, settings))
                {
                    xmlSerializer.Serialize(writer, obj);
                    return stringWriter.ToString();
                }
            }
        }

        public static string Serialize(object obj)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
            using (var stringWriter = new StringWriter())
            {
                var settings = new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true };
                using (XmlWriter writer = XmlWriter.Create(stringWriter, settings))
                {
                    xmlSerializer.Serialize(writer, obj);
                    return stringWriter.ToString();
                }
            }
        }
    }
}
