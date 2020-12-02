using NUnit.Framework;
using RPGCore.Projects.UnitTests.Utilities;

namespace RPGCore.Projects.UnitTests.Project
{
	[TestFixture(TestOf = typeof(ProjectExplorer))]
	public class CreatingProjectShould
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test, Parallelizable]
		public void CreateBlankProject()
		{
			var importPipeline = ImportPipeline.Create().Build();
			string projectPath = TestUtilities.CreateFilePath("project");

			using (var explorer = ProjectExplorer.CreateProject(projectPath, importPipeline))
			{
				explorer.Definition.Properties.Name = "TestName1";
				explorer.Definition.SaveChanges();
			}
		}
	}
}
