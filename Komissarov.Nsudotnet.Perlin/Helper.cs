using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komissarov.Nsudotnet.Perlin
{
	/// <summary>
	/// Contains hardcoded array of floats, required for bicubic interpolation coefficients calculation
	/// </summary>
	static class Helper
	{
		private static float[,] _factorsMatrix;

		static Helper( )
		{
			_factorsMatrix = new[,]

			 {	{ 0f, 0f, 0f, 0f, 0f, 36f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f },
				{ 0f, -12f, 0f, 0f, 0f, -18f, 0f, 0f, 0f, 36f, 0f, 0f, 0f, -6f, 0f, 0f },
				{ 0f, 18f, 0f, 0f, 0f, -36f, 0f, 0f, 0f, 18f, 0f, 0f, 0f, 0f, 0f, 0f },
				{ 0f, -6f, 0f, 0f, 0f, 18f, 0f, 0f, 0f, -18f, 0f, 0f, 0f, 6f, 0f, 0f },

				{ 0f, 0f, 0f, 0f, -12f, -18f, 36f, -6f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f },
				{ 4f, 6f, -12f, 2f, 6f, 9f, -18f, 3f, -12f, -18f, 36f, -6f, 2f, 3f, -6f, 1f },
				{ -6f, -9f, 18f, -3f, 12f, 18f, -36f, 6f, -6f, -9f, 18f, -3f, 0f, 0f, 0f, 0f },
				{ 2f, 3f, -6f, 1f, -6f, -9f, 18f, -3f, 6f, 9f, -18f, 3f, -2f, -3f, 6f, -1f },

				{ 0f, 0f, 0f, 0f, 18f, -36f, 18f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f },
				{ -6f, 12f, -6f, 0f, -9f, 18f, -9f, 0f, 18f, -36f, 18f, 0f, -3f, 6f, -3f, 0f },
				{ 9f, -18f, 9f, 0f, -18f, 36f, -18f, 0f, 9f, -18f, 9f, 0f, 0f, 0f, 0f, 0f },
				{ -3f, 6f, -3f, 0f, 9f, -18f, 9f, 0f, -9f, 18f, -9f, 0f, 3f, -6f, 3f, 0f },

				{ 0f, 0f, 0f, 0f, -6f, 18f, -18f, 6f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f },
				{ 2f, -6f, 6f, -2f , 3f, -9f, 9f, -3f, -6f, 18f, -18f, 6f, 1f, -3f, 3f, -1f },
				{ -3f, 9f, -9f, 3f , 6f, -18f, 18f, -6f, -3f, 9f, -9f, 3f, 0f, 0f, 0f, 0f },
				{ 1f, -3f, 3f, -1f , -3f, 9f, -9f, 3, 3f, -9f, 9f, -3f, -1f, 3f, -3f, 1f } };
		}

		public static float[,] FactorsMatrix
		{
			get { return _factorsMatrix; }
		}
	}
}
