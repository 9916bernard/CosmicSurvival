using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public partial class BattleField : MonoBehaviour
{
    public void Damage_Spawn(Vector3 InPos, BattleDamage InDamage)
    {
        _DamagePool.Pop(0).Play(InPos, InDamage);
    }
}
