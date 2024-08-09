using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PearlAstroid : Astroid
{
    private int pearlReward = 2;

    public override void OnDestroyed()
    {
        // Give gold to the player
        //USER.fund.AddGold(goldReward);
        //USER.Inst().SaveData();
        battleManager.pearl += pearlReward;
        // Call the base class OnDestroyed to handle deactivation
        base.OnDestroyed();
    }
}
