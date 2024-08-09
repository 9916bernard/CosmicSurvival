using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Metal : MonoBehaviour
{
    [HideInInspector] public ObjectPoolSimple<Metal> _pool = null;
    private int MetalValue = 1; // Value of the experience orb
    private BattleManager battleManager = null;

    public void Init(BattleManager manager)
    {
        battleManager = manager;
        gameObject.SetActive(true);
        MetalValue = 1;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                battleManager.GainMetal(MetalValue);
                _pool.Push(this);
            }
        }
    }

    public void SetPool(ObjectPoolSimple<Metal> pool)
    {
        _pool = pool;
    }

    public static Metal MakeFactory(ObjectPoolSimple<Metal> Pool)
    {
        var _Metal = Instantiate(Resources.Load("Battle/Pickup/Metal")).GetComponent<Metal>();
        _Metal.SetPool(Pool);
        return _Metal;
    }
}
