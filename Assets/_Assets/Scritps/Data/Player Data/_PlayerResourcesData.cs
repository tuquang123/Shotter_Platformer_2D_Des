using UnityEngine;
using System.Collections;
using Newtonsoft.Json;

public class _PlayerResourcesData
{
    public int coin;
    public int gem;
    public int stamina;
    public int medal;
    public int tournamentTicket;

    public void Save()
    {
        string s = JsonConvert.SerializeObject(this);

        ProfileManager.UserProfile.playerResourcesData.Set(s);
        ProfileManager.SaveAll();

        DebugCustom.Log("PlayerResourcesData=" + s);
    }


    #region Coin

    public void ReceiveCoin(int value)
    {
        if (value > 0)
        {
            coin += value;
            Save();

            EventDispatcher.Instance.PostEvent(EventID.ReceiveCoin, value);
        }
    }

    public void ConsumeCoin(int value)
    {
        if (value > 0)
        {
            coin -= value;

            if (coin < 0)
                coin = 0;

            Save();

            EventDispatcher.Instance.PostEvent(EventID.ConsumeCoin, value);
        }
    }

    #endregion

    #region Gem

    public void ReceiveGem(int value)
    {
        if (value > 0)
        {
            gem += value;
            Save();

            EventDispatcher.Instance.PostEvent(EventID.ReceiveGem, value);
        }
    }

    public void ConsumeGem(int value)
    {
        if (value > 0)
        {
            gem -= value;

            if (gem < 0)
                gem = 0;

            Save();

            EventDispatcher.Instance.PostEvent(EventID.ConsumeGem, value);
        }
    }

    #endregion

    #region Stamina

    public void ReceiveStamina(int value)
    {
        if (value > 0)
        {
            stamina += value;
            Save();

            EventDispatcher.Instance.PostEvent(EventID.ReceiveStamina, value);
        }
    }

    public void ConsumeStamina(int value)
    {
        if (value > 0)
        {
            stamina -= value;

            if (stamina < 0)
                stamina = 0;

            Save();

            EventDispatcher.Instance.PostEvent(EventID.ConsumeStamina, value);
        }
    }

    #endregion

    #region Medal
    public void ReceiveMedal(int value)
    {
        if (value > 0)
        {
            medal += value;
            Save();

            EventDispatcher.Instance.PostEvent(EventID.ReceiveMedal, value);
        }
    }

    public void ConsumeMedal(int value)
    {
        if (value > 0)
        {
            medal -= value;

            if (medal < 0)
                medal = 0;

            Save();

            EventDispatcher.Instance.PostEvent(EventID.ConsumeMedal, value);
        }
    }

    #endregion

    #region Tournament ticket
    public void ReceiveTournamentTicket(int value)
    {
        if (value > 0)
        {
            tournamentTicket += value;
            Save();
        }
    }

    public void ConsumeTournamentTicket(int value)
    {
        if (value > 0)
        {
            tournamentTicket -= value;

            if (tournamentTicket < 0)
                tournamentTicket = 0;

            Save();
        }
    }

    public void ResetTicketNewday()
    {
        if (tournamentTicket < StaticValue.TOURNAMENT_FREE_ENTRANCE)
        {
            tournamentTicket = StaticValue.TOURNAMENT_FREE_ENTRANCE;
        }

        Save();
    }

    #endregion
}
