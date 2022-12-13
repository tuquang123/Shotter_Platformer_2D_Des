using UnityEngine;
using System;
using G2.Sdk.SecurityHelper;

namespace G2.Sdk.PlayerPrefsHelper
{
    public class DateTimeProfileData : BaseProfileDataType<DateTime>
    {
        public DateTime data { private set; get; }

        public DateTimeProfileData(string tag, DateTime defaultValue, DataEncryption dataEncryption, bool isAutoInit = true)
            : base(tag, defaultValue, dataEncryption, isAutoInit)
        {
        }

        //--------------------------------------------------------------------------------
        public static implicit operator DateTime(DateTimeProfileData dateTimeProfileData)
        {
            return dateTimeProfileData.data;
        }

        public override void Set(DateTime value)
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
        protected override void Load(DateTime defaultValue)
        {
            data = this.LoadFromPlayerPrefs(defaultValue);
        }

        protected override void Save(DateTime value)
        {
            data = value;
            this.SaveToPlayerPrefs(value);
        }

        protected override void InitData(DateTime defaultValue)
        {
            data = defaultValue;
            base.InitData(defaultValue);
        }

        //--------------------------------------------------------------------------------
        protected override DateTime LoadFromPlayerPrefs(DateTime defaultValue)
        {
            DateTime result;
            try
            {
                result = DateTime.Parse(dataEncryption.Decrypt(PlayerPrefs.GetString(encryptedTag)));
            }
            catch
            {
                return defaultValue;
            }

            return result;
        }

        protected override void SaveToPlayerPrefs(DateTime value)
        {
            PlayerPrefs.SetString(encryptedTag, dataEncryption.Encrypt(value.ToString()));
        }
    }
}