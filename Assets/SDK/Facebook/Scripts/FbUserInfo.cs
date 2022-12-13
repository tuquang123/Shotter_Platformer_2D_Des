public class FbUserInfo
{
    public string Id;
    public string Name;
    public string Email;
    public string ProfileImageURL;

    public FbUserInfo() { }

    public FbUserInfo(string id, string name, string email, string profileImageURL)
    {
        this.Id = id;
        this.Name = name;
        this.Email = email;
        this.ProfileImageURL = profileImageURL;
    }

    public override string ToString()
    {
        return string.Format("ID: {0}\nName: {1}\nEmail: {2}\nScore: {3}", Id, Name, Email);
    }
}