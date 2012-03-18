using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QzoneSDK.Config
{
    class QzoneException: Exception {
        /// <summary>
        /// Internal error code.
        /// </summary>
        public QzoneExceptionType Code { get; set; }
        /// <summary>
        /// HTTP response
        /// </summary>
        public string Response { get; set; }
     
        public QzoneException(string message, QzoneExceptionType code)
            : this(message,code,null)
        {

        }
        public QzoneException(string message, QzoneExceptionType code, string response)
            : base(message)
        {
            this.Code = code;
            this.Response = response;
        }
    }
}
