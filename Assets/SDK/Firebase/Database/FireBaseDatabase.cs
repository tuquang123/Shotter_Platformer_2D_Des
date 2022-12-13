using Firebase;
using Firebase.Database;
#if UNITY_EDITOR
using Firebase.Unity.Editor;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Firebase.Auth;
using System;
using Newtonsoft.Json;
using Facebook.Unity;

public class FireBaseDatabase : Singleton<FireBaseDatabase>
{
    public DateTime timeStartChallenge;
    [SerializeField]
    private bool isTest;
    [SerializeField]
    private string dbUrl;
    [SerializeField]
    private string dbSandboxUrl;

    [Header("Database Node")]
    [SerializeField]
    private string tournamentNode;
    [SerializeField]
    private string userNode;
    [SerializeField]
    private string userDataNode;
    [SerializeField]
    private string nameNode;
    [SerializeField]
    private string scoreNode;
    [SerializeField]
    private string receivedNode;

    [Header("Config")]
    [SerializeField]
    private float timeoutOffline = 180; // 3 min
    [SerializeField]
    private float cacheTimeout = 300; // 5 min
    [SerializeField]
    private LogLevel logLevel;

    private DatabaseReference reference;

    private bool isAuthenticating = false;
    private bool isDatabaseOnline;

    private int trySaveInfoCount;
    private int trySaveScoreCount;
    private int trySaveReceivedRewardCount;
    private int trySaveUserInventory;

    private float timeGetTournamentData = -99999f;

    private List<TournamentData> tournamentData = new List<TournamentData>(50);
    private List<TournamentData> emptyTournamentData = new List<TournamentData>();
    private List<TournamentData> tournamentDataRewarded = new List<TournamentData>();
    private IEnumerator timeoutOfflineCoroutine;
    private string cachedWeekRange;

    public bool IsTest { get { return isTest; } }
    public bool IsOnline { get { return isDatabaseOnline; } }
    public bool IsAuthenticated { get { return AuthUserInfo != null; } }
    public UserInfo AuthUserInfo { get; private set; }


    #region UNITY METHODS

    void Awake()
    {
        FirebaseApp.LogLevel = logLevel;

#if UNITY_EDITOR
        if (isTest)
        {
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://rambo-2017-test.firebaseio.com/");
        }
        else
        {
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://rambo-2017-36964068.firebaseio.com/");
        }

        FirebaseApp.DefaultInstance.SetEditorP12FileName("Metal Commando-042499ab5f76.p12");
        FirebaseApp.DefaultInstance.SetEditorServiceAccountEmail("realtimedatabase-unity-editor@rambo-2017-36964068.iam.gserviceaccount.com");
        FirebaseApp.DefaultInstance.SetEditorP12Password("notasecret");
#endif

        if (isTest)
        {
            reference = FirebaseDatabase.GetInstance(dbSandboxUrl).RootReference;
        }
        else
        {
            reference = FirebaseDatabase.GetInstance(dbUrl).RootReference;
        }

        reference.Database.GoOffline();
        timeoutOfflineCoroutine = CheckAFK();

        DontDestroyOnLoad(this);
    }

    void OnDisable()
    {
        StopAllCoroutines();
        GoOffline();
    }

    #endregion


    #region PUBLIC METHODS

    public void GetTopTournament(int top, string weekRange, UnityAction<List<TournamentData>> callback)
    {
        if (!IsAuthenticated)
        {
            DebugCustom.Log("Firebase is not authenticate");
            return;
        }

        if (Time.realtimeSinceStartup - timeGetTournamentData < cacheTimeout && cachedWeekRange.Equals(weekRange))
        {
            DebugCustom.Log("Firebase get tournament data from CACHE");

            if (callback != null)
            {
                callback(tournamentData);
            }

            return;
        }

        GoOnline();

        reference.Child(tournamentNode).Child(weekRange).OrderByChild(scoreNode).LimitToLast(top).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                if (task.Result.Exists)
                {
                    cachedWeekRange = weekRange;

                    DataSnapshot snapshot = task.Result;
                    tournamentData.Clear();
                    timeGetTournamentData = Time.realtimeSinceStartup;

                    foreach (var item in snapshot.Children)
                    {
                        string uId = item.Key;
                        int score = 0;
                        int primaryGunId = 0;
                        bool received = false;

                        try
                        {
                            Dictionary<string, object> dat = item.Value as Dictionary<string, object>;

                            if (dat.ContainsKey("score"))
                                score = Convert.ToInt32(dat["score"]);
                            if (dat.ContainsKey("primaryGunId"))
                                primaryGunId = Convert.ToInt32(dat["primaryGunId"]);
                            if (dat.ContainsKey("received"))
                                received = Convert.ToBoolean(dat["received"]);

                            TournamentData data = new TournamentData(item.Key, score, primaryGunId, received);
                            tournamentData.Add(data);
                        }
                        catch (Exception e)
                        {
                            DebugCustom.LogError("Firebase parse tournament data exception: " + e.Message);
                        }
                    }
                }
                else
                {
                    DebugCustom.Log(string.Format("Firebase week range {0} not exists", weekRange));
                }
            }
            else
            {
                DebugCustom.Log("Firebase get tournament data FAILED");
            }

