using System.Collections;
using UnityEngine;

public class MinerDroneTurret : Turret
{
    private float floatAmplitude = 0.04f; // Amplitude of the floating effect
    private float floatFrequency = 3f; // Frequency of the floating effect
    private Vector3 originalPosition; // The original position of the drone
    private Vector3 initialScale; // The initial scale of the drone
    private Astroid targetAstroid; // The current target astroid
    private Base baseStation; // The base station

    // Start is called before the first frame update

    override public void Init(UTBEquipment_Record data, Base baseStation)
    {
        base.Init(data, baseStation);
        this.baseStation = baseStation;
        originalPosition = transform.position;
        initialScale = transform.localScale;
        StartCoroutine(MineAstroids());
    }

    // Update is called once per frame
    void Update()
    {
        ApplyFloatingEffect();
    }

    // Coroutine to handle mining astroids
    private IEnumerator MineAstroids()
    {
        while (true)
        {
            FindNearestAstroid();
            if (targetAstroid != null)
            {
                yield return MoveToTarget(targetAstroid.transform.position);
                if(_stat.InActiveDuration > 0)
                {
                    yield return MoveToTarget(originalPosition);
                    yield return new WaitForSeconds(_stat.InActiveDuration);
                }
               
                
            }

            yield return null;
        }
    }

    // Find the nearest astroid
    private void FindNearestAstroid()
    {
        Astroid[] astroids = FindObjectsOfType<Astroid>();
        float shortestDistance = Mathf.Infinity;
        Astroid nearestAstroid = null;

        foreach (Astroid astroid in astroids)
        {
            float distance = Vector3.Distance(transform.position, astroid.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestAstroid = astroid;
            }
        }

        targetAstroid = nearestAstroid;
    }

    // Move to the target position
    private IEnumerator MoveToTarget(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _stat.ProjectileSpeed * Time.deltaTime);
            yield return null;
        }
    }

    // OnTriggerEnter2D to detect collision with astroid
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Astroid>() != null)
        {
            Astroid astroid = collision.gameObject.GetComponent<Astroid>();
            astroid.OnDestroyed();
        }
    }

    // Apply floating effect to the drone
    private void ApplyFloatingEffect()
    {
        float scaleChange = floatAmplitude * Mathf.Sin(Time.time * floatFrequency);
        transform.localScale = initialScale + new Vector3(scaleChange, scaleChange, 0);
    }
}


