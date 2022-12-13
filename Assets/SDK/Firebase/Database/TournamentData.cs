using Newtonsoft.Json;
using System.Collections.Generic;

public class TournamentData
{
    public string id;
    public int score;
    public int primaryGunId;
    public bool received;

    public TournamentData() { }

    public TournamentData(string id, int score, int primaryGunId, bool received)
    {
        this.id = id;
        this.score = score;
        this.primaryGunId = primaryGunId;
        this.received = received;
    }

    public override string ToString()
    {
        return string.Format("ID: {0} | Primary Gun: {1} | Score: {2} | Received reward: {3}", id, primaryGunId, score, received);
    }

    public string GetJsonString()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("score", score);
        data.Add("primaryGunId", primaryGunId);
        data.Add("received", received);

        return JsonConvert.SerializeObject(data);
    }
}
