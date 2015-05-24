using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;

namespace Komissarov.Nsudotnet.TaskScheduler
{
	static class TaskScheduler
	{
		private delegate void SchedulerTask( object[] arguments );

		private class SchedulerJob : IJob
		{
			private SchedulerTask task;

			public SchedulerJob( SchedulerTask task )
			{
				this.task = task;
			}

			public void Execute( object argument )
			{
				task( ( object[] )argument );
			}
		}

		private static ThreadPool pool;
		private static ConcurrentDictionary<IJob, SchedulerJob> table;

		static TaskScheduler( )
		{
			pool = new ThreadPool( );
			table = new ConcurrentDictionary<IJob, SchedulerJob>( );
		}

		//body for threadpool
		private static void DelayedExecutionBody( object[] arguments )
		{
			IJob job = ( IJob )arguments[0];
			TimeSpan delay = ( TimeSpan )arguments[1];
			object argument = arguments[2];

			Thread.Sleep( delay );
			job.Execute( argument );
			SchedulerJob outJob;
			table.TryRemove( job, out outJob );
		}

		//body for threadpool
		private static void PeriodicExecutionBody( object[] arguments )
		{
			IJob job = ( IJob )arguments[0];
			TimeSpan period = ( TimeSpan )arguments[1];
			object argument = arguments[2];

			try
			{
				while ( true )
				{
					Thread.Sleep( period );
					job.Execute( argument );
				}
			}
			catch ( ThreadInterruptedException )
			{
				SchedulerJob outJob;
				table.TryRemove( job, out outJob );
				return;
			}
		}

		//body for threadpool
		private static void CronExecutionBody( object[] arguments )
		{
			IJob job = ( IJob )arguments[0];
			CronParser cronParser = ( CronParser )arguments[1];
			object argument = arguments[2];

			try
			{
				while ( true )
				{
					DateTime next = cronParser.CalcNextDate( );
					Thread.Sleep( next - DateTime.Now );
					job.Execute( argument );
				}
			}
			catch ( ThreadInterruptedException )
			{
				SchedulerJob outJob;
				table.TryRemove( job, out outJob );
				return;
			}
		}

		public static void ScheduleDelayedJob( IJob job, TimeSpan delay, object argument = null )
		{
			object[] arguments = new object[] { job, delay, argument };
			var delayedJob = new SchedulerJob( DelayedExecutionBody );
			table.TryAdd( job, delayedJob );
			pool.LaunchTask( delayedJob, arguments );
		}

		public static void SchedulePeriodicJob( IJob job, TimeSpan period, object argument = null )
		{
			object[] arguments = new object[] { job, period, argument };
			var periodicJob = new SchedulerJob( PeriodicExecutionBody );
			table.TryAdd( job, periodicJob );
			pool.LaunchTask( periodicJob, arguments );
		}

		public static void SchedulePeriodicJob( IJob job, string cronExpression, object argument = null )
		{
			CronParser parser = new CronParser( cronExpression );

			object[] arguments = new object[] { job, parser, argument };
			var cronJob = new SchedulerJob( CronExecutionBody );
			table.TryAdd( job, cronJob );
			pool.LaunchTask( cronJob, arguments );
		}

		public static void CancelJob( IJob job )
		{
			SchedulerJob jobToCancel;
			if ( table.TryRemove( job, out jobToCancel ) )
				pool.CancelTask( jobToCancel );
		}
	}
}
