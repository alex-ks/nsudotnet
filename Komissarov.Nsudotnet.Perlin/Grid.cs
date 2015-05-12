using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Komissarov.Nsudotnet.Perlin
{
	class Grid
	{
		private int _n, _m;
		private float[,] _grid, _factors, _buffer;
		private float[,][,] _coefficients;
		private int _lastN, _lastM;
		private Random _random;

		private Size _size;

		public Size Size
		{
			set
			{
				if ( value.Width < _n || value.Height < _m )
					throw new ArgumentException( );
				_size = value;
			}
			get
			{
				return _size;
			}
		}

		public Grid( int n, int m )
		{
			_n = n;
			_m = m;
			_grid = new float[n, m];
			_factors = new float[4, 4];
			_buffer = new float[4, 4];
			_lastN = _lastM = -1;
			_random = new Random( DateTime.Now.Millisecond );
			_coefficients = Helper.GetFactorsArray( );
			InitGrid( );
		}

		private void InitGrid( )
		{
			for ( int i = 0; i < _n; ++i )
				for ( int j = 0; j < _m; ++j )
					_grid[i, j] = _random.Next( 50, 255 );
		}

		//calculation of factors for bicubic interpolation
		private void CalcFactors( int n, int m )
		{
			for ( int i = -1; i < 3; ++i )
				for ( int j = -1; j < 3; ++j )
				{
					try
					{
						_buffer[i + 1, j + 1] = _grid[n + i, m + j];
					}
					catch ( IndexOutOfRangeException e )
					{
						_buffer[i + 1, j + 1] = _grid[n, m];
					}
				}

			for ( int i = 0; i < 4; ++i )
				for ( int j = 0; j < 4; ++j )
					_factors[i, j] = 0;

			for ( int i = 0; i < 4; ++i )
				for ( int j = 0; j < 4; ++j )
					for ( int k = 0; k < 4; ++k )
						for ( int l = 0; l < 4; ++l )
							_factors[i, j] += _coefficients[i, j][k, l] * _buffer[k, l];
		}

		//value calculates on demand, caching bicubic coefficients for points from one cell
		public float GetValue( int x, int y )
		{
			if ( x < 0 || x >= Size.Width )
				throw new ArgumentException( );
			if ( y < 0 || y >= Size.Height )
				throw new ArgumentException( );

			float fx = ( ( float )x ) / Size.Width * _n;
			float fy = ( ( float )y ) / Size.Height * _m;

			int n = ( int )fx,
				m = ( int )fy;

			float result = 0.0f;

			if ( n != _lastN || m != _lastM )
				CalcFactors( n, m );

			_lastN = n;
			_lastM = m;

			for ( int i = 0; i < 4; ++i )
				for ( int j = 0; j < 4; ++j )
					result += _factors[i, j] * ( float )Math.Pow( fx - n, i ) * ( float )Math.Pow( fy - m, j );

			//bilinear interpolation
			//var intermediate1 = ( 1 - fx + n ) * _buffer[1, 1] + ( fx - n ) * _buffer[1, 2];
			//var intermediate2 = ( 1 - fx + n ) * _buffer[2, 1] + ( fx - n ) * _buffer[2, 2];

			//result = ( ( 1 - fy + m ) * intermediate1 + ( fy - m ) * intermediate2 );

			result = result >= 0f ? result : 0f;
			result = result <= 255f ? result : 255f;
			return result;
		}
	}
}
