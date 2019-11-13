using System;

namespace RPGCore.View
{
	public class Server
	{
		private GameView serverView;
		private GameView clientView;

		public void Run ()
		{
			serverView = new GameView ();
			clientView = new GameView ();

			var dispatcher = new ViewDispatcher (serverView);
			dispatcher.OnPacketGenerated += clientView.Apply;


			var character = new ViewCharacter ();
			character.Name.Value = "Unknown";
			serverView.Entities.Add (character.Id, character);

			LogCurrentState ();

			character.Name.Value = "New Name";

			LogCurrentState ();

			serverView.Entities.Remove (character.Id);

			LogCurrentState ();
		}

		private void LogCurrentState()
		{
			Console.WriteLine ();
			Console.WriteLine ("Server View");
			Console.WriteLine (string.Join (", ", serverView.Entities));
			Console.WriteLine ("Client View");
			Console.WriteLine (string.Join (", ", clientView.Entities));
		}
	}
}
