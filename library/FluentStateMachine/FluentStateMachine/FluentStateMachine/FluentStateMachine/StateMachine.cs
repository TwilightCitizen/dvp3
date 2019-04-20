using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace FluentStateMachine
{
	public class StateMachine< TState, TTrigger > where TState : Enum  where TTrigger : Enum 
	{
		public TState State   { get; private set; }
		
		public bool   Running { get; private set; }
		
		private ConcurrentQueue< TTrigger > Triggers = new ConcurrentQueue< TTrigger >();
		
		internal Dictionary< String, Action > TransitionActions = new Dictionary< String, Action >();
		internal Dictionary< String, TState > Transitions       = new Dictionary< String, TState >();
	
		public StateMachine( TState startState )
		{
			State = startState;
		}
		
		public ForState< TState, TTrigger > For( TState state )
		{
			return new ForState< TState, TTrigger >( this, state );
		}
		
		public async void Start()
		{
			Running = true;
			
			await Task.Run( () =>
			{
				while( Running )
				{
					TTrigger trigger;
					
					if( Triggers.TryDequeue( out trigger ) )
					{
						var exitAction  = State + "-->";
						var nextState   = Transitions[ State + "<->" + trigger ];
						var transAction = State + "-->" + nextState;
						var entryAction = "-->" + nextState;
						
						if( TransitionActions.ContainsKey( exitAction  ) ) TransitionActions[ exitAction  ].Invoke();
						if( TransitionActions.ContainsKey( transAction ) ) TransitionActions[ transAction ].Invoke();
						if( TransitionActions.ContainsKey( entryAction ) ) TransitionActions[ entryAction ].Invoke();
						
						State = nextState;
					}
					
					Thread.Sleep( 0 );
				}
			} );
		}
		
		public void Trigger( TTrigger trigger )
		{
			Triggers.Enqueue( trigger );
		}
		
		public void Stop()
		{
			Running = false;
		}
	}
	
	public class ForState< TState, TTrigger > where TState : Enum  where TTrigger : Enum 
	{
		private StateMachine< TState, TTrigger > Machine;
		private TState                           State;
		
		internal ForState( StateMachine< TState, TTrigger> machine, TState state )
		{
			Machine = machine;
			State   = state;
		}
		
		public StateMachine< TState, TTrigger > OnEntry( Action action )
		{
			Machine.TransitionActions[ "-->" + State ] = action;
			
			return Machine;
		}
		
		public StateMachine< TState, TTrigger > OnExit( Action action )
		{
			Machine.TransitionActions[ State + "-->" ] = action;
			
			return Machine;
		}
		
		public StateMachine< TState, TTrigger > On( TTrigger trigger, Action action, TState nextState )
		{
			Machine.Transitions[       State + "<->" + trigger   ] = nextState;
			Machine.TransitionActions[ State + "-->" + nextState ] = action;
			
			return Machine;
		}
	}
}