using System;

namespace RPGCore.Behaviour
{
	public interface IConnectionTypeConverter : IConnection
	{
		Type ConvertFromType { get; }
		Type ConvertToType { get; }

		void SetSource(IConnection source);
	}
}
