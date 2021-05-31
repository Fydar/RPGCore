using RPGCore.Data.Polymorphic.Inline;

namespace RPGCore.Documentation.Samples.RPGCore.Data
{
	public class PolymorphicInlineBaseExplicit
	{
		#region types
		[SerializeType(typeof(CreateProcedure), "create")]
		[SerializeType(typeof(UpdateProcedure), "update")]
		[SerializeType(typeof(RemoveProcedure), "remove")]
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
