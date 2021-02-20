using System.Collections.Generic;

namespace RPGCore.Packages
{
	public interface IResourceDependency
	{
		bool IsValid { get; }
		string Key { get; }
		IResource? Resource { get; }
		IReadOnlyDictionary<string, string> Metadata { get; }
	}
}
