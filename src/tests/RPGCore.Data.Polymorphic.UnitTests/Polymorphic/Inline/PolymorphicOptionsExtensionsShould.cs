using NUnit.Framework;
using RPGCore.Data.Polymorphic;
using RPGCore.Data.Polymorphic.Inline;

namespace RPGCore.Data.UnitTests.Polymorphic.Inline
{
	[TestFixture(TestOf = typeof(PolymorphicOptionsExtensions))]
	public class PolymorphicOptionsExtensionsShould
	{
		[SerializeType(typeof(CreateProcedure), TypeName.Name)]
		[SerializeType(typeof(UpdateProcedure), TypeName.Name)]
		[SerializeType(typeof(SomethingProcedure), TypeName.Name)]
		public interface IProcedure
		{
		}

		public class CreateProcedure : IProcedure
		{
			public string CreateString { get; set; } = "";
		}

		[SerializeThisType(typeof(IProcedure))]
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
			var options = new PolymorphicOptions
			{
				CaseInsensitive = true,
				DescriminatorName = "$type"
			};

			options.UseInline();

			var configuration = options.Build();
		}
	}
}
