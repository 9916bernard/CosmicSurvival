using UnityEngine;

public class UserJson_Account
{
    public string AccountName;
    public int Exp;

    public UserJson_Account()
    {
        AccountName = "guest";
        Exp = 0;
    }
}

public class UserData_Account : UserDataBase<UserJson_Account>
{
    public UserData_Account()
    {
        _userDataName = "Account";
    }
};
