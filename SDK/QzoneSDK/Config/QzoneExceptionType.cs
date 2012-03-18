using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QzoneSDK.Config
{
    enum QzoneExceptionType
    {
         /// <summary>
        /// Calling a OpenQQ API without having a valid request/access token
        /// </summary>
        TOKEN_REQUIRED = 1, 
        /// <summary>
        /// OpenQQ servers sent an error. For example, bad JSON
        /// </summary>
        REMOTE_ERROR   = 2,
        /// <summary>
        /// Unable to complete an HTTP request. For example, empty JSON response
        /// </summary>
        REQUEST_FAILED = 3,
        /// <summary>
        /// Unable to connect to OpenQQ API servers
        /// </summary>
        CONNECT_FAILED = 4 
    }
}
