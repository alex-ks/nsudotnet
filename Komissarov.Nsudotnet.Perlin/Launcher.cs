using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komissarov.Nsudotnet.Perlin
{
	class Launcher
	{
		private const int GridCount = 4;

		public static void Main( string[] args )
		{
			if ( args.Length < 3 )
			{
				Console.WriteLine( "using: <width> <height> <output image name>.bmp opt::<mode>" );
				return;
			}

			Size size;

			try
			{
				size = new Size( int.Parse( args[0] ), int.Parse( args[1] ) );
			}
			catch ( FormatException e )
			{
				Console.WriteLine( "Bad argument!" );
				Console.WriteLine( "using: <width> <height> <output image name>.bmp opt::<mode>" );
				return;
			}

			NoiseGenerator generator;

			if ( args.Length == 4 && args[3].Equals( "async" ) )
				generator = new AsyncNoiseGenerator( GridCount );
			else
				generator = new NoiseGenerator( GridCount );

			var start = DateTime.Now;

			Bitmap map = generator.GenerateImage( size );

			var end = DateTime.Now;

			Console.Write( end - start );
			Console.WriteLine( " elapsed" );

			map.Save( args[2], System.Drawing.Imaging.ImageFormat.Bmp );
		}
	}
}
