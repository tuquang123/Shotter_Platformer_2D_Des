using UnityEngine;
using G2.Sdk.SecurityHelper;

namespace G2.Sdk.PlayerPrefsHelper
{
    public class IntProfileData : BaseProfileDataType<int>
    {
        public SecuredInt data { private set; get; }

        public IntProfileData(string tag, int defaultValue, DataEncryption dataEncryption, bool isAutoInit = true)
            : base(tag, defaultValue, dataEncryption, isAutoInit)
        {
        }

        //--------------------------------------------------------------------------------
        public static implicit operator int(IntProfileData intProfileData)
        {
            return intProfileData.data.Value;
        }

        public override void Set(int value)
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
        protected override void Load(int defaultValue)
        {
            data.Value = this.LoadFromPlayerPrefs(defaultValue);
        }

        protected override void Save(int value)
        {
            data.Value = value;
            this.SaveToPlayerPrefs(value);
        }

        protected override void InitData(int defaultValue)
        {
            data = new SecuredInt(defaultValue);
            base.InitData(defaultValue);
        }

        //--------------------------------------------------------------------------------
        protected override int LoadFromPlayerPrefs(int defaultValue)
        {
            int result;
            try
            {
                result = int.Parse(dataEncryption.Decrypt(PlayerPrefs.GetString(encryptedTag)));
            }
            catch
            {
                return defaultValue;
            }

            return result;
        }

        protected override void SaveToPlayerPrefs(int value)
        {
            PlayerPrefs.SetString(encryptedTag, dataEncryption.Encrypt(value.ToString()));
        }
    }
}