using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CellViewRankReward : MonoBehaviour
{
    public TournamentRank rankType;
    public Image rankIcon;
    public Text rankName;
    public Text rankPoint;
    public Button btnClaim;
    public GameObject labelAchieved;
    public Image background;
    public Sprite[] bgSprites;
    public RewardElement[] rewardCells;

    private List<RewardData> rewards;

    public void Load()
    {
        StaticTournamentRankData staticData = GameData.staticTournamentRankData.GetData((int)rankType);
        TournamentRank playerCurRank = GameData.staticTournamentRankData.GetCurrentRank(GameData.playerTournamentData.score);

        background.sprite = bgSprites[(int)rankType];
        rankIcon.sprite = GameResourcesUtils.GetTournamentRankImage((int)rankType);
        rankIcon.SetNativeSize();
        rankName.text = rankType.ToString().ToUpper();
        rankPoint.text = staticData.score > 0 ? string.Format("Score: {0}", staticData.score) : string.Empty;

        rewards = staticData.rewards;

        for (int i = 0; i < rewardCells.Length; i++)
        {
            RewardElement cell = rewardCells[i];

            cell.gameObject.SetActive(i < staticData.rewards.Count);

            if (i < staticData.rewards.Count)
            {
                RewardData rw = staticData.rewards[i];
                cell.SetInformation(rw);
            }
        }

        btnClaim.gameObject.SetActive((int)playerCurRank >= (int)rankType);

        switch (rankType)
        {
            case TournamentRank.Ducky:
                labelAchieved.SetActive(false);
                btnClaim.gameObject.SetActive(false);
                break;

            case TournamentRank.Bronze:
                labelAchieved.SetActive(ProfileManager.UserProfile.isClaimedRank1);
                if (ProfileManager.UserProfile.isClaimedRank1)
                    btnClaim.gameObject.SetActive(false);
                break;

            case TournamentRank.Silver:
                labelAchieved.SetActive(ProfileManager.UserProfile.isClaimedRank2);
                if (ProfileManager.UserProfile.isClaimedRank2)
                    btnClaim.gameObject.SetActive(false);
                break;

            case TournamentRank.Gold:
                labelAchieved.SetActive(ProfileManager.UserProfile.isClaimedRank3);
                if (ProfileManager.UserProfile.isClaimedRank3)
                    btnClaim.gameObject.SetActive(false);
                break;

            case TournamentRank.Platinum:
                labelAchieved.SetActive(ProfileManager.UserProfile.isClaimedRank4);
                if (ProfileManager.UserProfile.isClaimedRank4)
                    btnClaim.gameObject.SetActive(false);
                break;

            case TournamentRank.Diamond:
                labelAchieved.SetActive(ProfileManager.UserProfile.isClaimedRank5);
                if (ProfileManager.UserProfile.isClaimedRank5)
                    btnClaim.gameObject.SetActive(false);
                break;

            case TournamentRank.Legend:
                labelAchieved.SetActive(ProfileManager.UserProfile.isClaimedRank6);
                if (ProfileManager.UserProfile.isClaimedRank6)
                    btnClaim.gameObject.SetActive(false);
                break;
        }

        if (playerCurRank == rankType)
        {
            background.rectTransform.localScale = new Vector3(1.05f, 1f, 1f);
        }
        else
        {
            background.rectTransform.localScale = Vector3.one;
        }
    }

    public bool IsAvailableClaim()
    {
        bool canClaim = false;
        TournamentRank playerRank = GameData.staticTournamentRankData.GetCurrentRank(GameData.playerTournamentData.score);

        if (playerRank >= rankType)
        {
            switch (rankType)
            {
                case TournamentRank.Bronze:
                    return !ProfileManager.UserProfile.isClaimedRank1;

                case TournamentRank.Silver:
                    return !ProfileManager.UserProfile.isClaimedRank2;

                case TournamentRank.Gold:
                    return !ProfileManager.UserProfile.isClaimedRank3;

                case TournamentRank.Platinum:
                    return !ProfileManager.UserProfile.isClaimedRank4;

                case TournamentRank.Diamond:
                    return !ProfileManager.UserProfile.isClaimedRank5;

                case TournamentRank.Legend:
                    return !ProfileManager.UserProfile.isClaimedRank6;
            }
        }

        return canClaim;
    }

    public void Claim()
    {
        if (rewards != null)
        {
            RewardUtils.Receive(rewards);
            Popup.Instance.ShowReward(rewards);
        }

        btnClaim.gameObject.SetActive(false);
        labelAchieved.gameObject.SetActive(true);

        switch (rankType)
        {
            case TournamentRank.Ducky:
                break;

            case TournamentRank.Bronze:
                ProfileManager.UserProfile.isClaimedRank1.Set(true);
                break;

            case TournamentRank.Silver:
                ProfileManager.UserProfile.isClaimedRank2.Set(true);
                break;

            case TournamentRank.Gold:
                ProfileManager.UserProfile.isClaimedRank3.Set(true);
                break;

            case TournamentRank.Platinum:
                ProfileManager.UserProfile.isClaimedRank4.Set(true);
                break;

            case TournamentRank.Diamond:
                ProfileManager.UserProfile.isClaimedRank5.Set(true);
                break;

            case TournamentRank.Legend:
                ProfileManager.UserProfile.isClaimedRank6.Set(true);
                break;
        }

        EventDispatcher.Instance.PostEvent(EventID.ClaimTournamentRankReward, rankType);

        FirebaseAnalyticsHelper.LogEvent("N_ClaimRankReward", rankType.ToString());
    }
}
