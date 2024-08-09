using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackAstroid : Astroid
{
    private int BlackReward = 2;

    public override void OnDestroyed()
    {

        battleManager.Black += BlackReward;

        base.OnDestroyed();
    }
}