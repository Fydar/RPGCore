using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace RPGCore.World
{
	/// <summary>
	/// A component pool.
	/// </summary>
	/// <typeparam name="T">The type of the component to pool.</typeparam>
	public class EntityComponentPool<T> : IEntityComponentPool
		where T : struct
	{
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		private T[] pool;

		internal EntityComponentPool(int capacity = 128)
		{
			pool = new T[capacity];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void IEntityComponentPool.SetCapacity(int capacity)
		{
			Array.Resize(ref pool, capacity);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal ref T GetItem(int index)
		{
			return ref pool[index];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void IEntityComponentPool.CopyData(int srcIdx, int dstIdx)
		{
			pool[dstIdx] = pool[srcIdx];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void IEntityComponentPool.Reset(int index)
		{
			pool[index] = default;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		object IEntityComponentPool.GetObject(int index)
		{
			return pool[index];
		}
	}
}
