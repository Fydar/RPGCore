using System;
using UnityEngine;

namespace RPGCore.Behaviour
{
	[Serializable]
	public abstract class OutputSocket : Socket
	{
		private const int socketSize = 16;

#if UNITY_EDITOR
		public Rect socketRect
		{
			get
			{
				return new Rect (drawRect.xMax + 5, drawRect.y, socketSize, socketSize);
			}
		}
#endif
	}
}

