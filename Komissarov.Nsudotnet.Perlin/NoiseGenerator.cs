using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komissarov.Nsudotnet.Perlin
{
	/// <summary>
	/// Encapsulates operations with grids
	/// </summary>
	class NoiseGenerator
	{
		private float[] _coefficients;

		public NoiseGenerator( int gridCount )
		{
			_coefficients = new float[gridCount];

			Random random = new Random( DateTime.Now.Millisecond );

			float sum = 0f;

			//calculating random coefficients
			for ( int i = 0; i < gridCount; ++i )
			{
				_coefficients[i] = random.Next( );
				sum += _coefficients[i];
			}

			//normalizing
			for ( int i = 0; i < gridCount; ++i )
				_coefficients[i] /= sum;
		}

		public Bitmap GenerateImage( Size size )
		{
			return GenerateImage( size, _coefficients );
		}

		public virtual Bitmap GenerateImage( Size size, params float[] coefficients )
		{
			Bitmap map = new Bitmap( size.Width, size.Height );

			int n = 2;
			int m = 2;

			Grid[,] grids = new Grid[coefficients.Length, 3];

			int pow = 1;

			for ( int i = 0; i < coefficients.Length; ++i )
			{
				for ( int j = 0; j < 3; ++j )
					grids[i, j] = new Grid( n * pow, m * pow )
					{
						Size = size
					};
				pow *= 2;
			}

			//merging content of different grids
			for ( int j = 0; j < size.Height; ++j )
				for ( int i = 0; i < size.Width; ++i )
				{
					float[] colors = new float[3];
					for ( int k = 0; k < coefficients.Length; ++k )
						for ( int l = 0; l < 3; ++l )
							colors[l] += coefficients[k] * grids[k, l].GetValue( i, j );

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
