using System;

public class CommandUsageAttribute : Attribute
{
	public string Help = "No help provided";

	public CommandUsageAttribute ()
	{

	}

	public CommandUsageAttribute (string _help)
	{
		Help = _help;
	}
}
