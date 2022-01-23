using System;
using System.Threading;

namespace RPGCore.World;

internal static class EntityType
{
	internal static int entityTypeCount;
}

internal static class EntityType<T>
	where T : struct
{
	internal static readonly int typeIdentifier;
	internal static readonly Type type;

	static EntityType()
	{
		typeIdentifier = Interlocked.Increment(ref EntityType.entityTypeCount);
		type = typeof(T);
	}
}
