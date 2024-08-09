using System.Collections.Generic;
using UnityEngine;

public class UserJson_Player
{
    public int ID = 0;
    public float _Record;
    public Dictionary<string, bool> TutorialCompletion;

    public UserJson_Player()
    {
        ID = 0;
        _Record = 0;
        TutorialCompletion = new Dictionary<string, bool>
        {
            { "BATTLE_START_MODE_MAIN", false },
            { "BATTLE_DROP_EXP", false },
            { "BATTLE_DROP_METAL", false },
            { "BATTLE_UPGRADE_UNIT", false },
            { "BATTLE_UPGRADE_BASE", false },
            { "BATTLE_DIE", false },
            { "BATTLE_MOVE_FAR", false },
            { "BATTLE_TIME_PASS", false },
            { "LOBBY_UPGRADE", false },
            { "LOBBY_RANKING", false }
        };
    }
}

public class UserData_Player : UserDataBase<UserJson_Player>
{
    public UserData_Player()
    {
        _userDataName = "Player";
    }

    public override void MakeDefaultData()
    {
    }

    public void SetRecord(float record)
    {
        _data._Record = record;
    }

    public float GetRecord()
    {
        return _data._Record;
    }

    public void SetTutorialCompleted(string tutorialKey)
    {
        if (_data.TutorialCompletion.ContainsKey(tutorialKey))
        {
            _data.TutorialCompletion[tutorialKey] = true;
        }
        else
        {
            Debug.LogWarning($"Tutorial key {tutorialKey} not found.");
        }
    }

    public bool IsTutorialCompleted(string tutorialKey)
    {
        if (_data.TutorialCompletion.ContainsKey(tutorialKey))
        {
            return _data.TutorialCompletion[tutorialKey];
        }
        else
        {
            Debug.LogWarning($"Tutorial key {tutorialKey} not found.");
            return false;
        }
    }

    public void ResetTutorials()
    {
        var keys = new List<string>(_data.TutorialCompletion.Keys);
        foreach (var key in keys)
        {
            _data.TutorialCompletion[key] = false;
        }
    }
}
