using RPGCore.Data.Polymorphic.Inline;

namespace RPGCore.Documentation.Samples.RPGCore.Data
{
	public class PolymorphicInlineChildExplicit
	{
		#region types
		public interface IProcedure
		{
		}

		[SerializeThisType(typeof(IProcedure), "create")]
		public class CreateProcedure : IProcedure
		{
		}

		[SerializeThisType(typeof(IProcedure), "update")]
		public class UpdateProcedure : IProcedure
		{
		}

		[SerializeThisType(typeof(IProcedure), "remove")]
		public class RemoveProcedure : IProcedure
		{
		}
		#endregion types
	}
}
