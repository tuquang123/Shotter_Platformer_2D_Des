using G2.Sdk.SecurityHelper;
using UnityEngine;

namespace G2.Sdk.PlayerPrefsHelper
{
    public class QuaternionProfileData : BaseProfileDataType<Quaternion>
    {
        public Quaternion data { private set; get; }

        public QuaternionProfileData(string tag, Quaternion defaultValue, DataEncryption dataEncryption, bool isAutoInit = true)
            : base(tag, defaultValue, dataEncryption, isAutoInit)
        {
        }

        //--------------------------------------------------------------------------------
        public static implicit operator Quaternion(QuaternionProfileData quaternionProfileData)
        {
            return quaternionProfileData.data;
        }

        public override void Set(Quaternion value)
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
        protected override void Load(Quaternion defaultValue)
        {
            data = this.LoadFromPlayerPrefs(defaultValue);
        }

        protected override void Save(Quaternion value)
        {
            data = value;
            this.SaveToPlayerPrefs(value);
        }

        protected override void InitData(Quaternion defaultValue)
        {
            data = defaultValue;
            base.InitData(defaultValue);
        }

        //--------------------------------------------------------------------------------
        protected override Quaternion LoadFromPlayerPrefs(Quaternion defaultValue)
        {
            Quaternion result;
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
                float z = float.Parse(plainData.Substring(i + 1, j - i - 1));

                i = j;
                j = plainData.Length;
                float w = float.Parse(plainData.Substring(i + 1, j - i - 1));

                result = new Quaternion(x, y, z, w);
            }
            catch
            {
                return defaultValue;
            }

            return result;
        }

        protected override void SaveToPlayerPrefs(Quaternion value)
        {
            PlayerPrefs.SetString(encryptedTag, dataEncryption.Encrypt(value.x + "," + value.y + "," + value.z + "," + value.w));
        }
    }
}