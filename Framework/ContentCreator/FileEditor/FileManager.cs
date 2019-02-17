using System;
using System.IO;

namespace ContentCreator.FileEditor
{
	public class FileManager : IDisposable
	{
		public string FilePath { get; }
		private FileSystemWatcher Watcher;

		public event Action OnChanged;

		public FileManager (string filePath)
		{
			FilePath = filePath;

			Console.WriteLine ("Created file manager");
			Console.WriteLine (ReadFile ());

			var info = new FileInfo (filePath);
			var folder = info.Directory;

			Watcher = new FileSystemWatcher (folder.FullName);
			Watcher.EnableRaisingEvents = true;

			Watcher.Created += ChangedHandler;
			Watcher.Changed += ChangedHandler;

			Watcher.Deleted += (fileWatcher, args) => Console.WriteLine (args.ChangeType + " at: " + args.FullPath);
			Watcher.Error += (fileWatcher, args) => Console.WriteLine (args.GetException ());
			Watcher.Renamed += (fileWatcher, args) => Console.WriteLine (args.ChangeType + " at: " + args.FullPath);

			// Watcher.NotifyFilter = NotifyFilters.LastWrite;
		}

		private void ChangedHandler (object sender, FileSystemEventArgs fileSystemEventArgs)
		{
			Console.WriteLine (fileSystemEventArgs.ChangeType + " at: " + fileSystemEventArgs.FullPath);
			OnChanged?.Invoke ();
		}

		public void Dispose ()
		{
			Console.WriteLine ("Disposing FileManager");
			Watcher.Dispose ();
		}

		public string ReadFile ()
		{
			System.Threading.Thread.Sleep (250);
			string[] data = File.ReadAllLines (FilePath);
			return string.Join ('\n', data);
		}

		public void WriteFile (string data)
		{
			File.WriteAllText (FilePath, data);
		}
	}
}
