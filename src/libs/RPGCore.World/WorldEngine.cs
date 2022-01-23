using System.Collections.Generic;

namespace RPGCore.World;

/// <summary>
/// A factory to be used to construct <see cref="World"/>.
/// </summary>
public class WorldEngine
{
	internal Dictionary<string, WorldEngineFactoryEntityOptions> entities;

	internal WorldEngine(Dictionary<string, WorldEngineFactoryEntityOptions> entities)
	{
		this.entities = entities;
	}

	/// <summary>
	/// Constructs a new <see cref="World"/>.
	/// </summary>
	/// <returns>A new <see cref="World"/>.</returns>
	public World ConstructWorld()
	{
		var entityPools = new Dictionary<string, EntityPool>();
		foreach (var entity in entities)
		{
			entityPools.Add(entity.Key, new EntityPool(entity.Value.components));
		}

		return new World(entityPools);
	}
}
