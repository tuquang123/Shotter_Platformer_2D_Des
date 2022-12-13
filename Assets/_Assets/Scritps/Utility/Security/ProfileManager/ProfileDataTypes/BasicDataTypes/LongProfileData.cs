using UnityEngine;
using G2.Sdk.SecurityHelper;

namespace G2.Sdk.PlayerPrefsHelper
{
    public class LongProfileData : BaseProfileDataType<long>
    {
        public SecuredLong data { private set; get; }

        public LongProfileData(string tag, long defaultValue, DataEncryption dataEncryption, bool isAutoInit = true)
            : base(tag, defaultValue, dataEncryption, isAutoInit)
        {
        }

        //--------------------------------------------------------------------------------
        public static implicit operator long(LongProfileData longProfileData)
        {
            return longProfileData.data.Value;
        }

        public override void Set(long value)
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
        protected override void Load(long defaultValue)
        {
            data.Value = this.LoadFromPlayerPrefs(defaultValue);
        }

        protected override void Save(long value)
        {
            data.Value = value;
            this.SaveToPlayerPrefs(value);
        }

        protected override void InitData(long defaultValue)
        {
            data = new SecuredLong(defaultValue);
            base.InitData(defaultValue);
        }

        //--------------------------------------------------------------------------------
        protected override long LoadFromPlayerPrefs(long defaultValue)
        {
            long result;
            try
            {
                result = long.Parse(dataEncryption.Decrypt(PlayerPrefs.GetString(encryptedTag)));
            }
            catch
            {
                return defaultValue;
            }

            return result;
        }

        protected override void SaveToPlayerPrefs(long value)
        {
            PlayerPrefs.SetString(encryptedTag, dataEncryption.Encrypt(value.ToString()));
        }
    }
}