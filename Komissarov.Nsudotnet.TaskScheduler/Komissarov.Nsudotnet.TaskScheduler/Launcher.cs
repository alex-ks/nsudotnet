using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;

namespace Komissarov.Nsudotnet.TaskScheduler
{
	class Launcher
	{
		private	static object padLock = new object( );

		public static void DoMagic( params object[] arg )
		{
			Thread.Sleep( 900 );
			lock ( padLock )
			{
				Console.WriteLine( Thread.CurrentThread.Name + " does Magic!" );
			}
		}

		class MagicDoer : IJob
		{
			public void Execute( object argument )
			{
				Console.WriteLine( Thread.CurrentThread.Name + " does Magic!" );
			}
		}

		public static void Main( )
		{
			MagicDoer job = new MagicDoer( );
			TaskScheduler.SchedulePeriodicJob( job, "* * * * *" );
			Thread.Sleep( 60000 );
			TaskScheduler.CancelJob( job );
			Console.ReadKey( );
		}
	}
}
