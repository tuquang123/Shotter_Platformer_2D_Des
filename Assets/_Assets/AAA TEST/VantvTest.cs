using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;
using System;
using Random = UnityEngine.Random;
using UnityEngine.Networking;

public class VantvTest : MonoBehaviour
{
    float timer = 0f;

    void Start()
    {
        //GenerateFirebaseTestUsers();
        //GenerateFirebaseTestTournaments();

        string json = "{\"code\":2,\"data\":{\"dateTime\":\"2018-04-08T15:29:32Z\"}}";

        MasterInfoResponse res = JsonConvert.DeserializeObject<MasterInfoResponse>(json);
        Debug.Log(res.data.dateTime);
    }



    void GenerateFirebaseTestUsers()
    {
        // Generate firebase test users data
        Dictionary<string, object> testUsers = new Dictionary<string, object>();

        for (int i = 0; i < 50; i++)
        {
            Dictionary<string, object> profile = new Dictionary<string, object>();

            Dictionary<string, string> info = new Dictionary<string, string>();
            info.Add("name", "Name User " + i);
            info.Add("email", i + "@test.com");
            info.Add("authId", i.ToString());

            profile.Add("profile", info);
            testUsers.Add(i.ToString(), profile);
        }

        Debug.Log(JsonConvert.SerializeObject(testUsers));
    }

    void GenerateFirebaseTestTournaments()
    {
        // Generate firebase test tournaments data
        Dictionary<string, Dictionary<string, object>> tournament = new Dictionary<string, Dictionary<string, object>>();

        for (int i = 0; i < 50; i++)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("score", 49 - i);
            data.Add("primaryGunId", Random.Range(0, 100));
            data.Add("received", false);

            tournament.Add(i.ToString(), data);
        }

        Debug.Log(JsonConvert.SerializeObject(tournament));
    }

    //IEnumerator DemoTimeleft()
    //{
    //    TimeSpan t;
    //    double timeleft = MasterInfo.Instance.GetTournamentTimeleftInSecond();

    //    while (timeleft > 0)
    //    {
    //        yield return StaticValue.waitOneSec;
    //        timeleft--;

    //        t = TimeSpan.FromSeconds(timeleft);

    //        int days = 0;
    //        int hours = 0;
    //        int minutes = 0;
    //        int seconds = 0;

    //        MasterInfo.Instance.CountDownTimer(t, out days, out hours, out minutes, out seconds);

    //        Debug.Log(string.Format("{0}D {1}H:{2}M:{3}S", days, hours, minutes, seconds));
    //    }
    //}
}