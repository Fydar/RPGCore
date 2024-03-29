using NUnit.Framework;
using RPGCore.Projects.UnitTests.Utilities;

namespace RPGCore.Projects.UnitTests.Project;

[TestFixture(Description = "Test the functionality of the ProjectExplorer to create new projects.", TestOf = typeof(ProjectExplorer))]
public class CreatingProjectShould
{
	[SetUp]
	public void Setup()
	{
	}

	[Test(Description = "Test the functionality of the ProjectExplorer to create blank projects."), Parallelizable]
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
