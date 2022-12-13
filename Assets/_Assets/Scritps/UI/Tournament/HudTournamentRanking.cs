using UnityEngine;
using EnhancedUI.EnhancedScroller;
using EnhancedUI;
using UnityEngine.UI;
using System.Collections.Generic;
using Facebook.Unity;
using System;
using System.Collections;
using Newtonsoft.Json;

public class HudTournamentRanking : MonoBehaviour, IEnhancedScrollerDelegate
{
    public EnhancedScroller scroller;
    public EnhancedScrollerCellView cellViewTournamentRank;

    public GameObject popupRank;
    public GameObject groupRank;
    public GameObject groupRewards;

    [Header("TOURNAMENT INFO")]
    public Text textRemainingTime;
    public Text textStartDate;
    public Text textEndDate;
    public GameObject groupTicket;
    public GameObject groupGem;
    public Text textGemChallenge;
    public GameObject tipRefreshScoreBoard;

    [Header("PLAYER INFO")]
    public Text textPlayerScore;
    public Text textPlayerFbName;
    public Image imgPlayerAvatar;
    public Sprite sprLoadingAvatar;
    public Text textRankProgress;
    public Image imgRankProgress;
    public Image imgRankIcon;
    public Text textFreeEntrance;

    [Header("TABS")]
    public Image imgTabRank;
    public Sprite sprTabRankSelect;
    public Sprite sprTabRankUnselect;
    public Image imgTabReward;
    public Sprite sprTabRewardSelect;
    public Sprite sprTabRewardUnselect;
    public GameObject notiRankReward;

    [Header("REWARDS")]
    public CellViewRankReward[] cellViewRewards;

    private int priceChallenge = 0;
    private SmallList<CellViewTournamentRankData> rankData = new SmallList<CellViewTournamentRankData>();
    private IEnumerator coroutineTimerSeason;


    private void Awake()
    {
        scroller.CreateContainer();
        scroller.Delegate = this;

        EventDispatcher.Instance.RegisterListener(EventID.GetFacebookAvatarDone, (sender, param) => OnGetFacebookAvatarDone());
        EventDispatcher.Instance.RegisterListener(EventID.GetFacebookNameDone, (sender, param) => OnGetFacebookNameDone());
        EventDispatcher.Instance.RegisterListener(EventID.ClaimTournamentRankReward, (sender, param) => CheckNotification());

        CheckWeek();

        textPlayerFbName.text = GameData.playerTournamentData.fbName;
        imgPlayerAvatar.sprite = GameData.playerTournamentData.sprAvatar != null ? GameData.playerTournamentData.sprAvatar : sprLoadingAvatar;
    }

    private void OnEnable()
    {
        SwitchTabRank();
        CheckTimeRemaining();
        CheckNotification();
    }

    private void OnDisable()
    {
        if (coroutineTimerSeason != null)
        {
            StopCoroutine(coroutineTimerSeason);
            coroutineTimerSeason = null;
        }
    }

    public void SwitchTabRank()
    {
        imgTabRank.sprite = sprTabRankSelect;
        imgTabReward.sprite = sprTabRewardUnselect;
        groupRank.SetActive(true);
        groupRewards.SetActive(false);
    }

    public void SwitchTabReward()
    {
        imgTabRank.sprite = sprTabRankUnselect;
        imgTabReward.sprite = sprTabRewardSelect;
        groupRank.SetActive(false);
        groupRewards.SetActive(true);

        for (int i = 0; i < cellViewRewards.Length; i++)
        {
            cellViewRewards[i].Load();
        }
    }

    private void CheckWeek()
    {
        string curWeek = MasterInfo.Instance.GetCurrentWeekRangeString();

        if (string.Compare(curWeek, ProfileManager.UserProfile.weekLastLogin) != 0)
        {
            ProfileManager.UserProfile.weekLastLogin.Set(curWeek);
            ProfileManager.UserProfile.isClaimedRank1.Set(false);
            ProfileManager.UserProfile.isClaimedRank2.Set(false);
            ProfileManager.UserProfile.isClaimedRank3.Set(false);
            ProfileManager.UserProfile.isClaimedRank4.Set(false);
            ProfileManager.UserProfile.isClaimedRank5.Set(false);
            ProfileManager.UserProfile.isClaimedRank6.Set(false);
            ProfileManager.UserProfile.tournamentGunProfile.Set(string.Empty);
        }

        if (GameData.playerTournamentData.isReceivedTopRankReward == false)
        {
            GameData.playerTournamentData.isReceivedTopRankReward = true;

            FireBaseDatabase.Instance.GetTopTournamentForRewarded(data =>
            {
                DebugCustom.Log("Get top previous week " + MasterInfo.Instance.GetPreviousWeekRangeString() + " - " + JsonConvert.SerializeObject(data));

                bool isCanReceiveTopReward = false;
                int rank = 0;

                for (int i = data.Count - 1; i >= 0; i--)
                {
                    rank++;

                    if (string.Compare(data[i].id, AccessToken.CurrentAccessToken.UserId) == 0 && data[i].received == false)
                    {
                        isCanReceiveTopReward = true;
                        break;
                    }
                }

                if (isCanReceiveTopReward)
                {
                    if (GameData.tournamentTopRankRewards.ContainsKey(rank - 1))
                    {
                        List<RewardData> rewards = GameData.tournamentTopRankRewards[rank - 1];
                        RewardUtils.Receive(rewards);
                        Popup.Instance.ShowReward(
                            rewards: rewards,
                             content: string.Format("Top {0} rank tournament rewards", rank));

                        FireBaseDatabase.Instance.SaveTournamentReceivedReward(AccessToken.CurrentAccessToken.UserId, MasterInfo.Instance.GetPreviousWeekRangeString());
                    }
                }
            });
        }
    }

