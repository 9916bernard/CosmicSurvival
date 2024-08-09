using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealPickUp : MonoBehaviour
{
    private BattleManager battleManager = null;
    [HideInInspector] public ObjectPoolSimple<HealPickUp> _pool = null;
    private int healValue = 1; // Value of the experience orb

    public void Init(BattleManager manager)
    {
        battleManager = manager;
        gameObject.SetActive(true);
        healValue = 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            battleManager.GainHealth(healValue);
            _pool.Push(this);
        }
    }

    public void SetPool(ObjectPoolSimple<HealPickUp> pool)
    {
        _pool = pool;
    }

    public static HealPickUp MakeFactory(ObjectPoolSimple<HealPickUp> Pool)
    {
        var _hp = Instantiate(Resources.Load("Battle/Pickup/HealPickUp")).GetComponent<HealPickUp>();
        _hp.SetPool(Pool);
        return _hp;
    }
}
