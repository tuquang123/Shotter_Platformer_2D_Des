using UnityEngine;
using G2.Sdk.SecurityHelper;

public class ProfileManager
{
    private static UserProfile userProfile;

    public static UserProfile UserProfile { get { return userProfile; } }

    private static DataEncryption dataEncryption;

    public static void Init(string password = "nzt", string saltKey = "N7x9QZt2")
    {
        if (userProfile == null)
        {
            dataEncryption = new DataEncryption(password, saltKey);
        }

        Load();
    }

    public static void Load()
    {
        if (userProfile == null)
        {
            userProfile = new UserProfile(dataEncryption);
        }
    }

    public static void Load(RawUserProfile newData)
    {
        userProfile.ResetTo(newData);
    }

    public static void SaveAll()
    {
        PlayerPrefs.Save();
    }

    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }
}