using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmberAstoid : Astroid
{
    [SerializeField] private int AmberReward = 5;

    public override void OnDestroyed()
    {

        battleManager.Amber += AmberReward;

        base.OnDestroyed();
    }
}
