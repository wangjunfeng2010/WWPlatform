//===================================================================================
// Microsoft patterns & practices
// Data Access Guidance
//===============================================================================
// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
//===============================================================================

using System;

namespace WWPlatform.Core.Utilities
{
	// Between VSTS2010 Beta 1 and RTM, the Contract class will be replaced with the 
	// System.Diagnostics.Contract class.
	public static class Contract
	{
		public static void ArgumentNotNull(object arg, string argumentName)
		{
			if (arg == null)
			{
				throw new ArgumentNullException(argumentName);
			}
		}

		public static void ArgumentNotEmpty(string arg, string argumentName)
		{
			if (string.IsNullOrEmpty(arg))
			{
				throw new ArgumentOutOfRangeException(argumentName);
			}
		}

		public static void ArgumentPositive<T>(T arg, string argumentName)
			where T : struct, IComparable<T>
		{
			if (arg.CompareTo(default(T)) <= 0)
			{
				throw new ArgumentOutOfRangeException(argumentName);
			}
		}

		public static void ArgumentNotNegative<T>(T arg, string argumentName)
			where T : struct, IComparable<T>
		{
			if (arg.CompareTo(default(T)) < 0)
			{
				throw new ArgumentOutOfRangeException(argumentName);
			}
		}

		public static void Invariant(bool test)
		{
			if (!test)
			{
				throw new InvalidOperationException();
			}
		}
	}
}