using System;

namespace ClientApp
{
	using FluentStateMachine;
	
	public class Program
	{
		private enum State   { on, off }
		private enum Trigger { twist   }
		
		private static StateMachine< State, Trigger > Lamp =
			new StateMachine< State, Trigger >( State.off )
				.For( State.off ).On( Trigger.twist, OffToOn, State.on  )
				.For( State.on  ).On( Trigger.twist, OnToOff, State.off )
				.For( State.on  ).OnEntry( EnteredOn  )
				.For( State.on  ).OnExit(  LeftOn     )
				.For( State.off ).OnEntry( EnteredOff )
				.For( State.off ).OnExit(  LeftOff    );
				
		
		public static void Main()
		{
			Console.Clear();
			Lamp.Start();
			Console.WriteLine( "Started" );
			Lamp.Trigger( Trigger.twist );
			Console.WriteLine( "Twisted" );
			Lamp.Trigger( Trigger.twist );
			Console.WriteLine( "Twisted" );
			//Lamp.Stop();
			//Console.WriteLine( "Stopped" );
            Console.ReadLine();
		}
		
		public static void OnToOff()
		{
			Console.WriteLine( "Lamp was turned off." );
		}
					 
		public static void OffToOn()
		{
			Console.WriteLine( "Lamp was turned on." );
		}
		
		public static void EnteredOn()
		{
			Console.WriteLine( "Lamp is on!" );
		}
		
		public static void LeftOn()
		{
			Console.WriteLine( "Lamp is turning off!" );
		}
		
		public static void EnteredOff()
		{
			Console.WriteLine( "Lamp is off!" );
		}
		
		public static void LeftOff()
		{
			Console.WriteLine( "Lamp is turning on!" );
		}
	}
}