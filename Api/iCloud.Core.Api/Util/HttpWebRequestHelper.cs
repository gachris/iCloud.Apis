using System;
using System.IO;
using System.Net;
using System.Xml;

namespace iCloud.Apis.Util
{
    public class HttpWebRequestHelper
    {
        public static T ExectueMethod<T>(string url, string method, string content, string contentType, WebHeaderCollection webHeaderCollection = null)
        {
            try
            {
                var responseStream = HttpWebRequestHelper.ExectueMethod(url, method, content, contentType, webHeaderCollection);
                using (StringReader s = new StringReader(responseStream))
                {
                    var xmlDocument = new XmlDocument();
                    xmlDocument.Load(s);
                    return XmlSerializerHelper.Deserialize<T>(xmlDocument.InnerXml);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string ExectueMethod(string url, string method, string content, string contentType, WebHeaderCollection webHeaderCollection = null)
        {
            HttpWebRequest httpGetRequest = (HttpWebRequest)WebRequest.Create(url);
            httpGetRequest.PreAuthenticate = true;
            httpGetRequest.Method = method;
            httpGetRequest.KeepAlive = false;
            httpGetRequest.Timeout = 5000;
            httpGetRequest.ServicePoint.ConnectionLeaseTimeout = 5000;
            httpGetRequest.ServicePoint.MaxIdleTime = 5000;

            foreach (string headerPair in webHeaderCollection)
                httpGetRequest.Headers.Add(headerPair, webHeaderCollection[headerPair]);

            if (!string.IsNullOrWhiteSpace(contentType))
                httpGetRequest.ContentType = contentType;

            using (StreamWriter streamWriter = new StreamWriter(httpGetRequest.GetRequestStream()))
            {
                string data = content;
                streamWriter.Write(data);
                streamWriter.Flush();
                streamWriter.Close();
            }

            using (HttpWebResponse httpGetResponse = (HttpWebResponse)httpGetRequest.GetResponse())
            {
                using (var reader = new StreamReader(httpGetResponse.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static Stream Download(string url, WebHeaderCollection webHeaderCollection = null)
        {
            WebClient request = new WebClient();

            foreach (string headerPair in webHeaderCollection)
                request.Headers.Add(headerPair, webHeaderCollection[headerPair]);

            byte[] data = request.DownloadData(url);

            return new MemoryStream(data);
        }
    }
}
