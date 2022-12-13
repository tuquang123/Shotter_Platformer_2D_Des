using UnityEngine;

namespace G2.Sdk.SecurityHelper
{
    [System.Serializable]
    public class SecuredLong
	{
		//Prevent memory-scanning hack
		private static long XOR_KEY = Random.Range (-1000000000, 1000000000);
		private long internalValue;
	
		public long Value {
			get {
				return internalValue ^ XOR_KEY;
			}
			set {
				internalValue = value ^ XOR_KEY;
			}
		}
	
		public SecuredLong (long value)
		{
			this.Value = value;
		}

		public SecuredLong ()
		{
		}
	
		//--------------------------------------------------------------------------------	
		public static implicit operator long (SecuredLong c)
		{
			return c.Value;
		}

		public static implicit operator string (SecuredLong c)
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