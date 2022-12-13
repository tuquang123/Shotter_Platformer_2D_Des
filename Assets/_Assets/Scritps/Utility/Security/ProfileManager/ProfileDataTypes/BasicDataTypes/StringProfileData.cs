using G2.Sdk.SecurityHelper;
using UnityEngine;

namespace G2.Sdk.PlayerPrefsHelper
{
    public class StringProfileData : BaseProfileDataType<string>
    {
        public string data { private set; get; }

        public StringProfileData(string tag, string defaultValue, DataEncryption dataEncryption, bool isAutoInit = true)
            : base(tag, defaultValue, dataEncryption, isAutoInit)
        {
        }

        //--------------------------------------------------------------------------------
        public static implicit operator string(StringProfileData stringProfileData)
        {
            return stringProfileData.data;
        }

        public override void Set(string value)
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
        protected override void Load(string defaultValue)
        {
            data = this.LoadFromPlayerPrefs(defaultValue);
        }

        protected override void Save(string value)
        {
            data = value;
            this.SaveToPlayerPrefs(value);
        }

        //--------------------------------------------------------------------------------
        protected override string LoadFromPlayerPrefs(string defaultValue)
        {
            string result;
            try
            {
                result = dataEncryption.Decrypt(PlayerPrefs.GetString(encryptedTag));
            }
            catch
            {
                return defaultValue;
            }

            return result;
        }

        protected override void SaveToPlayerPrefs(string value)
        {
            if (value != null)
            {
                PlayerPrefs.SetString(encryptedTag, dataEncryption.Encrypt(value));
            }
        }
    }
}