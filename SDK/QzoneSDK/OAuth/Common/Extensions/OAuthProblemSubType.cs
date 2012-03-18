using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QzoneSDK.OAuth.Common.Extensions
{
    [Flags]
    public enum OAuthProblemSubType
    {
        oauth_parameters_absent,
        oauth_parameters_rejected,
        oauth_acceptable_timestamps,
        oauth_acceptable_versions,
        oauth_problem_advice,
    }
}
