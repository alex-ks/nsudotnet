using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;

namespace Komissarov.Nsudotnet.TaskScheduler
{
	class ThreadPool
	{
		private ConcurrentDictionary<IJob, PooledThread> workingThreads;
		private ConcurrentBag<PooledThread> pooledThreads;
		

		private class PooledThread
		{
			public static int ThreadCounter
			{
				set;
				get;
			}

			private ConcurrentQueue<Tuple<IJob, object>> tasks;
			private ThreadPool pool;
			private Thread thread;

			static PooledThread( )
			{
				ThreadCounter = 0;
			}

			public PooledThread( ThreadPool pool )
			{
				tasks = new ConcurrentQueue<Tuple<IJob, object>>( );
				this.pool = pool;
				thread = new Thread( this.Execute );
				thread.IsBackground = true;
				thread.Name = "Thread #" + ThreadCounter++;
			}

			public void Start( )
			{
				thread.Start( );
			}

			public void CancelTask( )
			{
				thread.Interrupt( );
			}

			public void Abort( )
			{
				thread.Abort( );
			}

			public void PushTask( IJob task, object argument )
			{
				tasks.Enqueue( new Tuple<IJob, object>( task, argument ) );
			}

			public void Execute( )
			{
				while ( true )
				{
					Tuple<IJob, object> task = default( Tuple<IJob, object> );
					try
					{
						if ( tasks.TryDequeue( out task ) )
							task.Item1.Execute( task.Item2 );
					}
					catch ( ThreadInterruptedException )
					{
						if ( task != default( Tuple<IJob, object> ) )
						{
							PooledThread thread;
							pool.workingThreads.TryRemove( task.Item1, out thread );
							pool.pooledThreads.Add( this );
						}
						continue;
					}

					PooledThread outThread;
					pool.workingThreads.TryRemove( task.Item1, out outThread );
					pool.pooledThreads.Add( this );

					try
					{
						lock ( this )
						{
							Monitor.Wait( this );
						}
					}
					catch ( ThreadInterruptedException )
					{
						return;
					}
				}
			}
		}

		public ThreadPool( )
		{
			pooledThreads = new ConcurrentBag<PooledThread>( );
			workingThreads = new ConcurrentDictionary<IJob, PooledThread>( );
		}

		public void LaunchTask( IJob task, object argument )
		{
			if ( pooledThreads.IsEmpty )
			{
				PooledThread pooledThread = new PooledThread( this );

				workingThreads.TryAdd( task, pooledThread );
				pooledThread.PushTask( task, argument );
				pooledThread.Start( );
			}
			else
			{
				PooledThread pooledThread;
				pooledThreads.TryTake( out pooledThread );
				pooledThread.PushTask( task, argument );

				workingThreads.TryAdd( task, pooledThread );

				lock ( pooledThread )
				{
					Monitor.Pulse( pooledThread );
				}
			}
		}

		public void CancelTask( IJob task )
		{
			PooledThread pooledThread;
			workingThreads.TryRemove( task, out pooledThread );
			if ( pooledThread != default( PooledThread ) )
				pooledThread.CancelTask( );
		}

		public void CancelAllTasks( )
		{
			foreach ( var threadDesc in workingThreads )
				threadDesc.Value.CancelTask( );
			
			workingThreads.Clear( );
		}

		public void Exterminate( )
		{
			while ( !pooledThreads.IsEmpty )
			{
				PooledThread pooledThread;
				pooledThreads.TryTake( out pooledThread );
				pooledThread.CancelTask( );
			}

			foreach ( var threadDesc in workingThreads )
			{
				threadDesc.Value.Abort( );
			}
			workingThreads.Clear( );

			PooledThread.ThreadCounter = 0;
		}
	}
}
