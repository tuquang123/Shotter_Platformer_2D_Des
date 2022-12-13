using G2.Sdk.SecurityHelper;
using UnityEngine;

namespace G2.Sdk.PlayerPrefsHelper
{
    public class Vector3ProfileData : BaseProfileDataType<Vector3>
    {
        public Vector3 data { private set; get; }

        public Vector3ProfileData(string tag, Vector3 defaultValue, DataEncryption dataEncryption, bool isAutoInit = true)
            : base(tag, defaultValue, dataEncryption, isAutoInit)
        {
        }

        //--------------------------------------------------------------------------------
        public static implicit operator Vector3(Vector3ProfileData vector3ProfileData)
        {
            return vector3ProfileData.data;
        }

        public override void Set(Vector3 value)
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
        protected override void Load(Vector3 defaultValue)
        {
            data = this.LoadFromPlayerPrefs(defaultValue);
        }

        protected override void Save(Vector3 value)
        {
            data = value;
            this.SaveToPlayerPrefs(value);
        }

        protected override void InitData(Vector3 defaultValue)
        {
            data = defaultValue;
            base.InitData(defaultValue);
        }

        //--------------------------------------------------------------------------------
        protected override Vector3 LoadFromPlayerPrefs(Vector3 defaultValue)
        {
            Vector3 result;
            string plainData = dataEncryption.Decrypt(PlayerPrefs.GetString(encryptedTag));

            try
            {
                int i = 0, j = plainData.IndexOf(',');
                float x = float.Parse(plainData.Substring(i, j - i));

                i = j;
                j = plainData.IndexOf(',', i + 1);
                float y = float.Parse(plainData.Substring(i + 1, j - i - 1));

                i = j;
                j = plainData.Length;
                float z = float.Parse(plainData.Substring(i + 1, j - i - 1));

                result = new Vector3(x, y, z);
            }
            catch
            {
                return defaultValue;
            }

            return result;
        }

        protected override void SaveToPlayerPrefs(Vector3 value)
        {
            PlayerPrefs.SetString(encryptedTag, dataEncryption.Encrypt(value.x + "," + value.y + "," + value.z));
        }
    }
}