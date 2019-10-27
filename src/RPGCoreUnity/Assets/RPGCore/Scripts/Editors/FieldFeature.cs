using UnityEngine;

namespace RPGCore.Unity.Editors
{
	public class FieldFeature
	{
		public bool Expanded;
		public Rect GlobalRenderedPosition;
		public Rect LocalRenderedPosition;

		public Rect InputSocketPosition => new Rect (GlobalRenderedPosition)
		{
			xMax = GlobalRenderedPosition.xMin,
			xMin = GlobalRenderedPosition.xMin - GlobalRenderedPosition.height
		};
	}
}
