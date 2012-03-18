//===================================================================================
// Microsoft patterns & practices
// Data Access Guidance
//===============================================================================
// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
//===============================================================================

namespace WWPlatform.Web.Components
{
	public interface IAuthenticationTicketService
	{
		void SignIn(string userName, bool persistent);
		void SignOut();
	}
}