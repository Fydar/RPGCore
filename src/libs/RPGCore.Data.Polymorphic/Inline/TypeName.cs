using RPGCore.Data.Polymorphic.Naming;
using System;
using System.Reflection;

namespace RPGCore.Data.Polymorphic.Inline
{
	/// <summary>
	/// Identifies a type naming source.
	/// </summary>
	[Flags]
	public enum TypeName
	{
		/// <summary>
		/// Indicates that no type names should be used.
		/// </summary>
		None = 0,

		/// <summary>
		/// Identifies type naming that is sourced from <see cref="Type.FullName"/>.
		/// </summary>
		/// <remarks>
		/// Equivilant to supplying <see cref="TypeFullNameNamingConvention"/>.
		/// </remarks>
		FullName = 1,

		/// <summary>
		///Identifies type naming that is sourced from <see cref="MemberInfo.Name"/>.
		/// </summary>
		/// <remarks>
		/// Equivilant to supplying <see cref="TypeNameNamingConvention"/>.
		/// </remarks>
		Name = 2,

		/// <summary>
		/// Identifies type naming that is sourced from <see cref="Type.AssemblyQualifiedName"/>.
		/// </summary>
		/// <remarks>
		/// Equivilant to supplying <see cref="TypeAssemblyQualifiedNameNamingConvention"/>.
		/// </remarks>
		AssemblyQualifiedName = 4,

		/// <summary>
		/// Identifies type naming that is sourced from <see cref="Type.GUID"/>.
		/// </summary>
		/// <remarks>
		/// Equivilant to supplying <see cref="TypeGuidNamingConvention"/>.
		/// </remarks>
		GUID = 8
	}
}
