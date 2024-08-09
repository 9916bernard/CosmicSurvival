using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Exp : MonoBehaviour
{
    private BattleManager battleManager = null;
    [HideInInspector] public ObjectPoolSimple<Exp> _pool = null;
    private int expValue = 1; // Value of the experience orb

    public void Init(BattleManager manager)
    {
        battleManager = manager;
        gameObject.SetActive(true);
        expValue = 1;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            battleManager.GainExperience(expValue);
            _pool.Push(this);
        }
    }

    public void SetPool(ObjectPoolSimple<Exp> pool)
    {
        _pool = pool;
    }

    public static Exp MakeFactory(ObjectPoolSimple<Exp> Pool)
    {
        var _Exp = Instantiate(Resources.Load("Battle/Pickup/exp")).GetComponent<Exp>();
        _Exp.SetPool(Pool);
        return _Exp;
    }
}
