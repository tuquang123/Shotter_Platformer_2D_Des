using UnityEngine;

namespace G2.Sdk.SecurityHelper
{
    [System.Serializable]
    public class SecuredInt
	{
		//Prevent memory-scanning hack
		private static int XOR_KEY = Random.Range (-1000000000, 1000000000);
		private int internalValue;
	
		public int Value {
			get {
				return internalValue ^ XOR_KEY;
			}
			set {
				internalValue = value ^ XOR_KEY;
			}
		}

		public SecuredInt (int value)
		{
			this.Value = value;
		}

		public SecuredInt ()
		{
		}

		//--------------------------------------------------------------------------------
		public static implicit operator int (SecuredInt c)
		{
			return c.Value;
		}

		public static implicit operator string (SecuredInt c)
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