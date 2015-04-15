using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komissarov.Nsudotnet.TaskScheduler
{
	///<summary>
	///<para> * * * * *  command to execute</para>
	///<para> │ │ │ │ │</para>
	///<para> │ │ │ │ │</para>
	///<para> │ │ │ │ └───── day of week (0 - 6) (0 to 6 are Sunday to Saturday)</para>
	///<para> │ │ │ └────────── month (0 - 11)</para>
	///<para> │ │ └─────────────── day of month (0 - 30)</para>
	///<para> │ └──────────────────── hour (0 - 23)</para>
	///<para> └───────────────────────── min (0 - 59)</para>
	///</summary>
	class CronParser
	{
		private int[] cronSchedule;

		public CronParser( string cronExpression )
		{
			string[] cronStrings = cronExpression.Split( new char[] { ' ' } );

			if ( cronStrings.Length != 5 )
				throw new ArgumentException( "Invalid expression" );

			cronSchedule = new int[] { 59, 23, 30, 11, 6 };

			for ( int i = 0; i < 5; ++i )
			{
				if ( cronStrings[i].Equals( "*" ) )
					cronSchedule[i] = -1;
				else
				{
					int n;
					bool isNumeric = int.TryParse( cronStrings[i], out n );

					if ( !isNumeric || n > cronSchedule[i] )
						throw new ArgumentException( "Invalid expression" );
					else
						cronSchedule[i] = n;
				}
			}
		}

		private int GetAdditionalDays( DateTime startTime, int dayOfWeek )
		{
			return ( 7 - ( int )startTime.DayOfWeek + dayOfWeek ) % 7;
		}

		public DateTime NextDate
		{
			get
			{
				DateTime now = DateTime.Now;

				DateTime next = new DateTime(
					now.Year,
					cronSchedule[3] == -1 ? now.Month : cronSchedule[3],
					cronSchedule[2] == -1 ? now.Day : cronSchedule[2],
					cronSchedule[1] == -1 ? now.Hour : cronSchedule[1],
					cronSchedule[0] == -1 ? now.Minute : cronSchedule[0],
					0 );

				for ( int i = 0; i < 4; ++i )
				{
					if ( next > now )
						break;

					if ( cronSchedule[i] == -1 )
						next = next.AddSomething( i + 1, 1 );
				}

				if ( next < now )
					next = next.AddYears( 1 );

				if ( cronSchedule[4] != -1 )
				{
					if ( cronSchedule[2] == -1 )
					{
						while ( ( int )next.DayOfWeek != cronSchedule[4] )
						{
							int daysToAdd = GetAdditionalDays( next, cronSchedule[4] );
							if ( cronSchedule[3] == -1 || next.AddDays( daysToAdd ).Month == cronSchedule[3] )
								next = next.AddDays( daysToAdd );
							else
								next.AddYears( 1 );
						}
					}
					else
					{
						while ( ( int )next.DayOfWeek != cronSchedule[4] )
						{
							if ( cronSchedule[3] == -1 )
								next = next.AddMonths( 1 );
							else
								next = next.AddYears( 1 );
						}
					}
				}

				return next;
			}
		}
	}
}
