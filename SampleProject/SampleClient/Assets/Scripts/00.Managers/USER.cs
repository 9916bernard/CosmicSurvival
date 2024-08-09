
using System.Collections.Generic;

public class USER : Singleton<USER>
{
    private readonly UserData_Setting _Setting = new();
    static public UserData_Setting setting = null;

    private readonly UserData_Account _Account = new();
    static public UserData_Account account = null;

    private readonly UserData_Upgrade _Upgrade = new();
    static public UserData_Upgrade upgrade = null;

    private readonly UserData_Fund _Fund = new();
    static public UserData_Fund fund = null;

    private readonly UserData_Player _Player = new();
    static public UserData_Player player = null;

    private readonly List<UserDataBaseRoot> _DataList = new();

    public override void Init()
    {
        _DataList.Add(_Setting);
        _DataList.Add(_Account);
        _DataList.Add(_Fund);
        _DataList.Add(_Player);
        _DataList.Add(_Upgrade);

        LoadData();

        RestoreStaticModule();
    }

    public override void RestoreStaticModule()
    {
        base.RestoreStaticModule();

        setting = _Setting;
        account = _Account;
        fund = _Fund;
        player = _Player;
        upgrade = _Upgrade;
    }

    public void LoadData()
    {
        for (int i = 0; i < _DataList.Count; i++)
        {
            _DataList[i].Load();
        }
    }

    public void SaveData()
    {
        for (int i = 0; i < _DataList.Count; i++)
        {
            _DataList[i].Save();
        }
    }

    public void MakeDefaultData()
    {
        for (int i = 0; i < _DataList.Count; i++)
        {
            _DataList[i].MakeDefaultData();
        }
    }
};
