﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komissarov.Nsudotnet.Perlin
{
	/// <summary>
	/// Encapsulates operations with grids, 
	/// each grid step is processed in parallel
	/// </summary>
	class AsyncNoiseGenerator : NoiseGenerator
	{
		private delegate void GridProcessor( int number );

		public AsyncNoiseGenerator( int gridCount )
			: base( gridCount )
		{

		}

		public override Bitmap GenerateImage( Size size, params float[] coefficients )
		{
			Bitmap map = new Bitmap( size.Width, size.Height );

			int n = 2;
			int m = 2;

			Grid[,] grids = new Grid[coefficients.Length, 3];
			float[][, ,] maps = new float[coefficients.Length][, ,];
			IAsyncResult[] results = new IAsyncResult[coefficients.Length];

			int pow = 1;

			//initializing grids and arrays for intermediate values
			for ( int i = 0; i < coefficients.Length; ++i )
			{
				for ( int j = 0; j < 3; ++j )
					grids[i, j] = new Grid( n * pow, m * pow )
					{
						Size = size
					};
				pow *= 2;

				maps[i] = new float[size.Width, size.Height, 3];
			}

			//calculates interpolated values for each grid step
			GridProcessor processor = number =>
			{
				for ( int j = 0; j < size.Height; ++j )
					for ( int i = 0; i < size.Width; ++i )
						for ( int k = 0; k < 3; ++k )
							maps[number][i, j, k] += coefficients[number] * grids[number, k].GetValue( i, j );
			};

			for ( int k = 0; k < coefficients.Length; ++k )
				results[k] = processor.BeginInvoke( k, null, null );

			foreach ( var result in results )
				processor.EndInvoke( result );

			//merging intermediate values
			for ( int j = 0; j < size.Height; ++j )
				for ( int i = 0; i < size.Width; ++i )
				{
					float[] colors = new float[3];
					for ( int k = 0; k < coefficients.Length; ++k )
						for ( int l = 0; l < 3; ++l )
							colors[l] += maps[k][i, j, l];

					for ( int l = 0; l < 3; ++l )
					{
						colors[l] = colors[l] <= 255 ? colors[l] : 255;
						colors[l] = colors[l] >= 0 ? colors[l] : 0;
					}

					Color color = Color.FromArgb( ( int )colors[0], ( int )colors[1], ( int )colors[2] );
					map.SetPixel( i, j, color );
				}

			return map;
		}
	}
}
