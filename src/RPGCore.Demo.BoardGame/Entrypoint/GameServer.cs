using RPGCore.Behaviour;
using System;

namespace RPGCore.Demo.BoardGame
{
	public class GameServer
	{
		public GameView ServerView;
		public int ServerViewVersion;

		public event Action<GameViewProcedure> OnRemoteCall;

		public void OnClientConnected(LocalId localId, string displayName)
		{
			var playerJoinedProcedure = new PlayerJoinedProcedure()
			{
				DisplayName = displayName,
				OwnerId = localId
			};

			RemoteCall(playerJoinedProcedure);
		}

		public void OnClientDisconnected(LocalId localId)
		{
			var playerJoinedProcedure = new PlayerLeftProcedure()
			{
				OwnerId = localId
			};

			RemoteCall(playerJoinedProcedure);
		}

		public void AcceptInput(LocalId localId, GameCommand command)
		{
			if (command is DeclareResourceCommand declareResourceCommand)
			{
				var procedure = new DeclareResourceProcedure()
				{
					Player = localId,
					ResourceIdentifier = declareResourceCommand.ResourceIdentifier
				};

				RemoteCall(procedure);
			}
			else if (command is PlaceResourceCommand placeResourceCommand)
			{
				var procedure = new PlaceResourceProcedure()
				{
					Player = localId,
					ResourceIdentifier = placeResourceCommand.ResourceIdentifier,
					ResourcePosition = placeResourceCommand.ResourcePosition
				};

				RemoteCall(procedure);
			}
			else if (command is BuildBuildingCommand buildBuildingCommand)
			{
				var procedure = new BuildBuildingProcedure()
				{
					Player = localId,
					BuildingIdentifier = buildBuildingCommand.BuildingIdentifier,
					BuildingPosition = buildBuildingCommand.BuildingPosition,
					Offset = buildBuildingCommand.Offset,
					Orientation = buildBuildingCommand.Orientation
				};

				RemoteCall(procedure);
			}
		}

		private void RemoteCall(GameViewProcedure procedure)
		{
			ServerView.Apply(procedure);

			OnRemoteCall?.Invoke(procedure);
		}
	}
}
