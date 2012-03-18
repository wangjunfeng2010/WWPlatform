//===================================================================================
// Microsoft patterns & practices
// Data Access Guidance
//===============================================================================
// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
//===============================================================================

using System.Web;

namespace WWPlatform.Web.Components
{
	public sealed class HttpContextSessionStore : ISessionStore
	{
		#region ISessionStore Members

		public object this[string key]
		{
			get { return HttpContext.Current.Session[key]; }
			set { HttpContext.Current.Session[key] = value; }
		}

		#endregion
	}
}