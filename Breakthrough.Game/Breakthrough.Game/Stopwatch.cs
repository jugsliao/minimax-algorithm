using System;

namespace Breakthrough.Game
{
	
	
	public class SimpleStopwatch
	{
		private DateTime StartTime;
        public TimeSpan Duration;
        public SimpleStopwatch()
		{
			
		}
		
		public void Start()
		{
			StartTime = DateTime.Now;
		}
		
		public double Stop()
		{
            Duration = DateTime.Now - StartTime;
            Console.WriteLine("AI> Time = {0} msecs", Duration.TotalMilliseconds);

            return Duration.TotalMilliseconds;
		}
	}
}
