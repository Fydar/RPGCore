using NUnit.Framework;
using RPGCore.Data.Polymorphic;
using RPGCore.Data.Polymorphic.Inline;
using RPGCore.Data.UnitTests.Utility;

namespace RPGCore.Data.UnitTests.Polymorphic.Inline
{
	[TestFixture(TestOf = typeof(PolymorphicOptionsExtensions))]
	public class PolymorphicOptionsExtensionsShould
	{
		[SerializeBaseType(typeof(CreateProcedure), TypeName.Name)]
		[SerializeBaseType(typeof(UpdateProcedure), TypeName.Name)]
		[SerializeBaseType(typeof(SomethingProcedure), TypeName.Name)]
		public interface IProcedure
		{
		}

		public class CreateProcedure : IProcedure
		{
			public string CreateString { get; set; } = "";
		}

		[SerializeSubType(typeof(IProcedure))]
		public class DestroyProcedure : IProcedure
		{
			public string DestroyString { get; set; } = "";
		}

		public class UpdateProcedure : IProcedure
		{
			public string UpdateString { get; set; } = "";
		}

		public class SomethingProcedure : CreateProcedure
		{
			public string SomethingString { get; set; } = "";
		}

		[Test, Parallelizable]
		public void AllowConfigurationViaAttributes()
		{
			var options = new PolymorphicOptionsBuilder
			{
				CaseInsensitive = true,
				DescriminatorName = "$type"
			};

			options.UseInline();

			var configuration = options.Build();

			RenderingUtility.RenderConfiguration(TestContext.Out, configuration);
		}
	}
}
