using System;

namespace RPGCore.Documentation;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class PresentOutputAttribute : Attribute
{
	public OutputFormat Format { get; }
	public string Name { get; }

	public PresentOutputAttribute(OutputFormat format, string name)
	{
		Format = format;
		Name = name;
	}
}
