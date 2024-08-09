using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmeraldAstroid : Astroid
{
    private int EmeraldReward = 2;

    public override void OnDestroyed()
    {

        battleManager.Emerald += EmeraldReward;

        base.OnDestroyed();
    }
}