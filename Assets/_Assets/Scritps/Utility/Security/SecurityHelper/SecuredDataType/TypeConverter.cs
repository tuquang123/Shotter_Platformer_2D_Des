using System;

namespace G2.Sdk.SecurityHelper
{
    public class TypeConverter
	{
		//Convenient class to transform floating-point bit representation to integer bit representation

		public static int FloatToInt (float value)
		{
			return BitConverter.ToInt32 (BitConverter.GetBytes (value), 0);
		}

		public static float IntToFloat (int value)
		{
			return BitConverter.ToSingle (BitConverter.GetBytes (value), 0);
		}

		public static long DoubleToLong (double value)
		{
			return BitConverter.ToInt64 (BitConverter.GetBytes (value), 0);
		}

		public static double LongToDouble (long value)
		{
			return BitConverter.ToDouble (BitConverter.GetBytes (value), 0);
		}
	}
}