using System.Collections.Generic;

namespace RPGCore.World;

/// <summary>
/// Options used to configure world entities.
/// </summary>
public class WorldEngineFactoryEntityOptions
{
	internal readonly List<WorldEngineEntityComponentPoolFactory> components;

	internal WorldEngineFactoryEntityOptions()
	{
		components = new List<WorldEngineEntityComponentPoolFactory>();
	}

	/// <summary>
	/// Adds a component to entities of this type.
	/// </summary>
	/// <typeparam name="T">The type of the component to add to the entity.</typeparam>
	public void AddComponent<T>()
		where T : struct
	{
		components.Add(
			new WorldEngineEntityComponentPoolFactory(
				typeof(T),
				ComponentType<T>.typeIdentifier,
				(capacity) => new EntityComponentPool<T>(capacity)
			)
		);
	}
}
