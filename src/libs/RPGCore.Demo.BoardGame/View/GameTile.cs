using Newtonsoft.Json;
using System.Diagnostics;

namespace RPGCore.Demo.BoardGame
{
	public class GameTile
	{
		[JsonIgnore]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public GameBoard Board { get; internal set; }

		public string? Resource;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Building? building;

		public Building? Building
		{
			get => HierachyHelper.Get(this, ref building);
			set => HierachyHelper.Set(this, ref building, value);
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

		/// <inheritdoc/>
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
