using System.Security.Principal;
using System.Web;

using WWPlatform.Core.Components;

namespace WWPlatform.Web.Components
{
	public sealed class HttpContextIdentityProvider : IIdentityProvider
	{
		#region IIdentityProvider Members

		public IIdentity Identity
		{
			get { return HttpContext.Current.User.Identity; }
			set { HttpContext.Current.User = new GenericPrincipal(value, null); }
		}

		#endregion
	}
}