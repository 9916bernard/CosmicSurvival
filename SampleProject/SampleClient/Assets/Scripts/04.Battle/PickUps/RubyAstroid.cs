using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyAstroid : Astroid
{
    private int RubyReward = 2;

    public override void OnDestroyed()
    {

        battleManager.Ruby += RubyReward;
 
        base.OnDestroyed();
    }
}
