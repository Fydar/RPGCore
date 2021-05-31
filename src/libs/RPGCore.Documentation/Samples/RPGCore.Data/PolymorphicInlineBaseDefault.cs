using RPGCore.Data.Polymorphic.Inline;

namespace RPGCore.Documentation.Samples.RPGCore.Data
{
	public class PolymorphicInlineBaseDefault
	{
		#region types
		[SerializeType]
		public interface IProcedure
		{
		}

		public class CreateProcedure : IProcedure
		{
		}

		public class UpdateProcedure : IProcedure
		{
		}

		public class RemoveProcedure : IProcedure
		{
		}
		#endregion types
	}
}
