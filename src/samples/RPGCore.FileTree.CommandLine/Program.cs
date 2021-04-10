using System.Threading.Tasks;

namespace RPGCore.FileTree.CommandLine
{
	internal sealed class Program
	{
		private static async Task Main(string[] args)
		{
			var fileTree = new InteractiveFileTree();

			await fileTree.RunAsync();
		}
	}
}
