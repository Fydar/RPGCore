using RPGCore.FileTree.FileSystem;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RPGCore.FileTree.CommandLine
{
	public sealed class InteractiveFileTree
	{


		public InteractiveFileTree()
		{
		}

		public async Task RunAsync()
		{
			var directory = new DirectoryInfo("root");
			var archive = new FileSystemArchive(directory);

			bool skipToken = false;

			archive.OnEntryChanged += (args) =>
			{
				skipToken = true;
			};

			while (true)
			{
				Console.Clear();
				Console.WriteLine(directory.FullName);
				Console.WriteLine();

				DrawTree(archive);

				while (!skipToken)
				{
					await Task.Delay(10);
				}
				skipToken = false;
			}
		}

		private static void DrawTree(IArchive archive)
		{
			DrawTreeRecursive(archive.RootDirectory, "");
		}

		private static void DrawTreeRecursive(IReadOnlyArchiveDirectory archiveDirectory, string linePrefix)
		{
			foreach (var directory in archiveDirectory.Directories.All.OrderBy(d => d.Name))
			{
				Console.WriteLine($"{linePrefix} |- {directory.Name}");

				DrawTreeRecursive(directory, $"{linePrefix}    ");
			}

			foreach (var file in archiveDirectory.Files.OrderBy(f => f.Name))
			{
				Console.WriteLine($"{linePrefix} |- {file.Name}");
			}
		}
	}
}
