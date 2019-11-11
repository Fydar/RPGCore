using RPGCore.Behaviour;
using RPGCore.Traits;
using System;
using System.Collections.Generic;

namespace RPGCore.View
{
	public class ViewCharacter : Entity
	{
		public TraitCollection Traits;

		public EntityRef SelectedTarget;

		public ViewCharacter()
		{
			Id = new EntityRef ()
			{
				EntityId = LocalId.NewId ()
			};
		}
	}

	public abstract class Entity
	{
		public EntityRef Id;

		public event Action<object> OnCreateEntityPacket;
	}

	public struct EntityRef
	{
		public LocalId EntityId;
	}

	public class GameView
	{
		public EventCollection<EntityRef, Entity> Entities = new EventCollection<EntityRef, Entity> ();

		public List<EntityRef> Characters = new List<EntityRef> ();
		public EventField<string> MapName = new EventField<string> ();
	}

	public class ViewDispatcher
	{
		public event Action<string> OnPacketGenerated;

		public GameView View { get; }

		public ViewDispatcher (GameView view)
		{
			View = view;

			View.Entities.Handlers[this].Add (new EntitySyncCollectionHandler(this));
		}

		class EntitySyncCollectionHandler : IEventCollectionHandler<EntityRef, Entity>
		{
			private readonly ViewDispatcher dispatcher;

			public EntitySyncCollectionHandler(ViewDispatcher dispatcher)
			{
				this.dispatcher = dispatcher;
			}

			public void Dispose ()
			{
			}

			public void OnAdd (EntityRef key, Entity value)
			{
			}

			public void OnRemove (EntityRef key, Entity value)
			{
			}
		}
	}

	public class Client
	{
		public void Run ()
		{

		}
	}

	public class Server
	{
		public void Run ()
		{
			var view = new GameView ();
			var dispatcher = new ViewDispatcher (view);

			var character = new ViewCharacter ();
			view.Entities.Add (character.Id, character);
		}
	}
}
