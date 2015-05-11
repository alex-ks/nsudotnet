﻿using System;
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
			if ( args.Length != 3 )
			{
				Console.WriteLine( "using: <width> <height> <output image name>.bmp" );
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
				Console.WriteLine( "using: <width> <height> <output image name>.bmp" );
				return;
			}

			NoiseGenerator generator = new NoiseGenerator( GridCount );

			Bitmap map = generator.GenerateImage( size );

			map.Save( args[2], System.Drawing.Imaging.ImageFormat.Bmp );
		}
	}
}