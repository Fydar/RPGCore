using System;
using System.Threading;

namespace RPGCore.World
{
	internal static class ComponentType
	{
		internal static int componentTypesCount;
	}

	internal static class ComponentType<T>
		where T : struct
	{
		internal static readonly int typeIdentifier;
		internal static readonly Type type;

		static ComponentType()
		{
			typeIdentifier = Interlocked.Increment(ref ComponentType.componentTypesCount);
			type = typeof(T);
		}
	}
}
