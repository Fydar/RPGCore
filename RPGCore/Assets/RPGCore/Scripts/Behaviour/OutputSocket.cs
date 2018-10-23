using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace RPGCore
{
	[Serializable]
	public abstract class OutputSocket : Socket
	{

		public const int socketSize = 16;


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