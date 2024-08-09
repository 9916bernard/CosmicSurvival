using UnityEngine;

public class GoldAstroid : Astroid
{
     private int goldReward = 200;

    public override void OnDestroyed()
    {
        // Give gold to the player
        //USER.fund.AddGold(goldReward);
        //USER.Inst().SaveData();
        battleManager.gold += goldReward;

        // Call the base class OnDestroyed to handle deactivation
        base.OnDestroyed();
    }
}
