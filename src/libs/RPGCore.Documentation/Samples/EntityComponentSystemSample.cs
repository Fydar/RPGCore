using RPGCore.World;

namespace RPGCore.Documentation.Samples.EntityComponentSystemSample
{
	public struct TransformComponent
	{
		public int X;
		public int Y;
	}

	public struct UnitComponent
	{
		public int Type;
	}

	public class EntityComponentSystemSample
	{
		public static void Run()
		{
			#region create_world
			var worldEngine = new WorldEngineFactory()
				.UseEntity("unit", options =>
				{
					options.AddComponent<TransformComponent>();
					options.AddComponent<UnitComponent>();
				})
				.Build();

			var world = worldEngine.ConstructWorld();
			#endregion create_world

			#region create_unit
			var units = world.GetEntityPool("unit");

			int newUnitId = units.New();
			ref var newUnitTransform = ref units.GetComponent<TransformComponent>(newUnitId);
			newUnitTransform.X += 1;

			units.Recycle(newUnitId);
			#endregion create_unit
		}
	}
}
