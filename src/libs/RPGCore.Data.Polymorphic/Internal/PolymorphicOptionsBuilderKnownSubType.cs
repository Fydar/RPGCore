using System;
using System.Collections.Generic;

namespace RPGCore.Data.Polymorphic
{
	/// <summary>
	/// Options for how a sub-type should be identified in serialization.
	/// </summary>
	public class PolymorphicOptionsBuilderKnownSubType : IPolymorphicOptionsBuilderKnownSubType
	{
		internal readonly List<IPolymorphicOptionsBuilderConfigure> configures;

		/// <summary>
		/// The type that this <see cref="PolymorphicOptionsBuilderKnownSubType"/> represents.
		/// </summary>
		public Type Type { get; }

		internal PolymorphicOptionsBuilderKnownSubType(Type type)
		{
			Type = type;
			configures = new List<IPolymorphicOptionsBuilderConfigure>();
		}

		/// <summary>
		/// Directs the serializer to locate additional base-types automatically.
		/// </summary>
		public void UseResolvedBaseTypes()
		{
			configures.Add(new PolymorphicOptionsBuilderConfigureResolveBaseTypes());
		}

		/// <summary>
		/// Directs the serializer to locate additional base-types automatically.
		/// </summary>
		public void UseResolvedBaseTypes(Action<PolymorphicOptionsBuilderResolveBaseType> options)
		{
			configures.Add(new PolymorphicOptionsBuilderConfigureResolveBaseTypes(options));
		}

		/// <summary>
		/// Adds a base-type to a <see cref="Type"/> list of valid base-types.
		/// </summary>
		/// <param name="baseType">The base-type to allow for this <see cref="Type"/>.</param>
		public void UseBaseType(Type baseType)
		{
			configures.Add(new PolymorphicOptionsBuilderConfigureUseBaseType(baseType));
		}

		/// <summary>
		/// Adds a base-type to a <see cref="Type"/> list of valid base-types.
		/// </summary>
		/// <param name="baseType">The base-type to allow for this <see cref="Type"/>.</param>
		/// <param name="options">Options used to configure how the base-type behaves when used by this <see cref="Type"/>.</param>
		public void UseBaseType(Type baseType, Action<PolymorphicOptionsBuilderExplicitType> options)
		{
			configures.Add(new PolymorphicOptionsBuilderConfigureUseBaseType(baseType, options));
		}

		/// <summary>
		/// Adds a base-type to a <see cref="Type"/> list of valid base-types.
		/// </summary>
		/// <typeparam name="TBaseType">The base-type to allow for this <see cref="Type"/>.</typeparam>
		/// <param name="options">Options used to configure how the base-type behaves when used by this <see cref="Type"/>.</param>
		public void UseBaseType<TBaseType>(Action<PolymorphicOptionsBuilderExplicitType> options)
		{
			UseBaseType(typeof(TBaseType), options);
		}
	}
}
