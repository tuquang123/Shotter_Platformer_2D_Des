using G2.Sdk.SecurityHelper;
using UnityEngine;

namespace G2.Sdk.PlayerPrefsHelper
{
    public class BoolProfileData : BaseProfileDataType<bool>
    {
        public bool data { private set; get; }

        public BoolProfileData(string tag, bool defaultValue, DataEncryption dataEncryption, bool isAutoInit = true)
            : base(tag, defaultValue, dataEncryption, isAutoInit)
        {
        }

        //--------------------------------------------------------------------------------
        public static implicit operator bool(BoolProfileData boolProfileData)
        {
            return boolProfileData.data;
        }

        public override void Set(bool value)
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
        protected override void Load(bool defaultValue)
        {
            data = this.LoadFromPlayerPrefs(defaultValue);
        }

        protected override void Save(bool value)
        {
            data = value;
            this.SaveToPlayerPrefs(value);
        }

        //--------------------------------------------------------------------------------
        protected override bool LoadFromPlayerPrefs(bool value)
        {
            bool result;
            try
            {
                result = bool.Parse(dataEncryption.Decrypt(PlayerPrefs.GetString(encryptedTag)));
            }
            catch
            {
                return value;
            }

            return result;
        }

        protected override void SaveToPlayerPrefs(bool value)
        {
            PlayerPrefs.SetString(encryptedTag, dataEncryption.Encrypt(value.ToString()));
        }
    }
}