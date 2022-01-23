using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace RPGCore.World;

/// <summary>
/// Represents a collection of entities with components.
/// </summary>
[DebuggerDisplay("Count = {Count,nq}")]
[DebuggerTypeProxy(typeof(EntityPoolDebugView))]
public sealed class EntityPool
{
	private readonly IEntityComponentPool[] componentPools;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private int componentsPoolSize;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly BitArray usedItems;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private int[] reservedItemsLookup;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private int itemsCount;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private int reservedItemsCount;

	/// <summary>
	/// The amount of entities that are currently active from the pool.
	/// </summary>
	public int Count => itemsCount - reservedItemsCount;

	internal EntityPool(IReadOnlyList<WorldEngineEntityComponentPoolFactory> components)
	{
		componentsPoolSize = 128;

		int componentsCount = components.Count;
		componentPools = new IEntityComponentPool[componentsCount];
		for (int i = 0; i < componentsCount; i++)
		{
			var component = components[i];
			componentPools[i] = component.ConstructEntityComponentPool(componentsPoolSize);
		}

		usedItems = new BitArray(128);
		reservedItemsLookup = new int[componentsPoolSize];
	}

	/// <summary>
	/// Increases the capacity of the pool.
	/// </summary>
	/// <param name="capacity"></param>
	internal void SetCapacity(int capacity)
	{
		if (capacity > componentsPoolSize)
		{
			SetCapacityInternal(capacity);
		}
	}

	/// <summary>
	/// Allocates a new entity from the pool.
	/// </summary>
	/// <returns>An identifier for the new entity created.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int New()
	{
		int id;
		if (reservedItemsCount > 0)
		{
			id = reservedItemsLookup[--reservedItemsCount];
		}
		else
		{
			id = itemsCount;
			if (itemsCount == componentsPoolSize)
			{
				SetCapacityInternal(itemsCount << 1);
			}

			itemsCount++;
		}
		usedItems.Set(id, true);
		return id;
	}

	/// <summary>
	/// Grabs a reference to an entity component in a pool.
	/// </summary>
	/// <typeparam name="T">The type of component to get from the entity.</typeparam>
	/// <param name="index">An identifier for the entity in the pool.</param>
	/// <returns>A reference to the component in the pool.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ref T GetComponent<T>(int index)
		where T : struct
	{
		var pool = GetPool<T>();
		return ref pool.GetItem(index);
	}

	/// <summary>
	/// Returns an entity to the pool.
	/// </summary>
	/// <param name="index">An identifier for the entity in the pool.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Recycle(int index)
	{
		for (int i = 0; i < componentPools.Length; i++)
		{
			var pool = componentPools[i];
			pool.Reset(index);
		}

		if (reservedItemsCount == reservedItemsLookup.Length)
		{
			Array.Resize(ref reservedItemsLookup, reservedItemsCount << 1);
		}
		usedItems.Set(index, false);
		reservedItemsLookup[reservedItemsCount++] = index;
	}

	/// <summary>
	/// Enumerates all reserved identifiers in the pool.
	/// </summary>
	/// <returns>An enumerator for all in-use entities.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public IEnumerable<int> AllIds()
	{
		for (int i = 0; i < usedItems.Count; i++)
		{
			if (usedItems[i])
			{
				yield return i;
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal void CopyData(int sourceIndex, int destinationIndex)
	{
		for (int i = 0; i < componentPools.Length; i++)
		{
			var pool = componentPools[i];
			pool.CopyData(sourceIndex, destinationIndex);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private EntityComponentPool<T> GetPool<T>()
		where T : struct
	{
		for (int i = 0; i < componentPools.Length; i++)
		{
			var pool = componentPools[i];
			if (pool is EntityComponentPool<T> castedPool)
			{
				return castedPool;
			}
		}
		throw new InvalidOperationException($"Unable to find pool of type \"{typeof(T).Name}\".");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void SetCapacityInternal(int capacity)
	{
		for (int i = 0; i < componentPools.Length; i++)
		{
			var pool = componentPools[i];
			pool.SetCapacity(capacity);
		}
		componentsPoolSize = capacity;
	}

	private class EntityPoolDebugView
	{
		[DebuggerDisplay("Entity", Name = "{EntityId}")]
		internal struct DebuggerEntity
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			public int EntityId;

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public DebuggerEntityComponent[] Components;
		}

		[DebuggerDisplay("{Value,nq}", Name = "{Key,nq}")]
		internal struct DebuggerEntityComponent
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			public string Key;

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public object Value;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly EntityPool entityPool;

		public EntityPoolDebugView(EntityPool entityPool)
		{
			this.entityPool = entityPool;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public DebuggerEntity[] Keys
		{
			get
			{
				var keys = new DebuggerEntity[entityPool.Count];

				int i = 0;
				foreach (int id in entityPool.AllIds())
				{
					var components = new DebuggerEntityComponent[entityPool.componentPools.Length];

					for (int c = 0; c < entityPool.componentPools.Length; c++)
					{
						var componentPool = entityPool.componentPools[c];

						components[c] = new DebuggerEntityComponent()
						{
							Key = componentPool.GetType().GetGenericArguments()[0].Name.Replace("Component", ""),
							Value = componentPool.GetObject(id)
						};
					}

					keys[i] = new DebuggerEntity
					{
						EntityId = id,

						Components = components
					};
					i++;
				}
				return keys;
			}
		}
	}
}