    private void CheckTimeRemaining()
    {
        if (coroutineTimerSeason != null)
        {
            StopCoroutine(coroutineTimerSeason);
        }

        coroutineTimerSeason = CoroutineTimerSeason();
        StartCoroutine(coroutineTimerSeason);

        DateTime now = MasterInfo.Instance.GetCurrentDateTime();
        int delta = DayOfWeek.Monday - now.DayOfWeek;
        DateTime firstDay = now.AddDays(delta);
        DateTime lastDay = firstDay.AddDays(6);

        textStartDate.text = string.Format("START: {0:00}/{1:00}", firstDay.Day, firstDay.Month);
        textEndDate.text = string.Format("END: {0:00}/{1:00}", lastDay.Day, lastDay.Month);
    }

    private IEnumerator CoroutineTimerSeason()
    {
        TimeSpan t;
        double timeleft = MasterInfo.Instance.GetTournamentTimeleftInSecond();

        while (timeleft > 0)
        {
            yield return StaticValue.waitOneSec;
            timeleft--;

            t = TimeSpan.FromSeconds(timeleft);

            int days = 0;
            int hours = 0;
            int minutes = 0;
            int seconds = 0;

            MasterInfo.Instance.CountDownTimer(t, out days, out hours, out minutes, out seconds);
            //Debug.Log(string.Format("{0}D {1}H:{2}M:{3}S", days, hours, minutes, seconds));
            textRemainingTime.text = string.Format("{0}D {1}H {2}M {3}S", days, hours, minutes, seconds);
        }

        textRemainingTime.text = string.Empty;
    }

    private void CheckNotification()
    {
        // Unclaim rewards
        int number = GetUnclaimRankRewards();
        notiRankReward.SetActive(number > 0);

        // Free entrance

        if (ProfileManager.UserProfile.countPlayTournament < StaticValue.TOURNAMENT_MAX_ENTRANCE
            && GameData.playerResources.tournamentTicket > 0)
        {
            textFreeEntrance.text = GameData.playerResources.tournamentTicket.ToString();
            textFreeEntrance.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            textFreeEntrance.transform.parent.gameObject.SetActive(false);
        }
    }

    private void CreateRankData(List<TournamentData> listData)
    {
        rankData.Clear();
        int rankIndex = 0;

        for (int i = listData.Count - 1; i >= 0; i--)
        {
            TournamentData data = listData[i];
            TournamentRank curRank = GameData.staticTournamentRankData.GetCurrentRank(data.score);
            StaticTournamentRankData staticData = GameData.staticTournamentRankData.GetData((int)curRank);
            CellViewTournamentRankData cellViewData = new CellViewTournamentRankData();
            cellViewData.indexRank = rankIndex;
            //cellViewData.rankName = staticData.rankName;

            if (rankIndex == 0)
                cellViewData.rankName = "GRAND MASTER";
            else if (rankIndex == 1)
                cellViewData.rankName = "MASTER";
            else if (rankIndex == 2)
                cellViewData.rankName = "CHALLENGER";
            else
                cellViewData.rankName = string.Empty;

            cellViewData.rewards = staticData.rewards;
            cellViewData.score = data.score;
            FireBaseDatabase.Instance.GetUserInfo(data.id, cellViewData.SetUserInfo);
            FbController.Instance.GetProfilePictureById(data.id, cellViewData.SetAvatar);
            cellViewData.sprGunId = GameResourcesUtils.GetGunImage(data.primaryGunId);
            cellViewData.sprRankIcon = GameResourcesUtils.GetTournamentRankImage(staticData.rankIndex);

            if (rankIndex < 3)
            {
                if (GameData.tournamentTopRankRewards.ContainsKey(rankIndex))
                    cellViewData.rewards = GameData.tournamentTopRankRewards[rankIndex];
            }

            rankData.Add(cellViewData);
            rankIndex++;
        }
    }

    public int GetUnclaimRankRewards()
    {
        int numberRewards = 0;

        for (int i = 0; i < cellViewRewards.Length; i++)
        {
            if (cellViewRewards[i].IsAvailableClaim())
            {
                numberRewards++;
            }
        }

        return numberRewards;
    }

