using System.IO;
using System.Threading.Tasks;

namespace RPGCore.Packages
{
	public abstract class ResourceImporter
	{
		public abstract string ImportExtensions { get; }

		public abstract void BuildResource(IResource resource, Stream writer);
    }
}
