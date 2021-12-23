using System;

namespace RPGCore.Data.Polymorphic
{
	/// <summary>
	/// Options used to configure the behaviour of a known sub type.
	/// </summary>
	public interface IPolymorphicOptionsBuilderKnownSubType
	{
		/// <summary>
		/// Directs the serializer to locate additional base-types automatically.
		/// </summary>
		public void UseResolvedBaseTypes();

		/// <summary>
		/// Directs the serializer to locate additional base-types automatically.
		/// </summary>
		public void UseResolvedBaseTypes(Action<PolymorphicOptionsBuilderResolveBaseType> options);

		/// <summary>
		/// Adds a base-type to a <see cref="Type"/> list of valid base-types.
		/// </summary>
		/// <param name="baseType">The base-type to allow for this <see cref="Type"/>.</param>
		void UseBaseType(Type baseType);

		/// <summary>
		/// Adds a base-type to a <see cref="Type"/> list of valid sub-types.
		/// </summary>
		/// <param name="baseType">The base-type to allow for this <see cref="Type"/>.</param>
		/// <param name="options">Options used to configure how the base-type behaves when used by this <see cref="Type"/>.</param>
		void UseBaseType(Type baseType, Action<PolymorphicOptionsBuilderExplicitType> options);

		/// <summary>
		/// Adds a base-type to a <see cref="Type"/> list of valid base-types.
		/// </summary>
		/// <typeparam name="TSubType">The base-type to allow for this <see cref="Type"/>.</typeparam>
		/// <param name="options">Options used to configure how the base-type behaves when used by this <see cref="Type"/>.</param>
		void UseBaseType<TSubType>(Action<PolymorphicOptionsBuilderExplicitType> options);
	}
}
