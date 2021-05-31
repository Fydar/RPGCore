using RPGCore.Data.Polymorphic.Inline;

namespace RPGCore.Documentation.Samples.RPGCore.Data
{
	public class PolymorphicInlineBaseTypeName
	{
		#region types
		[SerializeType(TypeName.Name)]
		public interface IProcedure
		{
		}
		#endregion types

		public class CreateProcedure : IProcedure
		{
		}

		public class UpdateProcedure : IProcedure
		{
		}

		public class RemoveProcedure : IProcedure
		{
		}
	}
}
