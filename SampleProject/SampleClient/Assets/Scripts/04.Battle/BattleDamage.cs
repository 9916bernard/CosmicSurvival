using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public partial class BattleDamage
{
    public long Damage = 0;
    public long CounterDamage = 0;

    public bool WillDead = false;

    public BattleDamage()
    {
    }

    public BattleDamage(long InDamage, long InCounterDamage, bool InWillDead)
    {
        Damage = InDamage;
        CounterDamage = InCounterDamage;
        WillDead = InWillDead;
    }
}
