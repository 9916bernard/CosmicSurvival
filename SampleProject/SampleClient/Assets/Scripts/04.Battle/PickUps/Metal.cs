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
                RemoveFromQueue(battleManager.metalSpawner.spawnedMetals, this);
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

    public void RemoveFromQueue<T>(Queue<T> queue, T elementToRemove)
    {
        int initialCount = queue.Count;

        for (int i = 0; i < initialCount; i++)
        {
            T currentElement = queue.Dequeue();

            // Skip the element to remove
            if (!EqualityComparer<T>.Default.Equals(currentElement, elementToRemove))
            {
                queue.Enqueue(currentElement);  // Add it back to the queue if it doesn't match
            }
        }
    }
}
