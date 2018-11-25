using System;
using UnityEngine;

namespace RPGCore.Tooltips
{
	[Serializable]
	public struct TooltipPositioner
	{
		public Vector2 flipPoint;
		public Vector2 pivot;
		public Vector2 anchorPivot;
	}
}