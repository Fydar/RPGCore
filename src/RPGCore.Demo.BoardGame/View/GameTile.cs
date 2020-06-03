using System;
using System.Diagnostics;

namespace RPGCore.Demo.BoardGame
{
	public class GameTile
	{
		public string Resource;

		public GameBoard Board { get; internal set; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Building building;

		public Building Building
		{
			get
			{
				return building;
			}
			set
			{
				if (building.Tile == null)
				{
					building = value;
					building.Tile = this;
				}
				else
				{
					throw new InvalidOperationException($"Can't add a {nameof(Building)} to this {nameof(GameTile)} as it belongs to another {nameof(GameTile)}.");
				}
			}
		}

		public bool IsEmpty
		{
			get
			{
				return Building == null
					&& Resource == null;
			}
		}

		public char ToChar()
		{
			if (IsEmpty)
			{
				return '\u2610';
			}
			if (Building != null)
			{
				return '^';
			}

			return Resource[0];
		}

		public override string ToString()
		{
			if (IsEmpty)
			{
				return "Empty";
			}

			if (Building != null)
			{
				return Building.ToString();
			}

			return Resource;
		}
	}
}
