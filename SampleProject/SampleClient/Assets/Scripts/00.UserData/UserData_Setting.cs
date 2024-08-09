
public class UserJson_Setting
{
    public EUI_LanguageType LanguageType;

    public UserJson_Setting()
    {
        LanguageType = EUI_LanguageType.KOREAN;
    }
}

public class UserData_Setting : UserDataBase<UserJson_Setting>
{
    public UserData_Setting()
    {
        _userDataName = "Setting";
    }
};
