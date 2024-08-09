using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SappireAstroid : Astroid
{
    private int SapphireReward = 2;

    public override void OnDestroyed()
    {

        battleManager.Sapphire += SapphireReward;

        base.OnDestroyed();
    }
}