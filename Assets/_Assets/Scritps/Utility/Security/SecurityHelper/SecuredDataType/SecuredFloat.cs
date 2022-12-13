using UnityEngine;

namespace G2.Sdk.SecurityHelper
{
    [System.Serializable]
    public class SecuredFloat
	{
		//Prevent memory-scanning hack
		private static int XOR_KEY = Random.Range (-1000000000, 1000000000);
		private int internalValue;
		
		public float Value {
			get {
				return TypeConverter.IntToFloat (internalValue ^ XOR_KEY);
			}
			set {
				internalValue = TypeConverter.FloatToInt (value) ^ XOR_KEY;
			}
		}
		
		public SecuredFloat (float value)
		{
			this.Value = value;
		}
		
		public SecuredFloat ()
		{
		}
		
		//--------------------------------------------------------------------------------
		public static implicit operator float (SecuredFloat c)
		{
			return c.Value;
		}
		
		public static implicit operator string (SecuredFloat c)
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