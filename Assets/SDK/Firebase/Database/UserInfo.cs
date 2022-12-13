using Newtonsoft.Json;
using System.Collections.Generic;

public class UserInfo
{
    public string id;
    public string authId;
    public string name;
    public string email;

    public UserInfo() { }

    public UserInfo(string id, string authId, string name, string email)
    {
        this.id = id;
        this.authId = authId;
        this.name = name;
        this.email = email;
    }

    public override string ToString()
    {
        return string.Format("ID: {0} | AuthId: {1} | Name: {2} | Email: {3}", id, authId, name, email);
    }

    public string GetJsonString()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("authId", authId);
        data.Add("name", name);
        data.Add("email", email);

        return JsonConvert.SerializeObject(data);
    }
}
