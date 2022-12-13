using G2.Sdk.SecurityHelper;

namespace G2.Sdk.PlayerPrefsHelper
{
    public class IntBitMaskProfileData : IntProfileData
    {
        public IntBitMaskProfileData(string tag, int defaultValue, DataEncryption dataEncryption)
            : base(tag, defaultValue, dataEncryption)
        {
        }

        //--------------------------------------------------------------------------------
        public void turnOn(int bit)
        {
            if (bit < 32)
            {
                this.Save(data | (1 << bit));
            }
        }

        public void turnOff(int bit)
        {
            if (bit < 32)
            {
                this.Save(data & ~(1 << bit));
            }
        }

        public bool isOn(int bit)
        {
            if (bit < 32)
            {
                if (((this.data >> bit) & 1) != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}