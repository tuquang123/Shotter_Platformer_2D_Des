using UnityEngine;

namespace G2.Sdk.SecurityHelper
{
    [System.Serializable]
    public class SecuredDouble
	{
		//Prevent memory-scanning hack
		private static long XOR_KEY = Random.Range (-1000000000, 1000000000);
		private long internalValue;
		
		public double Value {
			get {
				return TypeConverter.LongToDouble (internalValue ^ XOR_KEY);
			}
			set {
				internalValue = TypeConverter.DoubleToLong (value) ^ XOR_KEY;
			}
		}
		
		public SecuredDouble (double value)
		{
			this.Value = value;
		}
		
		public SecuredDouble ()
		{
		}
		
		//--------------------------------------------------------------------------------	
		public static implicit operator double (SecuredDouble c)
		{
			return c.Value;
		}
		
		public static implicit operator string (SecuredDouble c)
		{
			return c.Value.ToString ();
		}
		
		//--------------------------------------------------------------------------------
		public override string ToString ()
		{
			return Value.ToString ();
		}
	}
}
