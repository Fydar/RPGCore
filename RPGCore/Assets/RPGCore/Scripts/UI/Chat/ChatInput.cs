using RPGCore.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatInput : MonoBehaviour
{
	public class DemoConsole : ConsoleMenu
	{
		public class GiveCommand : ConsoleAction
		{
			[CommandUsage]
			void Nickname (string itemId)
			{
				Chat.Instance.Log ("Giving " + itemId + ".");
			}
		}

		public override void BuildCommands ()
		{
			Commands = new Dictionary<string, ConsoleCommand> ();

			GiveCommand nicknameCmd = new GiveCommand ();
			Commands.Add ("give", nicknameCmd);
		}
	}

	public ConsoleCommand CurrentCommand;

	public InputField Field;
	public Button Submit;
	private bool allowEnter;
	private int lookback = -1;

	private List<string> lastInputs = new List<string> ();

	private void Awake ()
	{
		CurrentCommand = new DemoConsole ();
	}

	private void Update ()
	{
		bool send = (Input.GetKey (KeyCode.Return) || Input.GetKey (KeyCode.KeypadEnter));
		if (allowEnter && Field.text.Length > 0 && send)
		{
			Send ();
			lookback = -1;
			allowEnter = false;
		}
		else if (send || (!Field.isFocused && Input.GetKey (KeyCode.T)))
		{
			Field.Select ();
			if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift))
			{
				Field.text = "/";
				Field.selectionAnchorPosition = 1;
				Field.selectionFocusPosition = 1;
			}
		}
		else
		{
			allowEnter = Field.isFocused || Field.isFocused;
		}

		if (Field.isFocused)
		{
			if (Input.GetKeyDown (KeyCode.UpArrow))
			{
				if (lookback + 1 < lastInputs.Count)
				{
					lookback++;
					Field.text = lastInputs[lastInputs.Count - 1 - lookback];
					Field.selectionAnchorPosition = Field.text.Length;
					Field.selectionFocusPosition = Field.text.Length;
				}
			}
			else if (Input.GetKeyDown (KeyCode.DownArrow))
			{
				if (lookback - 1 >= 0)
				{
					lookback--;
					Field.text = lastInputs[lastInputs.Count - 1 - lookback];
					Field.selectionAnchorPosition = Field.text.Length;
					Field.selectionFocusPosition = Field.text.Length;
				}
				else
				{
					lookback = -1;
					Field.text = "";
				}
			}
			else if (Input.anyKeyDown)
			{
				lookback = -1;
			}
		}
	}

	public void Send ()
	{
		string text = Field.text;
		if (string.IsNullOrEmpty (text))
			return;

		if (text.StartsWith ("/", StringComparison.Ordinal))
		{
			string command = text.Substring (1);
			ExecuteCommand (command);
		}
		else
		{
			Chat.Instance.Log ("<color=#777>Character:</color> " + text);
		}

		lastInputs.Add (text);
		if (lastInputs.Count > 30)
			lastInputs.RemoveAt (lastInputs.Count - 1);

		Field.text = "";
	}

	public void ExecuteCommand (string command)
	{
		string[] parameters = command.Split (new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);

		CurrentCommand.Run (parameters);
	}
}
