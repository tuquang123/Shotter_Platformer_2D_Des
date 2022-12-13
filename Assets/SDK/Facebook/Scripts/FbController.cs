using Facebook.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.Networking;
using Newtonsoft.Json;
//using BestHTTP;

public class FbController : MonoBehaviour
{
    public static FbController Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject singletonObject = new GameObject("FB Controller");
                _instance = singletonObject.AddComponent<FbController>();

                DebugCustom.Log("Create FB Controller");
            }

            return _instance;
        }

        private set { }
    }
    public FbUserInfo LoggedInUserInfo { get; private set; }

    private static FbController _instance;
    private Sprite userProfilePicture;
    private bool isSendingDataToFacebook = false;
    private string profileImgLinkFormat = "https://graph.facebook.com/{0}/picture?width=130&height=130";

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this);

        InitFacebook();
        DontDestroyOnLoad(this);
    }


    #region AUTHENTICATION

    /// <summary>
    /// Khởi tạo Facebook, được gọi trong Awake
    /// </summary>
    /// <param name="callback">Tùy chọn hàm trả về sau khi Init thành công</param>
    public void InitFacebook(UnityAction callback = null)
    {
        if (FB.IsInitialized == false)
        {
            FB.Init(() => OnInitComplete(callback), OnHideUnity);
        }
        else
        {
            FB.ActivateApp();
        }
    }

    /// <summary>
    /// Đăng nhập Facebook với quyền chỉ GET về.
    /// Nếu vào game và đăng nhập FB sau đó ra app FB đăng nhập tài khoản khác thì vào game sẽ tự động Logout và đăng nhập lại tài khoản mới
    /// </summary>
    /// <param name="callback">Tùy chọn hàm trả về sau khi callback được gọi. Tham số bool trả về thành công = true hay không = false</param>
    public void LoginWithReadPermission(UnityAction<bool> callback = null)
    {
        if (FB.IsInitialized)
        {
            var perms = new List<string>() { "public_profile", "email", "user_friends" };

            FB.LogInWithReadPermissions(
                perms,
                (ILoginResult result) => LoginCallback(result, callback));
        }
        else
        {
            InitFacebook(() =>
            {
                LoginWithReadPermission(callback);
            });
        }
    }

    /// <summary>
    /// Đăng nhập Facebook với quyền POST như Share, Post Score
    /// </summary>
    /// <param name="callback">Tùy chọn hàm trả về sau khi callback được gọi. Tham số bool trả về thành công = true hay không = false</param>
    public void LoginWithPublishAction(UnityAction<bool> callback = null)
    {
        if (FB.IsInitialized)
        {
            FB.LogInWithPublishPermissions(
                new List<string>() { "publish_actions" },
                (ILoginResult result) => LoginCallback(result, callback));
        }
        else
        {
            InitFacebook(() =>
            {
                LoginWithPublishAction(callback);
            });
        }
    }

    /// <summary>
    /// Thoát phiên đăng nhập hiện tại
    /// </summary>
    public void Logout(UnityAction callback = null)
    {
        FB.LogOut();
        LoggedInUserInfo = null;
        userProfilePicture = null;
        PlayerPrefs.DeleteKey("FbUserInfo");

        if (callback != null)
        {
            callback();
        }
    }

    #endregion


    #region GET INFOMATION

    /// <summary>
    /// Lấy ID, Tên, Email, Score và Link ảnh đại điện của user hiện tại
    /// </summary>
    /// <param name="callback">Hàm trả về một đối tượng FbUserInfo</param>
    public void GetLoggedInUserInfomation(UnityAction<FbUserInfo> callback)
    {
        if (FB.IsLoggedIn)
        {
            if (LoggedInUserInfo == null)
            {
                FB.API(
                    "/me?fields=name,email,id",
                    HttpMethod.GET,
                    (IGraphResult result) => GetUserInfoCallback(result, callback));
            }
            else
            {
                callback(LoggedInUserInfo);
            }
        }
        else
        {
            LoginWithReadPermission(success =>
            {
                if (success)
                    GetLoggedInUserInfomation(callback);
            });
        }
    }

    /// <summary>
    /// Lấy ảnh đại diện của user hiện tại
    /// </summary>
    /// <param name="callback">Hàm trả về một Sprite có thể set vào Image trong Canvas</param>
    public void GetLoggedInUserProfilePicture(UnityAction<Sprite> callback)
    {
        if (LoggedInUserInfo != null)
        {
            if (userProfilePicture == null)
            {
                GetProfilePicture(LoggedInUserInfo.ProfileImageURL, callback);
            }
            else
            {
                DebugCustom.Log("Profile picture already downloaded");
                callback(userProfilePicture);
            }
        }
    }

    /// <summary>
    /// Lấy ảnh đại diện của bạn bè bằng Image URL
    /// </summary>
    /// <param name="imageURL">Link ảnh</param>
    /// <param name="callback">Hàm trả về một Sprite có thể set vào Image trong Canvas</param>
    public void GetProfilePicture(string imageURL, UnityAction<Sprite> callback)
    {
        StartCoroutine(AsyncGetProfilePicture(imageURL, callback));

        //HTTPRequest request = new HTTPRequest(new Uri(imageURL), callback: (req, res) => OnRequestFinished(req, res, callback));
        //request.Send();
    }

    /// <summary>
    /// Lấy ảnh đại diện của bạn bè bằng Facebbok ID
    /// </summary>
    /// <param name="fbId">Facebook Id</param>
    /// <param name="callback">Hàm trả về một Sprite có thể set vào Image trong Canvas</param>
    public void GetProfilePictureById(string fbId, UnityAction<Sprite> callback)
    {
        string imgUrl = string.Format(profileImgLinkFormat, fbId);
        GetProfilePicture(imgUrl, callback);
    }

    #endregion


    #region SCREENSHOT & AVATAR

    /// <summary>
    /// Lấy ảnh đại diện của user nào đó và set vào Image trong Canvas
    /// </summary>
    /// <param name="imageURL">Link ảnh</param>
    /// <param name="avatar">Image trong Canvas để set ảnh sau khi lấy</param>
    public void SetProfilePicture(string imageURL, Image avatar)
    {
        StartCoroutine(AsyncGetProfilePicture(imageURL, avatar));

        //HTTPRequest request = new HTTPRequest(new Uri(imageURL), callback: (req, res) => OnRequestFinished(req, res, avatar));
        //request.Send();
    }

    /// <summary>
    /// Lấy ảnh đại diện của user nào đó và set vào Image trong Canvas
    /// </summary>
    /// <param name="fbId">Facebook Id</param>
    /// <param name="avatar">Image trong Canvas để set ảnh sau khi lấy</param>
    public void SetProfilePictureById(string fbId, Image avatar)
    {
        string imgUrl = string.Format(profileImgLinkFormat, fbId);
        SetProfilePicture(imgUrl, avatar);
    }

    /// <summary>
    /// Chụp ảnh lại màn hình để Share
    /// </summary>
    public void TakeScreenshotForShare()
    {
//        Application.CaptureScreenshot("FbCaptureImage.png");
        DebugCustom.Log("Take Screenshot");
    }

    /// <summary>
    /// Lấy Screenshot vừa chụp
    /// </summary>
    /// <returns></returns>
    public Texture2D GetScreenshot()
    {
        Texture2D image = new Texture2D(Screen.width, Screen.height);

#if UNITY_EDITOR
        string screenshotPath = Application.dataPath.Substring(0, Application.dataPath.Length - 7) + "/FbCaptureImage.png";
#else
        string screenshotPath = Application.persistentDataPath + "/FbCaptureImage.png";
#endif
        if (File.Exists(screenshotPath))
        {
            byte[] imageData = File.ReadAllBytes(screenshotPath);
            image.LoadImage(imageData);
        }

        return image;
    }

    #endregion


    #region SHARING TO FACEBOOK

    /// <summary>
    /// Share ảnh lên facebook (Cần quyền publish_actions, LoginWithPublishPermission)
    /// </summary>
    /// <param name="message">Nội dung chia sẻ lên FB</param>
    /// <param name="callback">Hàm trả về bool thành công = true, thất bại = false</param>
    public void ShareScreenshot(string message, UnityAction<bool> callback = null)
    {
        if (isSendingDataToFacebook)
        {
            DebugCustom.Log("Sending data to FB");
            return;
        }

        if (FB.IsLoggedIn)
        {
            if (!AccessToken.CurrentAccessToken.Permissions.Contains("publish_actions"))
            {
                LoginWithPublishAction(loginStatus =>
                {
                    if (loginStatus)
                    {
                        ShareScreenshot(message, callback);
                    }
                });
            }
            else
            {
                isSendingDataToFacebook = true;

                byte[] screenshot = GetScreenshot().EncodeToJPG();

                WWWForm wwwForm = new WWWForm();
                wwwForm.AddBinaryData("image", screenshot);
                wwwForm.AddField("message", message);
                FB.API(
                    "/me/photos",
                    HttpMethod.POST,
                    (IGraphResult result) => ShareScreenshotCallback(result, callback),
                    wwwForm);
            }
        }
        else
        {
            LoginWithReadPermission(success =>
            {
                if (success)
                {
                    ShareScreenshot(message, callback);
                }
            });
        }
    }

    /// <summary>
    /// Share một ảnh có sẵn lên Facebook (Cần quyền publish_actions, LoginWithPublishPermission)
    /// </summary>
    /// <param name="message">Nội dung chia sẻ lên FB</param>
    /// <param name="image">Ảnh</param>
    /// <param name="callback">Hàm trả về bool thành công = true, thất bại = false</param>
    public void ShareImage(string message, Texture2D image, UnityAction<bool> callback = null)
    {
        if (isSendingDataToFacebook || image == null)
        {
            DebugCustom.Log("Sending data to FB or Image is null");
            return;
        }

        if (FB.IsLoggedIn)
        {
            if (!AccessToken.CurrentAccessToken.Permissions.Contains("publish_actions"))
            {
                LoginWithPublishAction(loginStatus =>
                {
                    if (loginStatus)
                    {
                        ShareImage(message, image, callback);
                    }
                });
            }
            else
            {
                isSendingDataToFacebook = true;

                byte[] imageData = image.EncodeToJPG();

                WWWForm wwwForm = new WWWForm();
                wwwForm.AddBinaryData("image", imageData);
                wwwForm.AddField("message", message);
                FB.API(
                    "/me/photos",
                    HttpMethod.POST,
                    (IGraphResult result) => ShareScreenshotCallback(result, callback),
                    wwwForm);
            }
        }
        else
        {
            LoginWithReadPermission(success =>
            {
                if (success)
                {
                    ShareImage(message, image, callback);
                }
            });
        }
    }

    /// <summary>
    /// Share game lên New Feed
    /// </summary>
    /// <param name="link">Link cần share</param>
    /// <param name="linkName">Tên link</param>
    /// <param name="linkCaption">Tóm tắt</param>
    /// <param name="linkDescription">Mô tả chi tiết</param>
    /// <param name="linkPicture">Link ảnh</param>
    /// <param name="callback">Hàm trả về bool thành công = true, thất bại = false</param>
    public void FeedShare(string link, string linkName, string linkCaption, string linkDescription, string linkPicture, UnityAction<bool> callback = null)
    {
        if (FB.IsLoggedIn)
        {
            FB.FeedShare(
                link: new Uri(link),
                linkName: linkName,
                linkCaption: linkCaption,
                linkDescription: linkDescription,
                picture: new Uri(linkPicture),
                callback: (IShareResult result) => FeedsShareCallback(result, callback)
                );
        }
        else
        {
            LoginWithReadPermission(success =>
            {
                if (success)
                {
                    FeedShare(link, linkName, linkCaption, linkDescription, linkPicture, callback);
                }
            });
        }
    }

    /// <summary>
    /// Share link
    /// </summary>
    /// <param name="link">Link cần share</param>
    /// <param name="callback">Hàm trả về bool thành công = true, thất bại = false</param>
    public void ShareLink(string link, UnityAction<bool> callback = null)
    {
        if (FB.IsLoggedIn)
        {
            FB.ShareLink(
                contentURL: new Uri(link),
                callback: (IShareResult result) => FeedsShareCallback(result, callback));
        }
        else
        {
            LoginWithReadPermission(success =>
            {
                if (success)
                {
                    ShareLink(link, callback);
                }
            });
        }
    }

    #endregion


    #region FRIEND ZONE

    /// <summary>
    /// Lấy danh sách bạn bè
    /// Lưu ý: Hàm này lấy danh sách bạn bè thông qua gọi API invitable_friends nên bạn bè trả về có ID là một Alias không thể dùng ID này để lấy ảnh thông qua "graph.facebook.com"
    /// </summary>
    /// <param name="numberFriend">Số lượng bạn cần lấy, tối đa là 1000</param>
    /// <param name="callback">Hàm trả về danh sách bạn bè</param>
    public void GetFriends(int numberFriend, bool justFriendPlayedGame, UnityAction<List<FbUserInfo>> callback)
    {
        if (numberFriend > 0)
        {
            if (FB.IsLoggedIn)
            {
                string fbApi = justFriendPlayedGame ? "/me/friends?fields=id,name,picture.width(130).height(130)&limit=1000" : "/me/invitable_friends?fields=id,name,picture.width(130).height(130)&limit=1000";

                FB.API(
                    fbApi,
                    HttpMethod.GET,
                    (IGraphResult result) => GetFriendsCallback(numberFriend, result, callback));
            }
            else
            {
                LoginWithReadPermission(success =>
                {
                    if (success)
                    {
                        GetFriends(numberFriend, justFriendPlayedGame, callback);
                    }
                });
            }
        }
    }

    /// <summary>
    /// Mời bạn bè chơi game. Nếu danh sách bạn bè null hoặc không có ai thì sẽ hiện Dialog mời bạn bè mặc định của Facebook
    /// </summary>
    /// <param name="message">Lời nhắn</param>
    /// <param name="friendList">Danh sách bạn bè muốn gửi</param>
    /// <param name="callback">Hàm trả về số lượng bạn bè được mời</param>
    public void InviteFriends(string message, List<string> friendList, string data = "", UnityAction<int> callback = null)
    {
        if (FB.IsLoggedIn)
        {
            FB.AppRequest(
                message: message,
                to: friendList,
                data: data,
                title: Application.productName,
                callback: (IAppRequestResult result) => InviteFriendsCallback(result, callback));
        }
        else
        {
            LoginWithReadPermission(success =>
            {
                if (success)
                {
                    InviteFriends(message, friendList, data, callback);
                }
            });
        }
    }

    /// <summary>
    /// Gửi app request cho bạn bè có đính kèm (Life, Move, Gift, ...)
    /// </summary>
    /// <param name="message"></param>
    /// <param name="friendList"></param>
    /// <param name="data"></param>
    /// <param name="callback"></param>
    public void SendDataToFriendViaAppRequest(string message, List<string> friendList, string data, UnityAction<int> callback = null)
    {
        InviteFriends(message, friendList, data, callback);
    }

    /// <summary>
    /// Lấy hết app request từ bạn bè về có thể check nhận được gì từ bạn bè không
    /// </summary>
    /// <param name="callback"></param>
    public void GetDataFromFriendAppRequest(UnityAction<List<FbAppRequestData>> callback)
    {
        if (FB.IsLoggedIn)
        {
            FB.API("/me/apprequests?fields=id,data,from", HttpMethod.GET, result => GetDataFromAppRequestCallback(result, callback));
        }
        else
        {
            LoginWithReadPermission(success =>
            {
                if (success)
                {
                    GetDataFromFriendAppRequest(callback);
                }
            });
        }
    }

    /// <summary>
    /// Xóa app request từ bạn bè
    /// </summary>
    /// <param name="requestId">Id của request lấy ở FbAppRequestData</param>
    public void DeleteDataFromFriendAppRequest(List<FbAppRequestData> requestDataList, UnityAction<List<string>> callback = null)
    {
        List<string> requestIds = new List<string>();

        for (int i = 0, amount = requestDataList.Count; i < amount; i++)
        {
            requestIds.Add(requestDataList[i].requestId);
        }

        if (requestIds.Count > 0)
        {
            if (FB.IsLoggedIn)
            {
#if UNITY_ANDROID
                string json = JsonConvert.SerializeObject(requestIds);
                DebugCustom.Log("DELETE: " + json);
                string fbApi = string.Format("?ids={0}&method=delete", json);
                FB.API(fbApi, HttpMethod.GET, result => DeleteAppRequestCallback(result, callback));
#elif UNITY_IOS
				StartCoroutine (SendDeleteRequest (requestIds, callback));
#endif
            }
            else
            {
                LoginWithReadPermission(success =>
                {
                    if (success)
                    {
                        DeleteDataFromFriendAppRequest(requestDataList, callback);
                    }
                });
            }
        }
    }

    #endregion


    #region PRIVATE METHODS

    private IEnumerator SendDeleteRequest(List<string> requestIds, UnityAction<List<string>> callback)
    {
        string json = JsonConvert.SerializeObject(requestIds);
        string graph = string.Format("https://graph.facebook.com?ids={0}&method=delete&access_token={1}", json, AccessToken.CurrentAccessToken.TokenString);

        using (UnityWebRequest www = UnityWebRequest.Get(graph))
        {
            yield return www.Send();

            List<string> deletedIds = null;

            if (www.isNetworkError)
            {
                DebugCustom.Log("Graph DELETE Error: " + www.error);
            }
            else
            {
                DebugCustom.Log("Graph DELETE Ok: " + www.downloadHandler.text);

                deletedIds = new List<string>();

                try
                {
                    Dictionary<string, bool> resultDic = JsonConvert.DeserializeObject<Dictionary<string, bool>>(www.downloadHandler.text);

                    foreach (KeyValuePair<string, bool> entry in resultDic)
                    {
                        if (entry.Value)
                        {
                            deletedIds.Add(entry.Key.Split(':')[0]);
                        }
                    }
                }
                catch
                {
                    DebugCustom.Log("Graph DELETE parse json error: " + www.downloadHandler.text);
                }
            }

            if (callback != null)
            {
                callback(deletedIds);
            }
        }
    }

    private IEnumerator AsyncGetProfilePicture(string imageURL, UnityAction<Sprite> callback)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageURL);
        yield return www.Send();

        Sprite img = null;

        if (!www.isNetworkError)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            img = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

            if (imageURL.Equals(LoggedInUserInfo.ProfileImageURL))
            {
                userProfilePicture = img;
            }
        }

        if (callback != null)
        {
            callback(img);
        }
    }

    private IEnumerator AsyncGetProfilePicture(string imageURL, Image avatar)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageURL);

        yield return www.Send();

        if (!www.isNetworkError)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            avatar.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }
    }

    //private void OnRequestFinished(HTTPRequest req, HTTPResponse res, UnityAction<Sprite> callback)
    //{
    //    if (res.IsSuccess)
    //    {
    //        Sprite img = null;
    //        Texture2D texture = res.DataAsTexture2D;
    //        img = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

    //        if (req.Uri.OriginalString.Equals(LoggedInUserInfo.ProfileImageURL))
    //        {
    //            userProfilePicture = img;
    //        }

    //        if (callback != null)
    //        {
    //            callback(img);
    //        }
    //    }
    //}

    //private void OnRequestFinished(HTTPRequest req, HTTPResponse res, Image avatar)
    //{
    //    if (res.IsSuccess)
    //    {
    //        Texture2D texture = res.DataAsTexture2D;
    //        avatar.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    //    }
    //}

    #endregion


    #region FACEBOOK CALLBACK

    private void OnHideUnity(bool isUnityShown)
    {
    }

    private void OnInitComplete(UnityAction callback)
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }

