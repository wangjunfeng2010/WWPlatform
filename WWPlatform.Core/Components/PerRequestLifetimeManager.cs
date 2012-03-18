using System;
using Microsoft.Practices.Unity;
using WWPlatform.Core.Utilities;

namespace WWPlatform.Core.Components
{
	public sealed class PerRequestLifetimeManager : LifetimeManager, IDisposable
	{
		private readonly Guid _key = Guid.NewGuid();
		private readonly IPerRequestStore _perRequestStore;

		public PerRequestLifetimeManager(IPerRequestStore perRequestStore)
		{
			Contract.ArgumentNotNull(perRequestStore, "perRequestStore");

			_perRequestStore = perRequestStore;
			_perRequestStore.EndRequest += OnEndRequest;
		}

		#region IDisposable Members

		public void Dispose()
		{
			RemoveValue();
		}

		#endregion

		public override object GetValue()
		{
			return _perRequestStore.GetValue(_key);
		}

		public override void SetValue(object newValue)
		{
			IDisposable previousValue = GetValue() as IDisposable;

			_perRequestStore.SetValue(_key, newValue);

			if (previousValue != null)
			{
				previousValue.Dispose();
			}
		}

		public override void RemoveValue()
		{
			IDisposable previousValue = GetValue() as IDisposable;

			_perRequestStore.Remove(_key);

			if (previousValue != null)
			{
				previousValue.Dispose();
			}
		}

		private void OnEndRequest(object sender, EventArgs e)
		{
			RemoveValue();
		}
	}
}