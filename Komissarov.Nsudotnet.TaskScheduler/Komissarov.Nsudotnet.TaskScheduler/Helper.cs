using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komissarov.Nsudotnet.TaskScheduler
{
	static class Helper
	{
		///<summary>
		///<para>Adds value to the following fields:</para>
		///<para>id = 0 for seconds</para>
		///<para>...</para>
		///<para>id = 5 for years</para>
		///</summary>
		public static DateTime AddSomething( this DateTime date, int somethingId, int value )
		{
			switch ( somethingId )
			{
				case 0:
					return date.AddSeconds( value );

				case 1:
					return date.AddMinutes( value );

				case 2:
					return date.AddHours( value );

				case 3:
					return date.AddDays( value );

				case 4:
					return date.AddMonths( value );

				case 5:
					return date.AddYears( value );

				default:
					return date;
			}
		}
	}
}
