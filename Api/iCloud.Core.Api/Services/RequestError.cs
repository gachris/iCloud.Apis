using System.Collections.Generic;
using System.Text;

namespace iCloud.Apis.Core.Services
{
    /// <summary>Collection of server errors</summary>
    public class RequestError
    {
        /// <summary>Contains a list of all errors</summary>
        public IList<SingleError> Errors { get; set; }

        /// <summary>The error code returned</summary>
        public int Code { get; set; }

        /// <summary>The error message returned</summary>
        public string Message { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(this.GetType().FullName).Append(this.Message).AppendFormat(" [{0}]", Code).AppendLine();
            if (this.Errors.IsNullOrEmpty<SingleError>())
            {
                stringBuilder.AppendLine("No individual errors");
            }
            else
            {
                stringBuilder.AppendLine("Errors [");
                foreach (SingleError error in Errors)
                    stringBuilder.Append('\t').AppendLine(error.ToString());
                stringBuilder.AppendLine("]");
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Enumeration of known error codes which may occur during a request.
        /// </summary>
        public enum ErrorCodes
        {
            /// <summary>
            /// The ETag condition specified caused the ETag verification to fail.
            /// Depending on the ETagAction of the request this either means that a change to the object has been
            /// made on the server, or that the object in question is still the same and has not been changed.
            /// </summary>
            ETagConditionFailed = 412, // 0x0000019C
        }
    }
}
