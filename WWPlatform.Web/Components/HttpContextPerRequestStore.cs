//===================================================================================
// Microsoft patterns & practices
// Data Access Guidance
//===============================================================================
// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
//===============================================================================

using System;
using System.Web;

using WWPlatform.Core.Components;
using WWPlatform.Core.Utilities;

namespace WWPlatform.Web.Components
{
	public sealed class HttpContextPerRequestStore : IPerRequestStore
	{
		private readonly HttpApplication _httpApplication;

		public HttpContextPerRequestStore(HttpApplication httpApplication)
		{
			Contract.ArgumentNotNull(httpApplication, "httpApplication");

			_httpApplication = httpApplication;
		}

		#region IPerRequestStore Members

		public event EventHandler EndRequest
		{
			add
			{
				if (_httpApplication != null)
				{
					_httpApplication.EndRequest += value;
				}
			}
			remove
			{
				if (_httpApplication != null)
				{
					_httpApplication.EndRequest -= value;
				}
			}
		}

		public object GetValue(object key)
		{
			if (HttpContext.Current != null)
			{
				return HttpContext.Current.Items[key];
			}

			return null;
		}

		public void SetValue(object key, object value)
		{
			if (HttpContext.Current != null)
			{
				HttpContext.Current.Items[key] = value;
			}
			else
			{
				throw new Exception("Unable to set value.  Context is null.");
			}
		}

		public void Remove(object key)
		{
			if (HttpContext.Current != null)
			{
				HttpContext.Current.Items.Remove(key);
			}
		}

		#endregion
	}
}