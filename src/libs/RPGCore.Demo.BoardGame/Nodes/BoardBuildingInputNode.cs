using RPGCore.Behaviour;

namespace RPGCore.Demo.BoardGame
{
	public class BoardBuildingInputNode : NodeTemplate<BoardBuildingInputNode>
	{
		public OutputSocket Player;
		public OutputSocket Building;

		public override Instance Create()
		{
			return new LocalBuildingInputInstance();
		}

		public class LocalBuildingInputInstance : Instance
		{
			public Output<LobbyPlayer> Player;
			public Output<Building> Building;

			public override InputMap[] Inputs(ConnectionMapper connections) => null;

			public override OutputMap[] Outputs(ConnectionMapper connections) => new[]
			{
				connections.Connect(ref Template.Player, ref Player),
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
