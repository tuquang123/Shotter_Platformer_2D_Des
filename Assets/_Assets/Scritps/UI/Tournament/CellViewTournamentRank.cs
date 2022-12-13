using UnityEngine;
using EnhancedUI.EnhancedScroller;
using UnityEngine.UI;

public class CellViewTournamentRank : EnhancedScrollerCellView
{
    public Image imgRankIconTop;
    public Image imgRankIconNormal;
    public Image imgAvatar;
    public Text textRankName;
    public Image imgGun;
    public Text textScore;
    public Text textRankIndex;
    public Text textPlayerName;

    public Image header;
    public Sprite sprHeaderTop1;
    public Sprite sprHeaderTop2;
    public Sprite sprHeaderTop3;

    public Image bgInfo;
    public Sprite bgInfoTop1;
    public Sprite bgInfoTop2;
    public Sprite bgInfoTop3;
    public Sprite bgInfoNormal;

    public Sprite loadingSpr;

    public TournamentRankRewardDisplay[] rewardCells;

    private CellViewTournamentRankData _data;


    public void SetData(CellViewTournamentRankData data)
    {
        _data = data;

        UpdateInformation();
    }

    private void UpdateInformation()
    {
        imgRankIconTop.sprite = _data.sprRankIcon;
        imgRankIconTop.SetNativeSize();
        textRankName.text = _data.rankName;

        // Rank Number
        textRankIndex.text = (_data.indexRank + 1).ToString();

        // Player Name
        textPlayerName.text = string.IsNullOrEmpty(_data.playerName) ? "Player Unknown" : _data.playerName;

        if (_data.indexRank < 3)
        {
            // Header
            header.gameObject.SetActive(true);
            if (_data.indexRank == 0)
            {
                header.sprite = sprHeaderTop1;
                bgInfo.sprite = bgInfoTop1;
            }
            else if (_data.indexRank == 1)
            {
                header.sprite = sprHeaderTop2;
                bgInfo.sprite = bgInfoTop2;
            }
            else if (_data.indexRank == 2)
            {
                header.sprite = sprHeaderTop3;
                bgInfo.sprite = bgInfoTop3;
            }

            // BG info

            SetRewards();

            imgRankIconNormal.gameObject.SetActive(false);
        }
        else
        {
            header.gameObject.SetActive(false);
            bgInfo.sprite = bgInfoNormal;
            imgRankIconNormal.gameObject.SetActive(true);
            imgRankIconNormal.sprite = _data.sprRankIcon;
            imgRankIconNormal.SetNativeSize();
        }

        imgGun.sprite = _data.sprGunId;
        imgGun.SetNativeSize();
        imgAvatar.sprite = _data.sprAvatar == null ? loadingSpr : _data.sprAvatar;
        textScore.text = _data.score.ToString();
    }

    public override void RefreshCellView()
    {
        base.RefreshCellView();

        UpdateInformation();
    }

    private void SetRewards()
    {
        if (_data.indexRank < 3)
        {
            for (int i = 0; i < rewardCells.Length; i++)
            {
                TournamentRankRewardDisplay cell = rewardCells[i];

                if (i < _data.rewards.Count)
                {
                    cell.gameObject.SetActive(true);
                    cell.SetInformation(_data.rewards[i]);
                }
                else
                {
                    cell.gameObject.SetActive(false);
                }
            }
        }
    }
}
