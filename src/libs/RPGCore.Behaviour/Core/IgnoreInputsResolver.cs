using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace RPGCore.Behaviour;

internal sealed class IgnoreInputsResolver : DefaultContractResolver
{
	protected override JsonProperty CreateProperty(System.Reflection.MemberInfo member, MemberSerialization memberSerialization)
	{
		var prop = base.CreateProperty(member, memberSerialization);

		if (IsSubclassOfRawGeneric(typeof(Input<>), prop.PropertyType))
		{
			prop.Ignored = true;
		}
		return prop;
	}

	private static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
	{
		while (toCheck != null && toCheck != typeof(object))
		{
			var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
			if (generic == cur)
			{
				return true;
			}
			toCheck = toCheck.BaseType;
		}
		return false;
	}
}
