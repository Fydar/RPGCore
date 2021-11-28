using NUnit.Framework;
using RPGCore.Data.Polymorphic;
using RPGCore.Data.Polymorphic.Naming;
using RPGCore.Data.UnitTests.Utility;

namespace RPGCore.Data.UnitTests.Polymorphic
{
	[TestFixture(TestOf = typeof(PolymorphicOptions))]
	public class PolymorphicOptionsShould
	{
		public interface IProcedure
		{
		}

		public class CreateProcedure : IProcedure
		{
			public string CreateString { get; set; } = "";
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
		public void AllowConfigurationOfComplexRelationships()
		{
			var options = new PolymorphicOptions
			{
				CaseInsensitive = true,
				DescriminatorName = "$type"
			};

			options.UseKnownBaseType(typeof(IProcedure), baseType =>
			{
				baseType.ResolveSubTypesAutomatically(resolveOptions =>
				{
					resolveOptions.TypeNaming = TypeNameNamingConvention.Instance;
					resolveOptions.TypeAliasing.Add(TypeGuidNamingConvention.Instance);
				});

				baseType.UseSubType(typeof(SomethingProcedure), subType =>
				{
					subType.Descriminator = "something";
					subType.Aliases.Add("UseSomething");
				});
			});

			options.UseKnownSubType(typeof(UpdateProcedure), subType =>
			{
				subType.Descriminator = "update";
				subType.Aliases.Add("doUpdate");

				subType.UseBaseType(typeof(IProcedure));

				subType.ResolveBaseTypesAutomatically();
			});

			var configuration = options.Build();

			RenderingUtility.RenderConfiguration(TestContext.Out, configuration);
		}
	}
}
