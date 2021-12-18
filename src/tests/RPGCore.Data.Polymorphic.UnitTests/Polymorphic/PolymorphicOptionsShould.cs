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
			var options = new PolymorphicOptionsBuilder
			{
				CaseInsensitive = true,
				DescriminatorName = "$type"
			};

			options.UseKnownBaseType(typeof(IProcedure), baseType =>
			{
				baseType.UseResolvedSubTypes(resolveOptions =>
				{
					resolveOptions.TypeNaming = TypeNameNamingConvention.Instance;
				});

				baseType.UseSubType(typeof(SomethingProcedure), subType =>
				{
					subType.Descriminator = "something";
					subType.Aliases.Add("UseSomething");
				});
			});

			options.UseKnownSubType(typeof(UpdateProcedure), subType =>
			{
				subType.UseBaseType(typeof(IProcedure), options =>
				{
					options.Descriminator = "update";
					options.Aliases.Add("doUpdate");
				});

				subType.UseResolvedBaseTypes(options =>
				{
					options.IdentifyWith(identity =>
					{
						identity.Aliases.Add(identity.BaseType.Name + "-update");
					});
				});
			});

			options.UseKnownBaseType(typeof(IProcedure), baseType =>
			{
				baseType.UseResolvedSubTypes(resolveOptions =>
				{
					resolveOptions.TypeAliasing.Add(TypeGuidNamingConvention.Instance);
				});
			});

			var configuration = options.Build();

			RenderingUtility.RenderConfiguration(TestContext.Out, configuration);
		}
	}
}
