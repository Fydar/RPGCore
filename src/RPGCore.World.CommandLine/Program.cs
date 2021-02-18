using RPGCore.World.CommandLine.Components;

namespace RPGCore.World.CommandLine
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var worldEngineFactory = new WorldEngineFactory()
				.UseEntity(EntityTypes.Unit, options =>
				{
					options.AddComponent<TransformComponent>();
					options.AddComponent<UnitComponent>();
				})
				.UseEntity(EntityTypes.TerrainChunk, options =>
				{
					options.AddComponent<TransformComponent>();
					options.AddComponent<UnitComponent>();
				});

			var worldEngine = worldEngineFactory.Build();
			var world = worldEngine.ConstructWorld();

			var units = world.GetEntityPool(EntityTypes.Unit);

			int newEntityId = units.New();
			ref var newEntityTransform = ref units.GetItem<TransformComponent>(newEntityId);
			newEntityTransform.InnerValue += 1;


			int newEntityId2 = units.New();
			ref var newEntity2Transform = ref units.GetItem<TransformComponent>(newEntityId2);
			newEntity2Transform.InnerValue += 2;


			int newEntityId3 = units.New();
			ref var newEntity3Transform = ref units.GetItem<TransformComponent>(newEntityId3);
			newEntity3Transform.InnerValue += 3;


			int newEntityId4 = units.New();
			ref var newEntity4Transform = ref units.GetItem<TransformComponent>(newEntityId4);
			newEntity4Transform.InnerValue += 4;


			units.Recycle(newEntityId2);
		}
	}
}
