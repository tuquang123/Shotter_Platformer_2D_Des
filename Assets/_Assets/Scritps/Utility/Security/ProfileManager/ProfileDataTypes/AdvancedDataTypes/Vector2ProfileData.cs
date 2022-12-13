using G2.Sdk.SecurityHelper;
using UnityEngine;

namespace G2.Sdk.PlayerPrefsHelper
{
    public class Vector2ProfileData : BaseProfileDataType<Vector2>
    {
        public Vector2 data { private set; get; }

        public Vector2ProfileData(string tag, Vector2 defaultValue, DataEncryption dataEncryption, bool isAutoInit = true)
            : base(tag, defaultValue, dataEncryption, isAutoInit)
        {
        }

        //--------------------------------------------------------------------------------
        public static implicit operator Vector2(Vector2ProfileData vector2ProfileData)
        {
            return vector2ProfileData.data;
        }

        public override void Set(Vector2 value)
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
        protected override void Load(Vector2 defaultValue)
        {
            data = this.LoadFromPlayerPrefs(defaultValue);
        }

        protected override void Save(Vector2 value)
        {
            data = value;
            this.SaveToPlayerPrefs(value);
        }

        protected override void InitData(Vector2 defaultValue)
        {
            data = defaultValue;
            base.InitData(defaultValue);
        }

        //--------------------------------------------------------------------------------
        protected override Vector2 LoadFromPlayerPrefs(Vector2 defaultValue)
        {
            Vector2 result;
            string plainData = dataEncryption.Decrypt(PlayerPrefs.GetString(encryptedTag));

            try
            {
                int i = 0, j = plainData.IndexOf(',');
                float x = float.Parse(plainData.Substring(i, j - i));

                i = j;
                j = plainData.Length;
                float y = float.Parse(plainData.Substring(i + 1, j - i - 1));

                result = new Vector2(x, y);
            }
            catch
            {
                return defaultValue;
            }

            return result;
        }

        protected override void SaveToPlayerPrefs(Vector2 value)
        {
            PlayerPrefs.SetString(encryptedTag, dataEncryption.Encrypt(value.x + "," + value.y));
        }
    }
}