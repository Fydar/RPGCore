using NUnit.Framework;
using RPGCore.Packages.UnitTests.Utilities;
using System.IO;

namespace RPGCore.Packages.UnitTests
{
	[TestFixture(TestOf = typeof(ProjectDefinitionProperties))]
	public class EditDefinitionsShould
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test, Parallelizable]
		public void ModifyName()
		{
			var importPipeline = ImportPipeline.Create().Build();
			string projectPath = TestUtilities.CreateFilePath("project");
			string exportPath = Path.Combine(TestUtilities.CreateFilePath("export"), "TestName1.bpkg");

			using (var explorer = ProjectExplorer.CreateProject(projectPath, importPipeline))
			{
				explorer.Definition.Properties.Name = "TestName1";
				explorer.Definition.SaveChanges();
			}

			using (var explorer = ProjectExplorer.Load(projectPath, importPipeline))
			{
				Assert.That(explorer.Definition.Properties.Name, Is.EqualTo("TestName1"));

				explorer.ExportZippedToDirectory(new BuildPipeline()
				{
					ImportPipeline = importPipeline
				}, TestUtilities.CreateFilePath("export"));
			}

			using (var explorer = PackageExplorer.LoadFromFileAsync(exportPath).Result)
			{
				Assert.That(explorer.Definition.Properties.Name, Is.EqualTo("TestName1"));
			}
		}

		[Test, Parallelizable]
		public void ModifyNameAndVersion()
		{
			var importPipeline = ImportPipeline.Create().Build();
			string projectPath = TestUtilities.CreateFilePath("project");
			string exportPath = Path.Combine(TestUtilities.CreateFilePath("export"), "TestName1.bpkg");

			using (var explorer = ProjectExplorer.CreateProject(projectPath, importPipeline))
			{
				explorer.Definition.Properties.Name = "TestName1";
				explorer.Definition.Properties.Version = "1.0.0";
				explorer.Definition.SaveChanges();
			}

			using (var explorer = ProjectExplorer.Load(projectPath, importPipeline))
			{
				Assert.That(explorer.Definition.Properties.Name, Is.EqualTo("TestName1"));
				Assert.That(explorer.Definition.Properties.Version, Is.EqualTo("1.0.0"));

				explorer.ExportZippedToDirectory(new BuildPipeline()
				{
					ImportPipeline = importPipeline,
				}, TestUtilities.CreateFilePath("export"));
			}

			using (var explorer = PackageExplorer.LoadFromFileAsync(exportPath).Result)
			{
				Assert.That(explorer.Definition.Properties.Name, Is.EqualTo("TestName1"));
				Assert.That(explorer.Definition.Properties.Version, Is.EqualTo("1.0.0"));
			}
		}

		[Test, Parallelizable]
		public void ModifyVersion()
		{
			var importPipeline = ImportPipeline.Create().Build();
			string projectPath = TestUtilities.CreateFilePath("project");
			string exportPath = Path.Combine(TestUtilities.CreateFilePath("export"), "project.bpkg");

			using (var explorer = ProjectExplorer.CreateProject(projectPath, importPipeline))
			{
				explorer.Definition.Properties.Version = "1.0.0";
				explorer.Definition.SaveChanges();
			}

			using (var explorer = ProjectExplorer.Load(projectPath, importPipeline))
			{
				Assert.That(explorer.Definition.Properties.Version, Is.EqualTo("1.0.0"));

				explorer.ExportZippedToDirectory(new BuildPipeline()
				{
					ImportPipeline = importPipeline,
				}, TestUtilities.CreateFilePath("export"));
			}

			using (var explorer = PackageExplorer.LoadFromFileAsync(exportPath).Result)
			{
				Assert.That(explorer.Definition.Properties.Version, Is.EqualTo("1.0.0"));
			}
		}
	}
}
