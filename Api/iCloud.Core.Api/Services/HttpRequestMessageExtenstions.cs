using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace iCloud.Apis.Core.Services
{
    /// <summary>Extension methods to <see cref="T:System.Net.Http.HttpRequestMessage" />.</summary>
    internal static class HttpRequestMessageExtenstions
    {
        /// <summary>
        /// Sets the content of the request by the given body and the the required GZip configuration.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="service">The service.</param>
        /// <param name="body">The body of the future request. If <c>null</c> do nothing.</param>
        /// <param name="gzipEnabled">
        /// Indicates if the content will be wrapped in a GZip stream, or a regular string stream will be used.
        /// </param>
        internal static void SetRequestSerailizedContent(this HttpRequestMessage request, IClientService service, object body, bool gzipEnabled, string contentType = null)
        {
            if (body == null)
                return;
            string mediaType = contentType;
            if (String.IsNullOrEmpty(mediaType))
                mediaType = "application/" + service.Serializer.Format;
            string content = service.SerializeObject(body);
            HttpContent httpContent;
            if (gzipEnabled)
            {
                httpContent = CreateZipContent(content);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue(mediaType) { CharSet = Encoding.UTF8.WebName };
            }
            else httpContent = new StringContent(content, Encoding.UTF8, mediaType);
            request.Content = httpContent;
        }

        /// <summary>Creates a GZip content based on the given content.</summary>
        /// <param name="content">Content to GZip.</param>
        /// <returns>GZiped HTTP content.</returns>
        internal static HttpContent CreateZipContent(string content)
        {
            StreamContent streamContent = new StreamContent(HttpRequestMessageExtenstions.CreateGZipStream(content));
            streamContent.Headers.ContentEncoding.Add("gzip");
            return (HttpContent)streamContent;
        }

        /// <summary>Creates a GZip stream by the given serialized object.</summary>
        private static Stream CreateGZipStream(string serializedObject)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(serializedObject);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (GZipStream gzipStream = new GZipStream((Stream)memoryStream, CompressionMode.Compress, true))
                    gzipStream.Write(bytes, 0, bytes.Length);
                memoryStream.Position = 0L;
                byte[] buffer = new byte[memoryStream.Length];
                memoryStream.Read(buffer, 0, buffer.Length);
                return (Stream)new MemoryStream(buffer);
            }
        }
    }
}
