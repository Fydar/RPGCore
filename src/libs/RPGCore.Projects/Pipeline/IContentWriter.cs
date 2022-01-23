using System.IO;
using System.Threading.Tasks;

namespace RPGCore.Projects.Pipeline;

/// <summary>
/// Provides a mechanism for deferring the writing of content in the import pipeline.
/// </summary>
public interface IContentWriter
{
	Task WriteContentAsync(Stream destination);
}
