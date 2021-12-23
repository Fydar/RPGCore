using UnityEngine;

namespace RPGCoreUnity.Editors
{
	public struct ElementDrawer
	{
		public Rect Position;
		public int Index;

		public ElementDrawer(Rect position, int index)
		{
			Position = position;
			Index = index;
		}
	}
}
