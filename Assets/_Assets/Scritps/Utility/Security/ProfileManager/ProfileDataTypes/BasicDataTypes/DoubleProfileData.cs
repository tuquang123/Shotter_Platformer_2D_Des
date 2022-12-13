using UnityEngine;
using System.Globalization;
using G2.Sdk.SecurityHelper;

namespace G2.Sdk.PlayerPrefsHelper
{
    public class DoubleProfileData : BaseProfileDataType<double>
    {
        public SecuredDouble data { private set; get; }

        public DoubleProfileData(string tag, double defaultValue, DataEncryption dataEncryption, bool isAutoInit = true)
            : base(tag, defaultValue, dataEncryption, isAutoInit)
        {
        }

        //--------------------------------------------------------------------------------
        public static implicit operator double(DoubleProfileData doubleProfileData)
        {
            return doubleProfileData.data;
        }

        public override void Set(double value)
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
        protected override void Load(double defaultValue)
        {
            data.Value = this.LoadFromPlayerPrefs(defaultValue);
        }

        protected override void Save(double value)
        {
            data.Value = value;
            this.SaveToPlayerPrefs(value);
        }

        protected override void InitData(double defaultValue)
        {
            data = new SecuredDouble(defaultValue);
            base.InitData(defaultValue);
        }

        //--------------------------------------------------------------------------------
        protected override double LoadFromPlayerPrefs(double defaultValue)
        {
            double result;
            try
            {
                result = double.Parse(dataEncryption.Decrypt(PlayerPrefs.GetString(encryptedTag)), CultureInfo.InvariantCulture);
            }
            catch
            {
                return defaultValue;
            }

            return result;
        }

        protected override void SaveToPlayerPrefs(double value)
        {
            PlayerPrefs.SetString(encryptedTag, dataEncryption.Encrypt(value.ToString()));
        }
    }
}