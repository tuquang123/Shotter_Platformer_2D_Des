using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;
using UnityEngine.UI;

public class DatabaseManager : MonoBehaviour
{
    public InputField inputPlayerName;

    void Start()
    {
        FbController.Instance.LoginWithReadPermission(success =>
        {
            if (success)
            {
                FireBaseDatabase.Instance.AuthenWithFacebook(AccessToken.CurrentAccessToken.UserId, AccessToken.CurrentAccessToken.TokenString, callback =>
                {
                    Debug.Log(callback.ToString());
                });
            }
        });
    }

    public void SearchPlayerByName()
    {
        if (!string.IsNullOrEmpty(inputPlayerName.text))
        {
            FireBaseDatabase.Instance.SearUserByName(inputPlayerName.text, user =>
            {
                Debug.Log(user.ToString());
            });
        }
    }
}
