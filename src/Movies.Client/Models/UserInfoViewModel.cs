namespace Movies.Client.Models;

public class UserInfoViewModel
{
    public Dictionary<string, string> UserInfoDict { get; private set; } = null;

    public UserInfoViewModel(Dictionary<string, string> userInfoDict)
    {
        UserInfoDict = userInfoDict;
    }



}
