using RPGCore.Behaviour;

namespace RPGCore.Demo.BoardGame
{
	public class GetNeighboursNode : NodeTemplate<GetNeighboursNode>
	{
		public InputSocket Building;
		public OutputSocket Neighbours;
		public bool IncludeDiagonal;

		public override Instance Create() => new GetNeighboursNodeInstance();

		public class GetNeighboursNodeInstance : Instance
		{
			private static readonly Integer2[] adjacentPoints = new Integer2[]
			{
				new Integer2(-1, 0),
				new Integer2(0, 1),
				new Integer2(0, -1),
				new Integer2(1, 0),
			};

			private static readonly Integer2[] surroundingPoints = new Integer2[]
			{
				new Integer2(-1, 1),
				new Integer2(-1, 0),
				new Integer2(-1, -1),

				new Integer2(0, 1),
				new Integer2(0, -1),

				new Integer2(1, 1),
				new Integer2(1, 0),
				new Integer2(1, -1),
			};

			public Input<Building> Building;
			public Output<GameTile[]> Neighbours;

			public override InputMap[] Inputs(ConnectionMapper connections) => new[]
			{
				connections.Connect(ref Template.Building, ref Building)
			};

			public override OutputMap[] Outputs(ConnectionMapper connections) => new[]
			{
				connections.Connect(ref Template.Neighbours, ref Neighbours),
			};

			public override void Setup()
			{
			}

			public override void Remove()
			{
			}

			public override void OnInputChanged()
			{
				if (Building.Value == null)
				{
					Neighbours.Value = null;
					return;
				}

				var directions = Template.IncludeDiagonal
					? surroundingPoints
					: adjacentPoints;

				var neighbours = new GameTile[directions.Length];
				for (int i = 0; i < directions.Length; i++)
				{
					var direction = directions[i];

					neighbours[i] = Building.Value.Tile.Board.GetTileRelative(
						Building.Value.Tile,
						direction);
				}

				Neighbours.Value = neighbours;
			}
		}
	}
}
