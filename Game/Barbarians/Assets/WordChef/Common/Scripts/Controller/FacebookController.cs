using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SimpleJSON;
using Facebook.MiniJSON;
using Facebook.Unity;

public class FacebookController : MonoBehaviour
{
    public Action onFacebookInitComplete;
    public Action onFacebookLoginComplete;
    public Action onShareLinkComplete;
    public Action onAppRequestComplete;
    public Action<List<InvitableFriend>> onGetInvitableFriendsComplete;
    public static FacebookController instance;

    public bool hasInviteFriend = true;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InitFacebook();
    }

    #region Init
    public void InitFacebook()
    {
#if UNITY_EDITOR || UNITY_IPHONE || UNITY_ANDROID
        if (FB.IsInitialized) return;
        FB.Init(OnInitComplete, OnHideUnity);
#endif
    }

    private void OnInitComplete()
    {
        if (onFacebookInitComplete != null) onFacebookInitComplete();
        Debug.Log("FB.Init completed: Is user logged in? " + FB.IsLoggedIn);
        if (FB.IsLoggedIn)
            OnLoggedIn();
    }

    private void OnHideUnity(bool isGameShown)
    {
        Debug.Log("Is game showing? " + isGameShown);
    }
    #endregion

    #region Login
    public void LoginFacebook()
    {
        if (!FB.IsInitialized) return;
        CallFBLoginForRead();
    }

    protected virtual void OnLoggedIn()
    {
        if (onFacebookLoginComplete != null) onFacebookLoginComplete();

        if (hasInviteFriend && dataFriends == null)
        {
            GetInvitableFriends();
        }
    }

    private void CallFBLoginForRead()
    {
        FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" }, LoginCallback);
    }

    private void CallFBLoginForPublish()
    {
        // It is generally good behavior to split asking for read and publish
        // permissions rather than ask for them all at once.
        //
        // In your own game, consider postponing this call until the moment
        // you actually need it.
        FB.LogInWithPublishPermissions(new List<string>() { "publish_actions" }, LoginCallback);
    }

    void LoginCallback(IResult result)
    {
        Debug.Log("LoginCallback");
        if (FB.IsLoggedIn)
        {
            OnLoggedIn();
        }
    }
    #endregion

    #region Get Avatar
    private void MyPictureCallback(Texture2D texture)
    {
        // TODO
    }

    public void LoadPictureAPI(string url, Action<Texture2D> callback, int width, int height, bool isMine)
    {
        FB.API(url, HttpMethod.GET, result =>
        {
            if (this == null) return;

            if (result.Error != null)
            {
                Debug.Log(result.Error);
                return;
            }

            var imageUrl = DeserializePictureURLString(result.RawResult);
            if (isMine)
                FacebookUtils.myAvatarUrl = imageUrl;

            StartCoroutine(CUtils.LoadPicture(imageUrl, callback, width, height, !isMine));
        });
    }

    public static string DeserializePictureURLString(string response)
    {
        return DeserializePictureURLObject(Json.Deserialize(response));
    }

    public static string DeserializePictureURLObject(object pictureObj)
    {
        var picture = (Dictionary<string, object>)(((Dictionary<string, object>)pictureObj)["data"]);
        object urlH = null;
        if (picture.TryGetValue("url", out urlH))
        {
            return (string)urlH;
        }

        return null;
    }

   
    #endregion

    #region FB.AppRequest() Friend Selector
    private string FriendSelectorTitle = "";
    private string FriendSelectorMessage = "Derp";
    private string FriendSelectorFilters = "[\"app_users\"]";
    private string FriendSelectorData = "{}";
    private string FriendSelectorExcludeIds = "";
    private string FriendSelectorMax = "";

    private void CallAppRequestAsFriendSelector()
    {
        // If there's a Max Recipients specified, include it
        int? maxRecipients = null;
        if (FriendSelectorMax != "")
        {
            try
            {
                maxRecipients = Int32.Parse(FriendSelectorMax);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        // Include the exclude ids
        string[] excludeIds = (FriendSelectorExcludeIds == "") ? null : FriendSelectorExcludeIds.Split(',');
        List<object> FriendSelectorFiltersArr = null;
        if (!String.IsNullOrEmpty(FriendSelectorFilters))
        {
            try
            {
                FriendSelectorFiltersArr = Facebook.MiniJSON.Json.Deserialize(FriendSelectorFilters) as List<object>;
            }
            catch
            {
                throw new Exception("JSON Parse error");
            }
        }

        FB.AppRequest(
            FriendSelectorMessage,
            null,
            FriendSelectorFiltersArr,
            excludeIds,
            maxRecipients,
            FriendSelectorData,
            FriendSelectorTitle,
            HandleResult
        );
    }

    private void HandleResult(IResult result)
    {
        if (result == null)
        {
            return;
        }

        // Some platforms return the empty string instead of null.
        if (!string.IsNullOrEmpty(result.Error))
        {
            Debug.Log("Error Response:\n" + result.Error);
        }
        else if (result.Cancelled)
        {
            Debug.Log("Cancelled Response:\n" + result.RawResult);
        }
        else if (!string.IsNullOrEmpty(result.RawResult))
        {
            Debug.Log("Success Response:\n" + result.RawResult);
        }
        else
        {
            Debug.Log("Empty Response\n");
        }
    }
    #endregion

    #region FB.AppRequest() Direct Request
    [Header("App request")]
    public string DirectRequestTitle = "";
    public string DirectRequestMessage = "";
    private string DirectRequestTo = "";

    private void CallAppRequestAsDirectRequest()
    {
        if (DirectRequestTo == "")
        {
            throw new ArgumentException("\"To Comma Ids\" must be specificed", "to");
        }

        FB.AppRequest(
            DirectRequestMessage,
            DirectRequestTo.Split(','),
            null,
            null,
            null,
            "",
            DirectRequestTitle,
            AppRequestCallback
        );
    }

    private void AppRequestCallback(IResult result)
    {
        if (CheckFBResult(result))
        {
            FacebookUtils.invitableOffset++;
            GetInvitableFriends();
            if (onAppRequestComplete != null) onAppRequestComplete();
        }
    }
    #endregion

    #region Share link
    [Header("Share link")]
    public string linkToShare;

    public void ShareLink()
    {
        FB.Mobile.ShareDialogMode = ShareDialogMode.FEED;
        FB.ShareLink(new Uri(linkToShare), callback: HandleResultShareLink);
    }

    private void HandleResultShareLink(IResult result)
    {
        if (CheckFBResult(result))
            onShareLinkComplete();
    }
    #endregion

    #region Take screenshot
    private IEnumerator TakeScreenshot()
    {
        yield return new WaitForEndOfFrame();

        var width = Screen.width;
        var height = Screen.height;
        var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        // Read screen contents into the texture
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();
        byte[] screenshot = tex.EncodeToPNG();

        var wwwForm = new WWWForm();
        wwwForm.AddBinaryData("image", screenshot, "InteractiveConsole.png");
        wwwForm.AddField("message", "herp derp.  I did a thing!  Did I do this right?");

        FB.API("me/photos", HttpMethod.POST, HandleResult, wwwForm);
    }
    #endregion

    #region Custom invite friends
    public JSONArray dataFriends = null;
    public List<InvitableFriend> invitableFriends;

    public bool CustomInviteFriends()
    {
        if (invitableFriends != null)
        {
            var dialog = (InviteFriendsDialog)DialogController.instance.GetDialog(DialogType.InviteFriends);
            dialog.Build(invitableFriends);
            DialogController.instance.ShowDialog(dialog, DialogShow.STACK);
            return true;
        }
        else
        {
            GetInvitableFriends();
            return false;
        }
    }

    public void GetInvitableFriends()
    {
        if (dataFriends != null)
        {
            GetInvitableFriends(dataFriends);
            return;
        }

        FB.API("me/invitable_friends?fields=id,name,picture.height(100).width(100)&limit=5000", HttpMethod.GET, result =>
        {
            if (CheckFBResult(result) == false) return;

            var N = JSON.Parse(result.RawResult);
            dataFriends = N["data"].AsArray;

            SaveFriendsForMap(dataFriends);
            GetInvitableFriends(dataFriends);
        });
    }

    private void GetInvitableFriends(JSONArray data)
    {
        const int maxInvite = CommonConst.MAX_INVITE_FRIEND;
        const int delta = maxInvite - CommonConst.MIN_INVITE_FRIEND;

        int invitableOffset = FacebookUtils.invitableOffset;
        int from = invitableOffset * maxInvite;
        int to = from + maxInvite;
        if (to <= data.Count)
        {
            // Do nothing.
        }
        else if (to > data.Count + delta)
        {
            if (invitableOffset == 0)
            {
                return;
            }

            invitableOffset = 0;
            from = invitableOffset * maxInvite;
            to = from + maxInvite;
        }
        else
        {
            to = data.Count;
        }

        invitableFriends = new List<InvitableFriend>();
        for (int i = from; i < to; i++)
        {
            string name = data[i]["name"].Value;
            string id = data[i]["id"].Value;
            string avatarUrl = data[i]["picture"]["data"]["url"].Value;

            invitableFriends.Add(new InvitableFriend(id, name, avatarUrl));
        }

        if (onGetInvitableFriendsComplete != null)
        {
            onGetInvitableFriendsComplete(invitableFriends);
        }
    }

    public void InviteFriends(string recipients)
    {
        DirectRequestTo = recipients;
        CallAppRequestAsDirectRequest();
    }
    #endregion

    #region Post & Delete Score
    public void PostScore(int score)
    {
        if (!FB.IsLoggedIn) return;
        if (!FacebookUtils.HasPublishActions()) return;

        var query = new Dictionary<string, string>();
        query["score"] = score.ToString();
        FB.API("/me/scores", HttpMethod.POST, result =>
        {
            if (result.Error != null)
                Toast.instance.ShowMessage(result.Error);
        }, query);
    }

    public void DeleteScore()
    {
        if (!FB.IsLoggedIn) return;

        FB.API("/me/scores", HttpMethod.DELETE, result =>
        {
            Debug.Log(result.RawResult);
        });
    }

    public void GetScore(Action<int> onGetScoreComplete)
    {
        if (!FB.IsLoggedIn)
        {
            onGetScoreComplete(-1);
            return;
        }

        FB.API("/me/scores", HttpMethod.GET, result =>
        {
            if (result.Error == null && !string.IsNullOrEmpty(result.RawResult))
            {
                var N = JSON.Parse(result.RawResult);
                if (N["data"].AsArray.Count != 0)
                {
                    int score = N["data"].AsArray[0]["score"].AsInt;
                    onGetScoreComplete(score);
                }
                else
                {
                    onGetScoreComplete(-1);
                }
            }
            else
            {
                onGetScoreComplete(-1);
            }
        });
    }
    #endregion

    #region Utils

    private bool CheckFBResult(IResult result)
    {
        if (result == null) return false;
        if (!string.IsNullOrEmpty(result.Error)) return false;
        if (result.Cancelled) return false;
        if (string.IsNullOrEmpty(result.RawResult)) return false;

        return true;
    }

    private void SaveUserPermission()
    {
        FB.API("/me/permissions", HttpMethod.GET, result =>
        {
            if (CheckFBResult(result) == false) return;
            try
            {
                var N = JSON.Parse(result.RawResult);
                var arr = N["data"].AsArray;

                List<string> permissionsList = new List<string>();
                foreach (JSONNode node in arr)
                {
                    string permission = node["permission"].Value;
                    string status = node["status"].Value;

                    if (status == "granted")
                    {
                        permissionsList.Add(permission);
                    }
                }
                string permissions = CUtils.BuildStringFromCollection(permissionsList);
                FacebookUtils.permissions = permissions;
            }
            catch (Exception) { };
        });
    }

    private void SaveFriendsForMap(JSONArray dataFriends)
    {
        if (FacebookUtils.friendAvatarUrls != "") return;

        List<string> avatarUrlList = new List<string>();
        List<int> selectedIndexes = new List<int>();
        for (int i = 0; i < CommonConst.MAX_FRIEND_IN_MAP; i++)
        {
            if (selectedIndexes.Count >= dataFriends.Count) break;
            int index = 0;
            while (true)
            {
                index = UnityEngine.Random.Range(0, dataFriends.Count - 1);
                if (!selectedIndexes.Contains(index))
                {
                    selectedIndexes.Add(index);
                    break;
                }
            }
            string avatarUrl = dataFriends[index]["picture"]["data"]["url"].Value;
            avatarUrlList.Add(avatarUrl);
        }

        string avatarUrls = CUtils.BuildStringFromCollection(avatarUrlList);
        FacebookUtils.friendAvatarUrls = avatarUrls;

        // levels
        List<int> friendLevels = new List<int>();
        for (int i = 0; i < CommonConst.START_FRIEND_LEVELS.Length; i++)
        {
            friendLevels.Add(CommonConst.START_FRIEND_LEVELS[i]);
        }
        string strFriendLevels = CUtils.BuildStringFromCollection(friendLevels);
        FacebookUtils.friendLevels = strFriendLevels;

        // scores
        for (int i = 0; i < friendLevels.Count; i++)
        {
            List<int> scores = new List<int>();
            for (int level = 1; level < friendLevels[i]; level++)
            {
                int baseScore = CommonConst.GetTargetScore(level);
                int score = UnityEngine.Random.Range(baseScore, baseScore * 3);
                score = (score / 10) * 10;
                scores.Add(score);
            }
            string strScores = CUtils.BuildStringFromCollection(scores);
            FacebookUtils.SetFriendScores(i, strScores);
        }
    }

    #endregion
}
