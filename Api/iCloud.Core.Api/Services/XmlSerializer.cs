using iCloud.Apis.Util;
using System;
using System.IO;

namespace iCloud.Apis.Core.Services
{
    /// <summary>Class for serialization and deserialization of JSON documents using the Newtonsoft Library.</summary>
    public class XmlSerializer : ISerializer
    {
        //private static readonly JsonSerializer _xmlSerializer;
        private static XmlSerializer _instance;

        /// <summary>A singleton instance of the Newtonsoft JSON Serializer.</summary>
        public static XmlSerializer Instance
        {
            get
            {
                return XmlSerializer._instance = XmlSerializer._instance ?? new XmlSerializer();
            }
        }

        static XmlSerializer()
        {
            //JsonSerializerSettings settings = new JsonSerializerSettings();
            //settings.NullValueHandling = NullValueHandling.Ignore;
            //settings.Converters.Add(new RFC3339DateTimeConverter());
            //XmlSerializer._xmlSerializer = JsonSerializer.Create(settings);
        }

        public string Format
        {
            get
            {
                return "xml";
            }
        }

        public void Serialize(object obj, Stream target)
        {
            //using (StreamWriter streamWriter = new StreamWriter(target))
            //{
            //    if (obj == null)
            //        obj = (object)string.Empty;
            //    //XmlSerializerHelper.XmlSerializer((TextWriter)streamWriter, obj);
            //}
            throw new Exception();
        }

        public string Serialize(object obj)
        {
            if (obj == null)
                obj = string.Empty;
            return XmlSerializerHelper.Serialize(obj);
        }

        public T Deserialize<T>(string input)
        {
            if (string.IsNullOrEmpty(input))
                return default(T);
            return XmlSerializerHelper.Deserialize<T>(input);
        }

        public object Deserialize(string input, Type type)
        {
            //if (string.IsNullOrEmpty(input))
            //    return (object)null;
            //return JsonConvert.DeserializeObject(input, type);
            throw new Exception();
        }

        public T Deserialize<T>(Stream input)
        {
            //using (StreamReader streamReader = new StreamReader(input))
            //    return (T)XmlSerializerHelper.Deserialize((TextReader)streamReader, typeof(T));
            throw new Exception();
        }
    }
}
