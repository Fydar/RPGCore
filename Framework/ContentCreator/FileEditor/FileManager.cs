using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ContentCreator.FileEditor
{
    public class FileManager : IDisposable
    {
		private FileSystemWatcher Watcher;

		public FileManager(string filePath)
		{
			string[] data = File.ReadAllLines(filePath);
			Console.WriteLine("Created file manager");
			Console.WriteLine(string.Join('\n', data));

			var info = new FileInfo(filePath);
			var folder = info.Directory;

			Watcher = new FileSystemWatcher(folder.FullName);
			Watcher.Changed += (fileWatcher, args) => Console.WriteLine(args.ChangeType + " at: " + args.FullPath);
			Watcher.Deleted += (fileWatcher, args) => Console.WriteLine(args.ChangeType + " at: " + args.FullPath);
			Watcher.Error += (fileWatcher, args) => Console.WriteLine(args.GetException());
			Watcher.Renamed += (fileWatcher, args) => Console.WriteLine(args.ChangeType + " at: " + args.FullPath);
			
			Watcher.NotifyFilter = NotifyFilters.LastWrite;

			Watcher.EnableRaisingEvents = true;
		}

		public void Dispose()
		{
			Console.WriteLine("Disposing FileManager");
		}
	}
}
