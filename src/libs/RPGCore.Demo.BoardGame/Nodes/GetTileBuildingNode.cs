using RPGCore.Behaviour;

namespace RPGCore.Demo.BoardGame
{
	public class GetTileBuildingNode : NodeTemplate<GetTileBuildingNode>
	{
		public InputSocket Tile;
		public OutputSocket Building;

		public override Instance Create()
		{
			return new GetTileBuildingInstance();
		}

		public class GetTileBuildingInstance : Instance
		{
			public Input<GameTile> Tile;
			public Output<Building> Building;

			public override InputMap[] Inputs(ConnectionMapper connections) => new[]
			{
				connections.Connect(ref Template.Tile, ref Tile),
			};

			public override OutputMap[] Outputs(ConnectionMapper connections) => new[]
			{
				connections.Connect(ref Template.Building, ref Building)
			};

			public override void Setup()
			{
			}

			public override void Remove()
			{
			}
		}
	}
}
