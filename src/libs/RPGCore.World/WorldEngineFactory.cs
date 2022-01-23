using System;
using System.Collections.Generic;

namespace RPGCore.World;

/// <summary>
/// A factory for constructing <see cref="WorldEngine"/>s.
/// </summary>
public class WorldEngineFactory
{
	private readonly Dictionary<string, WorldEngineFactoryEntityOptions> entities;

	/// <summary>
	/// Initialises a new instance of the <see cref="WorldEngineFactory"/>.
	/// </summary>
	public WorldEngineFactory()
	{
		entities = new Dictionary<string, WorldEngineFactoryEntityOptions>();
	}

	/// <summary>
	/// Adds a type of entity to the world.
	/// </summary>
	/// <param name="identifier">A unique identifier for the entity type.</param>
	/// <param name="options">Options used to configure world entities.</param>
	/// <returns>This <see cref="WorldEngineFactory"/> for continued construction.</returns>
	public WorldEngineFactory UseEntity(string identifier, Action<WorldEngineFactoryEntityOptions> options)
	{
		var optionsData = new WorldEngineFactoryEntityOptions();

		options(optionsData);

		entities.Add(identifier, optionsData);

		return this;
	}

	/// <summary>
	/// Constructs a new <see cref="WorldEngine"/> from the current state of the factory.
	/// </summary>
	/// <returns>A <see cref="WorldEngine"/> from the current state of the factory.</returns>
	public WorldEngine Build()
	{
		return new WorldEngine(entities);
	}
}
