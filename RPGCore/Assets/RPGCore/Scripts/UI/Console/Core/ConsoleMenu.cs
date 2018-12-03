using System.Collections.Generic;
using UnityEngine;

public abstract class ConsoleMenu : ConsoleCommand
{
	public Dictionary<string, ConsoleCommand> Commands;

	protected ConsoleMenu ()
	{
		BuildCommands ();
	}

	public abstract void BuildCommands ();

	public override void Run (ref string[] Parameters, int offset = 0)
	{
		if (Parameters.Length == offset)
		{
			Debug.Log (Help ());
			return;
		}

		string commandID = Parameters[offset].ToLower ();

		offset++;

		Commands[commandID].Run (ref Parameters, offset);
	}

	public override string Help ()
	{
		string helpString = "Sub Menus:";

		foreach (KeyValuePair<string, ConsoleCommand> command in Commands)
		{
			helpString += "\n\t" + command.Key + ": " + command.Value.GetType ().Name;
		}

		return helpString;
	}
}
