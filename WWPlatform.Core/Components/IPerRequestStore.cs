using System;

namespace WWPlatform.Core.Components
{
	public interface IPerRequestStore
	{
		event EventHandler EndRequest;

		object GetValue(object key);
		void SetValue(object key, object value);
		void Remove(object key);
	}
}