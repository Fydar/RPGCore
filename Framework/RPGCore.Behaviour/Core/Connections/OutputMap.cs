using System;

namespace RPGCore.Behaviour
{
	public struct OutputMap
	{
		public OutputSocket Source;
		public Type ConnectionType;
		public object Link;

		public OutputMap (OutputSocket source, Type connectionType, object link)
		{
			Source = source;
			Link = link;
			ConnectionType = connectionType;
		}
	}
}
