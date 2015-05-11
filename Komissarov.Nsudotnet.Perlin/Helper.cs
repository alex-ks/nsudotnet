using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komissarov.Nsudotnet.Perlin
{
	static class Helper
	{
		public static float[,][,] GetFactorsArray( )
		{
			float[,][,] result = new float[4, 4][,];

			result[0, 0] = new[,] { { 0f, 0f, 0f, 0f }, { 0f, 1f, 0f, 0f }, { 0f, 0f, 0f, 0f }, { 0f, 0f, 0f, 0f } };
			result[0, 1] = new[,] { { 0f, 0f, 0f, 0f }, { -0.5f, 0f, 0.5f, 0f }, { 0f, 0f, 0f, 0f }, { 0f, 0f, 0f, 0f } };
			result[0, 2] = new[,] { { 0f, 0f, 0f, 0f }, { 1f, -2.5f, 2f, -0.5f }, { 0f, 0f, 0f, 0f }, { 0f, 0f, 0f, 0f } };
			result[0, 3] = new[,] { { 0f, -0.5f, 0f, 0f }, { -0.5f, 1.5f, -1.5f, 0.5f }, { 0f, 0f, 0f, 0f }, { 0f, 0f, 0f, 0f } };

			result[1, 0] = new[,] { { 0f, -0.5f, 0f, 0f }, { 0f, 0f, 0f, 0f }, { 0f, 0.5f, 0f, 0f }, { 0f, 0f, 0f, 0f } };
			result[1, 1] = new[,] { { 0.25f, 0f, -0.25f, 0f }, { 0f, 0f, 0f, 0f }, { -0.25f, 0f, 0.25f, 0f }, { 0f, 0f, 0f, 0f } };
			result[1, 2] = new[,] { { -0.5f, 1.25f, -1f, 0.25f }, { 0f, 0f, 0f, 0f }, { 0.5f, -1.25f, 1f, -0.25f }, { 0f, 0f, 0f, 0f } };
			result[1, 3] = new[,] { { 0.25f, -0.75f, 0.75f, -0.25f }, { 0f, 0f, 0f, 0f }, { -0.25f, 0.75f, -0.75f, 0.25f }, { 0f, 0f, 0f, 0f } };

			result[2, 0] = new[,] { { 0f, 1f, 0f, 0f }, { 0f, -2.5f, 0f, 0f }, { 0f, 2f, 0f, 0f }, { 0f, -0.5f, 0f, 0f } };
			result[2, 1] = new[,] { { -0.5f, 0f, 0.5f, 0f }, { 1.25f, 0f, -1.25f, 0f }, { -1f, 0f, 1f, 0f }, { 0.25f, 0f, -0.25f, 0f } };
			result[2, 2] = new[,] { { 1f, -2.5f, 2f, -0.5f }, { -2.5f, 6.25f, -5f, 1.25f }, { 2f, -5f, 4f, -1f }, { -0.5f, 1.25f, -1f, 0.25f } };
			result[2, 3] = new[,] { { -0.5f, 1.5f, -1.5f, 0.5f }, { 1.25f, -3.75f, 3.75f, -1.25f }, { -1f, 3f, -3f, 1f }, { 0.25f, -0.75f, 0.75f, -0.25f } };

			result[3, 0] = new[,] { { 0f, -0.5f, 0f, 0f }, { 0f, 1.5f, 0f, 0f }, { 0f, -1.5f, 0f, 0f }, { 0f, 0.5f, 0f, 0f } };
			result[3, 1] = new[,] { { 0.25f, 0f, -0.25f, 0f }, { -0.75f, 0f, 0.75f, 0f }, { 0.75f, 0f, -0.75f, 0f }, { -0.25f, 0f, 0.25f, 0f } };
			result[3, 2] = new[,] { { -0.5f, 1.25f, -1f, 0.25f }, { 1.5f, -3.75f, 3f, -0.75f }, { -1.5f, 3.75f, -3f, 0.75f }, { 0.5f, -1.25f, 1f, -0.25f } };
			result[3, 3] = new[,] { { 0.25f, -0.75f, 0.75f, -0.25f }, { -0.75f, 2.25f, -2.25f, 0.75f }, { 0.75f, -2.25f, 2.25f, -0.75f }, { -0.25f, 0.75f, -0.75f, 0.25f } };

			return result;
		}
	}
}



//_factors[0, 0] = _swap[1, 1];
//_factors[0, 1] = 0.5f * ( -_swap[1, 0] + _swap[1, 2] );
//_factors[0, 2] = _swap[1, 0] - 2.5f * _swap[1, 1] + 2.0f * _swap[1, 2] - 0.5f * _swap[1, 3];
//_factors[0, 3] = -0.5f * _swap[1, 0] + 1.5f * _swap[1, 1] - 1.5f * _swap[1, 2] + 0.5f * _swap[1, 3];

//_factors[1, 0] = 0.5f * ( -_swap[0, 1] + _swap[2, 1] );
//_factors[1, 1] = 0.25f * ( _swap[0, 0] - _swap[0, 2] - _swap[2, 0] + _swap[2, 2] );
//_factors[1, 2] = -0.5f * _swap[0, 0] + 1.25f * _swap[0, 1] - _swap[0, 2] + 0.25f * _swap[0, 3]
//	+ 0.5f * _swap[2, 0] - 1.25f * _swap[2, 1] + _swap[2, 2] - 0.25f * _swap[2, 3];
//_factors[1, 3] = 0.25f * _swap[0, 0] - 0.75f * _swap[0, 1] + 0.75f * _swap[0, 2] - 0.25f * _swap[0, 3]
//	- 0.25f * _swap[2, 0] + 0.75f * _swap[2, 1] - 0.75f * _swap[2, 2] + 0.25f * _swap[2, 3];

//_factors[2, 0] = _swap[0, 1] - 2.5f * _swap[1, 1] + 2.0f * _swap[2, 1] - 0.5f * _swap[3, 1];
//_factors[2, 1] = -0.5f * _swap[0, 0] + 0.5f * _swap[0, 2] + 1.25f * _swap[1, 0] - 1.25f * _swap[1, 2]
//	- _swap[2, 0] + _swap[2, 2] + 0.25f * _swap[3, 0] - 0.25f * _swap[3, 2];
//_factors[2, 2] = _swap[0, 0] - 2.5f * _swap[0, 1] + 2.0f * _swap[0, 2] - 0.5f * _swap[0, 3]
//	- 2.5f * _swap[1, 0] + 6.25f * _swap[1, 1] - 5.0f * _swap[1, 2] + 1.25f * _swap[1, 3]
//	+ 2.0f * _swap[2, 0] - 5.0f * _swap[2, 1] + 4.0f * _swap[2, 2] - _swap[2, 3]
//	- 0.5f * _swap[3, 0] + 1.25f * _swap[3, 1] - _swap[3, 2] + 0.25f * _swap[3, 3];
//_factors[2, 3] = _swap[1, 1];

//_factors[3, 0] = _swap[1, 1];
//_factors[3, 1] = _swap[1, 1];
//_factors[3, 2] = _swap[1, 1];
//_factors[3, 3] = _swap[1, 1];