    public void Challenge()
    {
        SoundManager.Instance.PlaySfxClick();

        if (ProfileManager.UserProfile.countPlayTournament >= StaticValue.TOURNAMENT_MAX_ENTRANCE)
        {
            Popup.Instance.Show("you have exceeded the number of times you can play tournament today.");
        }
        else
        {
            if (GameData.playerResources.gem < priceChallenge)
            {
                Popup.Instance.ShowToastMessage("not enough gems");
            }
            else
            {
                EventDispatcher.Instance.PostEvent(EventID.ClickStartTournament, priceChallenge);
            }
        }
    }

    public void Open(List<TournamentData> data)
    {
        CreateRankData(data);
        scroller.ReloadData();

        FillInformation();
        tipRefreshScoreBoard.SetActive(data.Count > 0);

        gameObject.SetActive(true);
    }

    public void Close()
    {
        popupRank.SetActive(false);
    }

    public void SetPlayerFbAvatar(Sprite spr)
    {
        imgPlayerAvatar.sprite = spr;
    }

    private void OnGetFacebookAvatarDone()
    {
        scroller.RefreshActiveCellViews();
    }

    private void OnGetFacebookNameDone()
    {
        scroller.RefreshActiveCellViews();
    }

    private void FillInformation()
    {
        TournamentRank playerCurRank = GameData.staticTournamentRankData.GetCurrentRank(GameData.playerTournamentData.score);

        textPlayerScore.text = GameData.playerTournamentData.score.ToString();
        imgRankIcon.sprite = GameResourcesUtils.GetTournamentRankImage((int)playerCurRank);
        imgRankIcon.SetNativeSize();

        if (playerCurRank >= TournamentRank.Legend)
        {
            imgRankProgress.fillAmount = 1f;
            textRankProgress.text = GameData.playerTournamentData.score.ToString();
        }
        else
        {
            int nextRankIndex = (int)playerCurRank + 1;
            StaticTournamentRankData curRankData = GameData.staticTournamentRankData.GetData((int)playerCurRank);
            StaticTournamentRankData nextRankData = GameData.staticTournamentRankData.GetData(nextRankIndex);

            float percent = Mathf.Clamp01((float)(GameData.playerTournamentData.score - curRankData.score) / (float)(nextRankData.score - curRankData.score));
            imgRankProgress.fillAmount = percent;
            textRankProgress.text = string.Format("{0}/{1}", GameData.playerTournamentData.score, nextRankData.score);
        }

        //remainingTickets.text = ProfileManager.UserProfile.countFreeTournamentTicket.ToString();
        //medal.text = GameData.playerResources.medal.ToString();

        if (ProfileManager.UserProfile.countPlayTournament >= StaticValue.TOURNAMENT_MAX_ENTRANCE)
        {
            groupGem.SetActive(false);
            groupTicket.SetActive(false);
            textFreeEntrance.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            if (GameData.playerResources.tournamentTicket > 0)
            {
                textFreeEntrance.transform.parent.gameObject.SetActive(true);
                textFreeEntrance.text = GameData.playerResources.tournamentTicket.ToString();
                groupGem.SetActive(false);
                groupTicket.SetActive(true);
                priceChallenge = 0;
            }
            else
            {
                textFreeEntrance.transform.parent.gameObject.SetActive(false);

                if (ProfileManager.UserProfile.countPlayTournament >= StaticValue.TOURNAMENT_FREE_ENTRANCE)
                {
                    groupGem.SetActive(true);
                    groupTicket.SetActive(false);

                    if (ProfileManager.UserProfile.countPlayTournament == 2)
                        priceChallenge = StaticValue.COST_ENTRANCE_TOURNAMENT_3RD;
                    else if (ProfileManager.UserProfile.countPlayTournament == 3)
                        priceChallenge = StaticValue.COST_ENTRANCE_TOURNAMENT_4TH;
                    else if (ProfileManager.UserProfile.countPlayTournament >= 4)
                        priceChallenge = StaticValue.COST_ENTRANCE_TOURNAMENT_5TH;

                    textGemChallenge.color = GameData.playerResources.gem >= priceChallenge ? Color.white : StaticValue.colorNotEnoughMoney;
                    textGemChallenge.text = priceChallenge.ToString();
                }
                else
                {
                    GameData.playerResources.ReceiveTournamentTicket(StaticValue.TOURNAMENT_FREE_ENTRANCE);
                    groupGem.SetActive(false);
                    groupTicket.SetActive(true);
                    priceChallenge = 0;
                    FillInformation();
                }
            }
        }
    }


    #region EnhancedScroller Handlers

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return rankData.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        if (dataIndex < 4)
        {
            return 100f;
        }
        else
        {
            return 61f;
        }
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        CellViewTournamentRank cellView = scroller.GetCellView(cellViewTournamentRank) as CellViewTournamentRank;
        //cellView.name = rankData[dataIndex].type.ToString();
        cellView.SetData(rankData[dataIndex]);
        return cellView;
    }

    #endregion
}
