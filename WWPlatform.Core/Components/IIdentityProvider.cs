using System.Security.Principal;

namespace WWPlatform.Core.Components
{
	public interface IIdentityProvider
	{
		IIdentity Identity { get; set; }
	}
}