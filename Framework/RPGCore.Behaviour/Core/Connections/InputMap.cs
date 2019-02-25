using System;

namespace RPGCore.Behaviour
{
	public struct InputMap
	{
		public InputSocket Source;
		public Type ConnectionType;
		public object Link;

		public InputMap (InputSocket source, Type connectionType, object link)
		{
			Source = source;
			Link = link;
			ConnectionType = connectionType;
		}
	}
}
