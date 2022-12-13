#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("Kug31gjjikSo3pIiLs6HvKVnFMo+vbO8jD69tr4+vb28A8ZCZzAL9LvPHCepeaJUzktKBlOz5ypumNZQDYvTlColx8PNntYTsagnmWKqrAMJE966Ulys87ocpHnSa9FoTplnChs/nAwczwJ39U0xfcsikx8S2VuW2bahiwSCUBo+SjP/YdxEs+9sWo0H76pWSzBnnjlSJ0fuzqQ9WS1/jb/hOuqK0PoOFLpF/DWn9GZ2CHJUSJq7hV6wP4YNVj01bkk/GXPNVYicKRuEATRNLqGojEfEfNinWlfKd4w+vZ6Msbq1ljr0Okuxvb29uby/1HkTdvB3TRTgA0FHoejUnPCScPQpeSKDXwYrL3xGd/nKsjWrqtiCpZrZdC4WDzdOP76/vby9");
        private static int[] order = new int[] { 2,1,5,7,12,8,6,9,10,12,11,11,13,13,14 };
        private static int key = 188;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
