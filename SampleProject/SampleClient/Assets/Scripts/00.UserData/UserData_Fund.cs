using Newtonsoft.Json.Bson;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class UserJson_Fund
{
    public long _Gold;
    public int _Pearl;
    public int _Ruby;
    public int _Amber;
    public int _Sapphire;
    public int _Emerald;
    public int _Black;
    public int _Diamond;
    public Dictionary<int, long> _Dic;

    public UserJson_Fund()
    {
        _Gold = 0;
        _Pearl = 0;
        _Ruby = 0;
        _Amber = 0;
        _Sapphire = 0;
        _Diamond = 0;
        _Emerald = 0;
        _Black = 0;
        _Dic = new();
    }
}

public class UserData_Fund : UserDataBase<UserJson_Fund>
{
    //private long _PlayPoint = 0;
    //public long PlayPoint { get { return _PlayPoint; } set { _PlayPoint = value; } }

    //private long _Diamond = 0;
    //public long Diamond { get { return _Diamond; } set { _Diamond = value; } }

    //private long _Gold = 0;
    //public long Gold { get { return _Gold; } set { _Gold = value; } }

    public UserData_Fund()
    {
        _userDataName = "Fund";

        //_data._Dic.Add(ETB_FUND.GOLD, 100);
    }

    public void AddGold(long gold)
    {
        _data._Gold += gold;

        AddFund(ETB_FUND.GOLD, gold);
    }

    public void AddDiamond(int diamond)
    {
        _data._Diamond += diamond;

        AddFund(ETB_FUND.DIAMOND, diamond);
    }
    public void AddRuby(int ruby)
    {
        _data._Ruby += ruby;

        AddFund(ETB_FUND.MINERAL_RUBY, ruby);
    }

    public void AddAmber(int amber)
    {
        _data._Amber += amber;

        AddFund(ETB_FUND.MINERAL_AMBER, amber);
    }

    public void AddSapphire(int sapphire)
    {
        _data._Sapphire += sapphire;

        AddFund(ETB_FUND.MINERAL_SAPPAIRE, sapphire);
    }

    public void AddPearl(int pearl)
    {
        _data._Pearl += pearl;

        AddFund(ETB_FUND.MINERAL_PEARL, pearl);
    }

    public void AddBlack(int black)
    {
        _data._Black += black;

        AddFund(ETB_FUND.MINERAL_BLACK, black);
    }

    public void AddEmerald(int emerald)
    {
        _data._Emerald += emerald;

        AddFund(ETB_FUND.MINERAL_EMERALD, emerald);
    }



    public void AddFund(ETB_FUND fund, long amount)
    {
        if (_data._Dic.ContainsKey((int)fund) == false)
        {
            _data._Dic.Add((int)fund, amount);
        }
        else
        {
            _data._Dic[(int)fund] += amount;
            
        }
    }

    public void SetFund(ETB_FUND fund, long amount)
    {
        if (_data._Dic.ContainsKey((int)fund) == false)
        {
            _data._Dic.Add((int)fund, amount);
        }
        else
        {
            _data._Dic[(int)fund] = amount;
        }
    }

    public long GetFund(ETB_FUND fund)
    {
        _data._Dic.TryGetValue((int)fund, out long amount);

        return amount;
    }


};
