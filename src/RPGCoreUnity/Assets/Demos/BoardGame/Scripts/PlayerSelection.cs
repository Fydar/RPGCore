using RPGCore.Demo.BoardGame;
using System;
using UnityEngine;

namespace RPGCoreUnity.Demo.BoardGame
{
	[Serializable]
	public class PlayerSelection
	{
		public bool isSelected;
		public int x;
		public int y;
		public int width;
		public int height;

		public Rect AsRect => new Rect(x, y, width, height);
		public IntegerRect AsIntegerRect => new IntegerRect(x, y, width, height);
	}
}
