using System;

namespace RPGCore.View
{
	public class Server
	{
		public void Run ()
		{
			var serverView = new GameView ();
			var clientView = new GameView ();

			var dispatcher = new ViewDispatcher (serverView);
			dispatcher.OnPacketGenerated += clientView.Apply;


			var character = new ViewCharacter ();
			serverView.Entities.Add (character.Id, character);

			Console.WriteLine ("Server View");
			Console.WriteLine (string.Join (", ", serverView.Entities));
			Console.WriteLine ("Server View");
			Console.WriteLine (string.Join (", ", clientView.Entities));
		}
	}
}
