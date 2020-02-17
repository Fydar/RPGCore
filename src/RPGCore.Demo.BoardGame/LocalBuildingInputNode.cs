using RPGCore.Behaviour;

namespace RPGCore.Demo.BoardGame
{
	public class LocalBuildingInputNode : NodeTemplate<LocalBuildingInputNode>
	{
		public OutputSocket Owner;
		public OutputSocket Building;

		public override Instance Create() => new LocalBuildingInputInstance();

		public class LocalBuildingInputInstance : Instance
		{
			public Output<GamePlayer> Owner;
			public Output<Building> Building;

			public override InputMap[] Inputs(ConnectionMapper connections) => null;

			public override OutputMap[] Outputs(ConnectionMapper connections) => new[]
			{
				connections.Connect(ref Template.Owner, ref Owner),
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
