using System.Collections;
using UnityEngine;

public class BlackHolePickUp : MonoBehaviour
{


    private void Awake()
    {
        // Spin the black hole
        this.transform.Rotate(Vector3.forward, 360 * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().battleManager.GainBlackHole(this);
        }
    }



}
