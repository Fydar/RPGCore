using System.Collections.Generic;

namespace RPGCore.World;

/// <summary>
/// A <see cref="World"/> containing entities.
/// </summary>
public class World
{
	internal Dictionary<string, EntityPool> entityPools;

	internal World(Dictionary<string, EntityPool> entityPools)
	{
		this.entityPools = entityPools;
	}

	/// <summary>
	/// Gets a pool of entities of a specified type.
	/// </summary>
	/// <param name="identifier">The identifier for the type of entities in the pool.</param>
	/// <returns>The pool of entities of the specified type.</returns>
	public EntityPool GetEntityPool(string identifier)
	{
		return entityPools[identifier];
	}
}
