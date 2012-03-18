using System.Web.Security;

using WWPlatform.Core.Utilities;

namespace WWPlatform.Web.Components
{
	public sealed class FormsAuthenticationTicketService : IAuthenticationTicketService
	{
		#region IAuthenticationTicketService Members

		public void SignIn(string userName, bool persistent)
		{
			Contract.ArgumentNotEmpty(userName, "userName");

			FormsAuthentication.SetAuthCookie(userName, persistent);
		}

		public void SignOut()
		{
			FormsAuthentication.SignOut();
		}

		#endregion
	}
}