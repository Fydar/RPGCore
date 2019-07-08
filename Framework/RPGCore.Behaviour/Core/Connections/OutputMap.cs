using System;

namespace RPGCore.Behaviour
{
	public struct OutputMap
	{
		public OutputSocket Source;
		public Type ConnectionType;

		public OutputMap (OutputSocket source, Type connectionType)
		{
			Source = source;
			ConnectionType = connectionType;
		}

		public override string ToString () => $"{Source} of type {ConnectionType}";
	}
}
