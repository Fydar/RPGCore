using RPGCore.Behaviour;

namespace RPGCore.Demo.BoardGame
{
	public class GlobalBuildingInputNode : NodeTemplate<GlobalBuildingInputNode>
	{
		public OutputSocket Players;
		public OutputSocket Building;

		public override Instance Create() => new GlobalBuildingInputInstance();

		public class GlobalBuildingInputInstance : Instance
		{
			public Output<GamePlayer[]> Players;
			public Output<Building> Building;

			public override InputMap[] Inputs(ConnectionMapper connections) => null;

			public override OutputMap[] Outputs(ConnectionMapper connections) => new[]
			{
				connections.Connect(ref Template.Players, ref Players),
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
