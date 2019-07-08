using System;

namespace RPGCore.Behaviour
{
	public struct InputMap
	{
		public InputSocket Source;
		public Type ConnectionType;

		public InputMap (InputSocket source, Type connectionType)
		{
			Source = source;
			ConnectionType = connectionType;
		}

		public override string ToString () => $"{Source} of type {ConnectionType}";
	}
}
