using System.Collections.Generic;
using UnityEngine;


public class UserJson_Upgrade
{
    public Dictionary<int, float> _Dic;

    public UserJson_Upgrade()
    {
        _Dic = new();
    }
}

public class UserData_Upgrade : UserDataBase<UserJson_Upgrade>
{
    public UserData_Upgrade()
    {
       _userDataName = "Upgrade";
    }

    public float GetUpgradeEffectLevel(ETB_UPGRADE_EFFECT effect)
    {
        _data._Dic.TryGetValue((int)effect, out float value);
        return value;
    }

    public void SetUpgradeEffectLevel(ETB_UPGRADE_EFFECT effect, float value)
    {
        _data._Dic[(int)effect] = value;
    }

    public void Upgrade(ETB_UPGRADE_EFFECT effect)
    {
        if (_data._Dic.ContainsKey((int)effect) == false)
        {
            _data._Dic.Add((int)effect, 1);
        }
        else
        {
            _data._Dic[(int)effect] += 1;
        }
    }


};