#if UNITY_EDITOR
        // Load Facebook user info from PlayerPrefs
        string jsonData = PlayerPrefs.GetString("FbUserInfo", string.Empty);

        if (!string.IsNullOrEmpty(jsonData))
        {
            DebugCustom.Log("FACEBOOK LOAD USER INFO FROM PLAYERPREFS");
            LoggedInUserInfo = JsonConvert.DeserializeObject<FbUserInfo>(jsonData);
        }
#else
        if (FB.IsLoggedIn)
        {
            // Load Facebook user info from PlayerPrefs
            string jsonData = PlayerPrefs.GetString("FbUserInfo", string.Empty);

            if (string.IsNullOrEmpty(jsonData))
            {
                LoggedInUserInfo = null;
                GetLoggedInUserInfomation(null);
            }
            else
            {
                DebugCustom.Log("FACEBOOK LOAD USER INFO FROM PLAYERPREFS");
                LoggedInUserInfo = JsonConvert.DeserializeObject<FbUserInfo>(jsonData);
            }
        }
#endif

        if (callback != null)
        {
            callback();
        }
    }

    private void LoginCallback(ILoginResult result, UnityAction<bool> callback)
    {
        if (result.Cancelled == false && string.IsNullOrEmpty(result.Error))
        {
            if (LoggedInUserInfo == null)
            {
                GetLoggedInUserInfomation(info =>
                {
                    if (callback != null)
                    {
                        callback(true);
                    }
                });
            }
            else if (callback != null)
            {
                callback(true);
            }
        }
        else
        {
            DebugCustom.Log("FB Login Error: " + result.Error);

            if (result.Error != null && result.Error.Contains("logged in as different Facebook user"))
            {
                Logout();
                LoginWithReadPermission(callback);
            }
            else
            {
                if (callback != null)
                {
                    callback(false);
                }
            }
        }
    }

    private void GetUserInfoCallback(IGraphResult result, UnityAction<FbUserInfo> callback)
    {
        if (result.Cancelled == false && string.IsNullOrEmpty(result.Error))
        {
            string id = result.ResultDictionary["id"].ToString();
            string name = result.ResultDictionary["name"].ToString();
            string email = string.Empty;
            string imgURL = string.Format(profileImgLinkFormat, id);

            if (result.ResultDictionary.ContainsKey("email"))
            {
                email = result.ResultDictionary["email"].ToString();
            }

            LoggedInUserInfo = new FbUserInfo(id, name, email, imgURL);

            string jsonData = JsonConvert.SerializeObject(LoggedInUserInfo);
            PlayerPrefs.SetString("FbUserInfo", jsonData);
            PlayerPrefs.Save();

            if (callback != null)
                callback(LoggedInUserInfo);
        }
        else
        {
            DebugCustom.Log("FB get user info error: " + result.Error);

            if (callback != null)
                callback(null);
        }
    }

    private void ShareScreenshotCallback(IGraphResult result, UnityAction<bool> callback)
    {
        if (result.Cancelled == false && string.IsNullOrEmpty(result.Error))
        {
            if (callback != null)
            {
                callback(true);
            }
        }
        else
        {
            DebugCustom.Log("FB share screenshot error: " + result.Error);

            if (callback != null)
            {
                callback(false);
            }
        }

        isSendingDataToFacebook = false;
    }

    private void FeedsShareCallback(IShareResult result, UnityAction<bool> callback)
    {
        if (result.Cancelled == false && string.IsNullOrEmpty(result.Error))
        {
            if (callback != null)
            {
                callback(true);
            }
        }
        else
        {
            DebugCustom.Log("FB feed share error: " + result.Error);

            if (callback != null)
            {
                callback(false);
            }
        }
    }

    private void GetFriendsCallback(int numberFriendInvite, IGraphResult result, UnityAction<List<FbUserInfo>> callback)
    {
        List<FbUserInfo> friends = new List<FbUserInfo>();

        if (result.Cancelled == false && string.IsNullOrEmpty(result.Error))
        {
            List<object> dataList = result.ResultDictionary["data"] as List<object>;

            int startIndex = 0;
            int endIndex = 0;

            if (dataList.Count > numberFriendInvite)
            {
                startIndex = Random.Range(0, dataList.Count - numberFriendInvite);
                endIndex = startIndex + numberFriendInvite;
            }
            else if (dataList.Count == numberFriendInvite)
            {
                endIndex = numberFriendInvite;
            }
            else
            {
                endIndex = dataList.Count;
            }

            DebugCustom.Log(string.Format("List friend: {0} - Number friend to invite: {1}", dataList.Count, numberFriendInvite));
            DebugCustom.Log(string.Format("Loop from startIndex={0} < endIndex={1}; i++", startIndex, endIndex));

            for (int i = startIndex; i < endIndex; i++)
            {
                Dictionary<string, object> friendData = dataList[i] as Dictionary<string, object>;

                string id = friendData["id"].ToString();
                string name = friendData["name"].ToString();
                string imageURL = string.Empty;

                Dictionary<string, object> pictureData = friendData["picture"] as Dictionary<string, object>;
                Dictionary<string, object> picture = pictureData["data"] as Dictionary<string, object>;
                imageURL = picture["url"].ToString();

                friends.Add(new FbUserInfo(id, name, string.Empty, imageURL));
            }
        }
        else
        {
            DebugCustom.Log("FB get friends error: " + result.Error);
        }

        if (callback != null)
            callback(friends);
    }

    private void InviteFriendsCallback(IAppRequestResult result, UnityAction<int> callback)
    {
        if (result.Cancelled == false && string.IsNullOrEmpty(result.Error))
        {
            if (callback != null && result.ResultDictionary.ContainsKey("to"))
            {
#if UNITY_ANDROID
                string temp = result.ResultDictionary["to"] as string;
                string[] listIdFriendInvited = temp.Split(',');
                callback(listIdFriendInvited.Length);
#elif UNITY_IOS
                List<object> listIdFriendInvited = result.ResultDictionary["to"] as List<object>;

                if(listIdFriendInvited != null)
                    callback(listIdFriendInvited.Count);
                else
                    callback(0);
#endif
            }
        }
        else if (callback != null)
        {
            DebugCustom.Log("FB invite friends error: " + result.Error);
            callback(0);
        }
    }


    private void GetDataFromAppRequestCallback(IGraphResult result, UnityAction<List<FbAppRequestData>> callback)
    {
        List<FbAppRequestData> listData = new List<FbAppRequestData>();

        if (result.Cancelled == false && string.IsNullOrEmpty(result.Error))
        {
            List<object> listRawData = result.ResultDictionary["data"] as List<object>;

            for (int i = 0, amount = listRawData.Count; i < amount; i++)
            {
                Dictionary<string, object> parsedData = listRawData[i] as Dictionary<string, object>;

                if (!parsedData.ContainsKey("data"))
                {
                    continue;
                }

                string requestId = parsedData["id"].ToString();
                string requestData = parsedData["data"].ToString();

                Dictionary<string, object> from = parsedData["from"] as Dictionary<string, object>;
                string senderId = from["id"].ToString();
                string senderName = from["name"].ToString();

                FbAppRequestData aRequest = new FbAppRequestData(requestId, requestData, senderId, senderName);
                listData.Add(aRequest);
            }
        }
        else
        {
            DebugCustom.Log("FB data app request error: " + result.Error);
        }

        if (callback != null)
        {
            callback(listData);
        }
    }

    private void DeleteAppRequestCallback(IGraphResult result, UnityAction<List<string>> callback)
    {
        List<string> deletedIds = new List<string>();

        if (result.Cancelled == false && string.IsNullOrEmpty(result.Error))
        {
            foreach (KeyValuePair<string, object> item in result.ResultDictionary)
            {
                if ((bool)item.Value)
                {
                    deletedIds.Add(item.Key.Split(':')[0]);
                }
            }
        }
        else
        {
            DebugCustom.Log("FB delete app request error: " + result.Error);
        }

        if (callback != null)
        {
            callback(deletedIds);
        }
    }

    #endregion

}