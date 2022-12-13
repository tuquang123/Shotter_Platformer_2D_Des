using G2.Sdk.SecurityHelper;
using UnityEngine;

namespace G2.Sdk.PlayerPrefsHelper
{
    public class RectProfileData : BaseProfileDataType<Rect>
    {
        public Rect data { private set; get; }

        public RectProfileData(string tag, Rect defaultValue, DataEncryption dataEncryption, bool isAutoInit = true)
            : base(tag, defaultValue, dataEncryption, isAutoInit)
        {
        }

        //--------------------------------------------------------------------------------
        public static implicit operator Rect(RectProfileData rectProfileData)
        {
            return rectProfileData.data;
        }

        public override void Set(Rect value)
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
        protected override void Load(Rect defaultValue)
        {
            data = this.LoadFromPlayerPrefs(defaultValue);
        }

        protected override void Save(Rect value)
        {
            data = value;
            this.SaveToPlayerPrefs(value);
        }

        protected override void InitData(Rect defaultValue)
        {
            data = defaultValue;
            base.InitData(defaultValue);
        }

        //--------------------------------------------------------------------------------
        protected override Rect LoadFromPlayerPrefs(Rect defaultValue)
        {
            Rect result;
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
                float width = float.Parse(plainData.Substring(i + 1, j - i - 1));

                i = j;
                j = plainData.Length;
                float height = float.Parse(plainData.Substring(i + 1, j - i - 1));

                result = new Rect(x, y, width, height);
            }
            catch
            {
                return defaultValue;
            }

            return result;
        }

        protected override void SaveToPlayerPrefs(Rect value)
        {
            PlayerPrefs.SetString(encryptedTag, dataEncryption.Encrypt(value.x + "," + value.y + "," + value.width + "," + value.height));
        }
    }
}