using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace RPGCore.Behaviour
{
	class IgnoreInputsResolver : DefaultContractResolver
	{
		protected override JsonProperty CreateProperty(System.Reflection.MemberInfo member, MemberSerialization memberSerialization)
		{
			var prop = base.CreateProperty(member, memberSerialization);

			if (typeof(IInput).IsAssignableFrom(prop.PropertyType))
			{
				prop.Ignored = true;
			}
			return prop;
		}
	}
}
