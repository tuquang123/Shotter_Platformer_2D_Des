using G2.Sdk.SecurityHelper;
using UnityEngine;

namespace G2.Sdk.PlayerPrefsHelper
{
    public class Vector4ProfileData : BaseProfileDataType<Vector4>
    {
        public Vector4 data { private set; get; }

        public Vector4ProfileData(string tag, Vector4 defaultValue, DataEncryption dataEncryption, bool isAutoInit = true)
            : base(tag, defaultValue, dataEncryption, isAutoInit)
        {
        }

        //--------------------------------------------------------------------------------
        public static implicit operator Vector4(Vector4ProfileData vector4ProfileData)
        {
            return vector4ProfileData.data;
        }

        public override void Set(Vector4 value)
        {
            if (data != value)
            {
                Save(value);
            }
        }

        public override string ToString()
        {
            return data.ToString();
        }

        //--------------------------------------------------------------------------------
        protected override void Load(Vector4 defaultValue)
        {
            data = this.LoadFromPlayerPrefs(defaultValue);
        }

        protected override void Save(Vector4 value)
        {
            data = value;
            this.SaveToPlayerPrefs(value);
        }

        protected override void InitData(Vector4 defaultValue)
        {
            data = defaultValue;
            base.InitData(defaultValue);
        }

        //--------------------------------------------------------------------------------
        protected override Vector4 LoadFromPlayerPrefs(Vector4 defaultValue)
        {
            Vector4 result;
            string plainData = dataEncryption.Decrypt(PlayerPrefs.GetString(encryptedTag));

            try
            {
                int i = 0, j = plainData.IndexOf(',');
                float x = float.Parse(plainData.Substring(i, j - i));

                i = j;
                j = plainData.IndexOf(',', i + 1);
                float y = float.Parse(plainData.Substring(i + 1, j - i - 1));

                i = j;
                j = plainData.IndexOf(',', i + 1);
                float z = float.Parse(plainData.Substring(i + 1, j - i - 1));

                i = j;
                j = plainData.Length;
                float w = float.Parse(plainData.Substring(i + 1, j - i - 1));

                result = new Vector4(x, y, z, w);
            }
            catch
            {
                return defaultValue;
            }

            return result;
        }

        protected override void SaveToPlayerPrefs(Vector4 value)
        {
            PlayerPrefs.SetString(encryptedTag, dataEncryption.Encrypt(value.x + "," + value.y + "," + value.z + "," + value.w));
        }
    }
}