using RPGCore.Data.Polymorphic.Inline;

namespace RPGCore.Documentation.Samples.RPGCore.Data
{
	public class PolymorphicInlineChildName
	{
		#region types
		public interface IProcedure
		{
		}

		[SerializeThisType("create")]
		public class CreateProcedure : IProcedure
		{
		}

		[SerializeThisType("update")]
		public class UpdateProcedure : IProcedure
		{
		}

		[SerializeThisType("remove")]
		public class RemoveProcedure : IProcedure
		{
		}
		#endregion types
	}
}