            if (callback != null)
            {
                if (task.Result.Exists)
                {
                    callback(tournamentData);
                }
                else
                {
                    callback(emptyTournamentData);
                }
            }
        });
    }

    public void GetTopTournamentForRewarded(UnityAction<List<TournamentData>> callback)
    {
        if (!IsAuthenticated)
        {
            DebugCustom.Log("Firebase is not authenticate");
            return;
        }

        GoOnline();

        tournamentDataRewarded.Clear();
        string weekRange = MasterInfo.Instance.GetPreviousWeekRangeString();

        reference.Child(tournamentNode).Child(weekRange).OrderByChild(scoreNode).LimitToLast(3).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                if (task.Result.Exists)
                {
                    DataSnapshot snapshot = task.Result;

                    foreach (var item in snapshot.Children)
                    {
                        string uId = item.Key;
                        int score = 0;
                        int primaryGunId = 0;
                        bool received = false;

                        try
                        {
                            Dictionary<string, object> dat = item.Value as Dictionary<string, object>;

                            if (dat.ContainsKey("score"))
                                score = Convert.ToInt32(dat["score"]);
                            if (dat.ContainsKey("primaryGunId"))
                                primaryGunId = Convert.ToInt32(dat["primaryGunId"]);
                            if (dat.ContainsKey("received"))
                                received = Convert.ToBoolean(dat["received"]);

                            TournamentData data = new TournamentData(item.Key, score, primaryGunId, received);
                            tournamentDataRewarded.Add(data);
                        }
                        catch (Exception e)
                        {
                            DebugCustom.LogError("Firebase parse tournament data exception: " + e.Message);
                        }
                    }
                }
                else
                {
                    DebugCustom.Log(string.Format("Firebase week range {0} not exists", weekRange));
                }
            }
            else
            {
                DebugCustom.Log("Firebase get tournament data FAILED");
            }

            if (callback != null)
            {
                callback(tournamentDataRewarded);
            }
        });
    }

    public void GetTournamentScore(string userId, string weekRange, UnityAction<int> callback)
    {
        if (!IsAuthenticated)
        {
            DebugCustom.Log("Firebase is not authenticate");
            return;
        }

        GoOnline();

        reference.Child(tournamentNode).Child(weekRange).Child(userId).Child(scoreNode).GetValueAsync().ContinueWith(task =>
        {
            int score = 0;

            if (task.IsCompleted)
            {
                if (task.Result.Exists)
                {
                    int.TryParse(task.Result.Value.ToString(), out score);
                }
                else
                {
                    DebugCustom.Log(string.Format("Firebae User {0} is not exist", userId));
                }
            }

            if (callback != null)
            {
                callback(score);
            }
        });
    }

    public void SaveTournamentData(TournamentData data, UnityAction<bool> callback = null)
    {
        if (!IsAuthenticated)
        {
            DebugCustom.Log("Firebase is not authenticate");
            return;
        }

        GoOnline();

        string weekRange = MasterInfo.Instance.GetWeekRangeString(timeStartChallenge);

        reference.Child(tournamentNode).Child(weekRange).Child(data.id).SetRawJsonValueAsync(data.GetJsonString()).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                if (callback != null)
                {
                    callback(false);
                }
            }
            else if (task.IsFaulted || !task.IsCompleted)
            {
                if (trySaveScoreCount < 5)
                {
                    DebugCustom.Log("Firebase try Save Score");
                    trySaveScoreCount++;
                    SaveTournamentData(data, callback);
                }
                else
                {
                    DebugCustom.Log("Firebase try Save Score FAILED " + task.Exception);

                    if (callback != null)
                    {
                        callback(false);
                    }
                }
            }
            else
            {
                trySaveScoreCount = 0;

                if (callback != null)
                {
                    callback(true);
                }
            }

            DebugCustom.Log("Firebase save user score " + task.IsCompleted);
        });
    }

    public void SaveTournamentReceivedReward(string userId, string weekRange, UnityAction<bool> callback = null)
    {
        if (!IsAuthenticated)
        {
            DebugCustom.Log("Firebase is not authenticate");
            return;
        }

        GoOnline();

        reference.Child(tournamentNode).Child(weekRange).Child(userId).Child(receivedNode).SetValueAsync(true).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                if (callback != null)
                {
                    callback(false);
                }
            }
            else if (task.IsFaulted || !task.IsCompleted)
            {
                if (trySaveReceivedRewardCount < 5)
                {
                    DebugCustom.Log("Firebase try Save Received");
                    trySaveReceivedRewardCount++;
                    SaveTournamentReceivedReward(userId, weekRange, callback);
                }
                else
                {
                    DebugCustom.Log("Firebase try Save Received FAILED");

                    if (callback != null)
                    {
                        callback(false);
                    }
                }
            }
            else
            {
                trySaveReceivedRewardCount = 0;

                if (callback != null)
                {
                    callback(true);
                }
            }

            DebugCustom.Log("Firebase save user Received " + task.IsCompleted);
        });
    }

    public void GetUserInfo(string userId, UnityAction<UserInfo> callback)
    {
        if (!IsAuthenticated)
        {
            DebugCustom.Log("Firebase is not authenticate");
            return;
        }

        GoOnline();

        reference.Child(userNode).Child(userId).Child(nameNode).GetValueAsync().ContinueWith(task =>
        {
            UserInfo info = null;

            if (task.IsCompleted)
            {
                if (task.Result.Exists)
                {
                    info = new UserInfo();
                    info.id = userId;
                    info.name = task.Result.Value == null ? "Soldier Unnamed" : task.Result.Value.ToString();
                }
            }

            if (callback != null)
            {
                callback(info);
            }
        });
    }

    public void SaveUserInfo(UserInfo info, UnityAction<bool> callback = null)
    {
        if (info == null)
        {
            DebugCustom.Log("Firebase info null");

            if (callback != null)
            {
                callback(false);
            }

            return;
        }

        if (PlayerPrefs.HasKey("__FirebasePlayerProfile"))
        {
            DebugCustom.Log("Firebase info already saved");

            if (callback != null)
            {
                callback(true);
            }

            return;
        }

        if (!IsAuthenticated)
        {
            DebugCustom.Log("Firebase is not authenticate");
            return;
        }

        GoOnline();

        reference.Child(userNode).Child(info.id).SetRawJsonValueAsync(info.GetJsonString()).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                if (callback != null)
                {
                    callback(false);
                }
            }
            else if (task.IsFaulted || !task.IsCompleted)
            {
                if (trySaveInfoCount < 5)
                {
                    DebugCustom.Log("Firebase try Save Info");
                    trySaveInfoCount++;
                    SaveUserInfo(info, callback);
                }
                else
                {
                    DebugCustom.Log("Firebase try Save Info FAILED");

                    if (callback != null)
                    {
                        callback(false);
                    }
                }
            }
            else
            {
                PlayerPrefs.SetString("__FirebasePlayerProfile", info.authId);
                PlayerPrefs.Save();

                trySaveInfoCount = 0;

                if (callback != null)
                {
                    callback(true);
                }
            }
        });
    }

    public void SaveUserData(string userId, UnityAction<bool> callback = null)
    {
        if (!IsAuthenticated)
        {
            DebugCustom.Log("Firebase is not authenticate");
            return;
        }

        GoOnline();

        string json = ProfileManager.UserProfile.GetRawUserProfileJsonString();

        reference.Child(userDataNode).Child(userId).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                if (callback != null)
                {
                    callback(false);
                }
            }
            else if (task.IsFaulted || !task.IsCompleted)
            {
                if (trySaveUserInventory < 5)
                {
                    DebugCustom.Log("Firebase try Save Inventory");
                    trySaveUserInventory++;
                    SaveUserData(userId, callback);
                }
                else
                {
                    DebugCustom.Log("Firebase try Save Inventory FAILED");

                    if (callback != null)
                    {
                        callback(false);
                    }
                }
            }
            else
            {
                trySaveUserInventory = 0;

                if (callback != null)
                {
                    callback(true);
                }
            }
        });
    }

    public void GetUserData(string userId, UnityAction<RawUserProfile> callback)
    {
        if (!IsAuthenticated)
        {
            DebugCustom.Log("Firebase is not authenticate");
            return;
        }

        GoOnline();

        reference.Child(userDataNode).Child(userId).GetValueAsync().ContinueWith(task =>
        {
            RawUserProfile inven = null;

            if (task.IsCompleted)
            {
                if (task.Result.Exists)
                {
                    try
                    {
                        inven = JsonConvert.DeserializeObject<RawUserProfile>(task.Result.GetRawJsonValue());
                    }
                    catch { }
                }
                else
                {
                    DebugCustom.Log("Firebase user inventory empty");
                }
            }

            if (callback != null)
            {
                callback(inven);
            }
        });
    }

    public void SearUserByName(string name, UnityAction<UserInfo> callback)
    {
        if (!IsAuthenticated)
        {
            DebugCustom.Log("Firebase is not authenticate");
            return;
        }

        GoOnline();

        reference.Child(userNode).OrderByChild(nameNode).EqualTo(name).GetValueAsync().ContinueWith(task =>
        {
            UserInfo user = null;

            if (task.IsCompleted)
            {
                if (task.Result.Exists)
                {
                    try
                    {
                        foreach (var data in task.Result.Children)
                        {
                            user = new UserInfo();

                            Dictionary<string, object> info = data.Value as Dictionary<string, object>;
                            user.id = data.Key;
                            info.TryGetValue("authId", out user.authId);
                            info.TryGetValue("name", out user.name);
                            info.TryGetValue("email", out user.email);
                        }
                    }
                    catch { }
                }
            }

            if (callback != null)
            {
                callback(user);
            }
        });
    }

    public void AuthenWithFacebook(string fbId, string accessToken, UnityAction<UserInfo> callback = null)
    {
        if (isAuthenticating)
        {
            DebugCustom.Log("Firebase Authenticating...");
            return;
        }

        if (AuthUserInfo != null)
        {
            DebugCustom.Log("Firebase is already authenticated");

            if (callback != null)
                callback(AuthUserInfo);

            return;
        }

        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        Credential credential = FacebookAuthProvider.GetCredential(accessToken);
        isAuthenticating = true;
        AuthUserInfo = null;

        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            isAuthenticating = false;

            if (task.IsCanceled)
            {
                DebugCustom.LogError("Firebase Authen was canceled.");
            }
            else if (task.IsFaulted || !task.IsCompleted)
            {
                DebugCustom.LogError("Firebase Authen Fail: " + task.Exception);
            }
            else
            {
                FirebaseUser user = task.Result;
                string authId = user.UserId;
                string name = FbController.Instance.LoggedInUserInfo.Name;
                string email = FbController.Instance.LoggedInUserInfo.Email;

                AuthUserInfo = new UserInfo(fbId, authId, name, email);

                SaveUserInfo(AuthUserInfo);
            }

            if (callback != null)
            {
                callback(AuthUserInfo);
            }
        });
    }

    #endregion


    #region PRIVATE METHODS

    private void GoOffline()
    {
        if (isDatabaseOnline)
        {
            reference.Database.GoOffline();
            isDatabaseOnline = false;

            DebugCustom.Log("FIREBASE OFFLINE");
        }
    }

    private void GoOnline()
    {
        timeoutOffline = 180f;

        if (!isDatabaseOnline)
        {
            if (timeoutOfflineCoroutine != null)
            {
                StopCoroutine(timeoutOfflineCoroutine);
            }

            reference.Database.GoOnline();
            isDatabaseOnline = true;

            timeoutOfflineCoroutine = CheckAFK();
            StartCoroutine(timeoutOfflineCoroutine);

            DebugCustom.Log("FIREBASE ONLINE");
        }
    }

    private IEnumerator CheckAFK()
    {
        while (timeoutOffline > 0f)
        {
            timeoutOffline -= Time.deltaTime;
            yield return null;
        }

        GoOffline();
    }

    #endregion

}
