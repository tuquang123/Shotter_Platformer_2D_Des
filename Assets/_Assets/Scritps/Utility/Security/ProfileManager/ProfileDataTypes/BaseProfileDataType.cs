using G2.Sdk.SecurityHelper;
using UnityEngine;

namespace G2.Sdk.PlayerPrefsHelper
{
    public abstract class BaseProfileDataType<T>
    {
        protected string encryptedTag;
        protected DataEncryption dataEncryption;

        public BaseProfileDataType(string tag, T defaultValue, DataEncryption dataEncryption, bool isAutoInit)
        {
            this.dataEncryption = dataEncryption;
            encryptedTag = dataEncryption.Encrypt(tag);

            if (isAutoInit)
            {
                InitData(defaultValue);
            }
        }

        //--------------------------------------------------------------------------------
        protected bool IsHasValue()
        {
            return PlayerPrefs.HasKey(encryptedTag);
        }

        //--------------------------------------------------------------------------------
        public abstract void Set(T value);

        protected abstract void Load(T defaultValue);

        protected abstract void Save(T value);

        //--------------------------------------------------------------------------------
        protected virtual void InitData(T defaultValue)
        {
            if (IsHasValue() == false)
            {
                Save(defaultValue);
            }
            else
            {
                Load(defaultValue);
            }
        }

        //--------------------------------------------------------------------------------
        protected abstract T LoadFromPlayerPrefs(T defaultValue);

        protected abstract void SaveToPlayerPrefs(T value);
    }
}