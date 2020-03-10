using System.Diagnostics;

namespace RPGCore.Demo.BoardGame
{
	public class GameTile
	{
		public Building Building;
		public string Resource;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public GameBoard Board { get; }

		public bool IsEmpty
		{
			get
			{
				return Building == null
					&& Resource == null;
			}
		}

		public GameTile(GameBoard board)
		{
			Board = board;
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
