using UnityEngine;
using System.Globalization;
using G2.Sdk.SecurityHelper;

namespace G2.Sdk.PlayerPrefsHelper
{
    public class FloatProfileData : BaseProfileDataType<float>
    {
        public SecuredFloat data { private set; get; }

        public FloatProfileData(string tag, float defaultValue, DataEncryption dataEncryption, bool isAutoInit = true)
            : base(tag, defaultValue, dataEncryption, isAutoInit)
        {
        }

        //--------------------------------------------------------------------------------
        public static implicit operator float(FloatProfileData floatProfileData)
        {
            return floatProfileData.data;
        }

        public override void Set(float value)
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
        protected override void Load(float defaultValue)
        {
            data.Value = this.LoadFromPlayerPrefs(defaultValue);
        }

        protected override void Save(float value)
        {
            data.Value = value;
            this.SaveToPlayerPrefs(value);
        }

        protected override void InitData(float defaultValue)
        {
            data = new SecuredFloat(defaultValue);
            base.InitData(defaultValue);
        }

        //--------------------------------------------------------------------------------
        protected override float LoadFromPlayerPrefs(float defaultValue)
        {
            float result;
            try
            {
                result = float.Parse(dataEncryption.Decrypt(PlayerPrefs.GetString(encryptedTag)), CultureInfo.InvariantCulture);
            }
            catch
            {
                return defaultValue;
            }

            return result;
        }

        protected override void SaveToPlayerPrefs(float value)
        {
            PlayerPrefs.SetString(encryptedTag, dataEncryption.Encrypt(value.ToString()));
        }
    }
}