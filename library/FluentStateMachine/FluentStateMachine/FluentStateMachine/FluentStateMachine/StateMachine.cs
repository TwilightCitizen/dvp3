/* Name:     David A. Clark, Jr.
 * Student:  0004796375
 * Class:    Development Portfolio 3 (MDV239-O)
 * Term:     C201904-01
 * Exercise: FluentStateMachine Library
 * Synopsis: A library for specifying program behavior as actions
 *           associated with the transition from program state to
 *           program state via triggers with a fluent instantiation
 *           syntax.
 * Date:     April 20, 2019 */

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace FluentStateMachine
{
    /* State Machine for Mapping States to States on Triggers with Associated Actions */

	public class StateMachine< TState, TTrigger > where TState : Enum  where TTrigger : Enum 
	{
        /* Current State of the Machine */

		public TState State   { get; private set; }
		
        /* Running Status of the Machine */

		public bool   Running { get; private set; }

        /* Queue for External Trigger Insertion and Internal Processing */
		
		private ConcurrentQueue< TTrigger > Triggers = new ConcurrentQueue< TTrigger >();
		
        /* Mappings from State Tranisitions to Actions and Triggers to State Transitions */

		internal Dictionary< String, Action > TransitionActions = new Dictionary< String, Action >();
		internal Dictionary< String, TState > Transitions       = new Dictionary< String, TState >();
	
        /* State Machine Must Have a Start/Default/Initial State */

		public StateMachine( TState startState )
		{
			State = startState;
		}
		
        /* Enable Fluent Syntax: Return Builder from For( State ) Invocation */

		public ForState< TState, TTrigger > For( TState state )
		{
			return new ForState< TState, TTrigger >( this, state );
		}
		
        /* Asynchronous Machine Loop: Processes Triggers as State Transitions and Transition Actions */

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
						
						if( TransitionActions.ContainsKey( exitAction  ) ) TransitionActions[ exitAction  ]?.Invoke();
						if( TransitionActions.ContainsKey( transAction ) ) TransitionActions[ transAction ]?.Invoke();
						if( TransitionActions.ContainsKey( entryAction ) ) TransitionActions[ entryAction ]?.Invoke();
						
						State = nextState;
					}
                    
					// Thread.Sleep( 0 );
				}
			} );
		}
		
        /* Enqueue a Trigger to the State Machine */

		public void Trigger( TTrigger trigger )
		{
			Triggers.Enqueue( trigger );
		}
		
        /* Stop the State Machine */

		public void Stop()
		{
			Running = false;
		}
	}
	
    /* Builder to Enable Fluent Instantiation Syntax for State Machine */

	public class ForState< TState, TTrigger > where TState : Enum  where TTrigger : Enum 
	{
        /* State Machine and State Being Built for It */

		private StateMachine< TState, TTrigger > Machine;
		private TState                           State;
		
        /* Builder Only Accessible to State Machine */

		internal ForState( StateMachine< TState, TTrigger> machine, TState state )
		{
			Machine = machine;
			State   = state;
		}
		
        /* Global/On-Every Entry Action for the State */

		public StateMachine< TState, TTrigger > OnEntry( Action action )
		{
			Machine.TransitionActions[ "-->" + State ] = action;
			
			return Machine;
		}

        /* Global/On-Every Exit Action for the State */

        public StateMachine< TState, TTrigger > OnExit( Action action )
		{
			Machine.TransitionActions[ State + "-->" ] = action;
			
			return Machine;
		}
		
        /* Action Occurring During Transition from State to Another. */

		public StateMachine< TState, TTrigger > On( TTrigger trigger, Action action, TState nextState )
		{
			Machine.Transitions[       State + "<->" + trigger   ] = nextState;
			Machine.TransitionActions[ State + "-->" + nextState ] = action;
			
			return Machine;
		}
	}
}