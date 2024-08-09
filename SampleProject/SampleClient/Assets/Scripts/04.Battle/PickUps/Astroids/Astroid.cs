using System.Collections;
using UnityEngine;

public class Astroid : MonoBehaviour
{
    public float health = 50f;
    private float maxHealth;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Vector3 startPosition;
    private AstroidSpawner spawner;

    private float verticalAmplitude = 0.1f;
    private float verticalFrequency = 0.5f;
    private float horizontalAmplitude = 0.1f;
    private float horizontalFrequency = 0.5f;

    [HideInInspector] public ObjectPool<int, Astroid> _pool = null;
    protected BattleManager battleManager;

    public virtual void Init(BattleManager battleManager)
    {
        this.battleManager = battleManager;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        maxHealth = health;
        startPosition = transform.position;
    }

    protected virtual void Update()
    {
        float yOffset = Mathf.Sin(Time.unscaledTime * verticalFrequency) * verticalAmplitude;
        float xOffset = Mathf.Cos(Time.unscaledTime * horizontalFrequency) * horizontalAmplitude;
        transform.position = startPosition + new Vector3(xOffset, yOffset, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TakeDamage(0.3f);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TakeDamage(0.3f);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            OnDestroyed();
        }
        else
        {
            StartCoroutine(BlinkEffect());
            UpdateAlphaBasedOnHealth();
        }
    }

    private IEnumerator BlinkEffect()
    {
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }

    private void UpdateAlphaBasedOnHealth()
    {
        Color color = spriteRenderer.color;
        color.a = health / maxHealth;
        spriteRenderer.color = color;
    }

    public virtual void OnDestroyed()
    {
        gameObject.SetActive(false);
        if (_pool != null)
        {
            _pool.Push(this);
        }
    }

    public void SetPool(ObjectPool<int, Astroid> pool)
    {
        _pool = pool;
    }

    public static Astroid MakeFactory(int key, ObjectPool<int, Astroid> pool)
    {
        GameObject astroidPrefab;
        switch (key)
        {
            case 1:
                astroidPrefab = Resources.Load<GameObject>("Battle/Astroid/GoldAstroid");
                break;
            case 2:
                astroidPrefab = Resources.Load<GameObject>("Battle/Astroid/PearlAstroid");
                break;
            case 3:
                astroidPrefab = Resources.Load<GameObject>("Battle/Astroid/RubyAstroid");
                break;
            case 4:
                astroidPrefab = Resources.Load<GameObject>("Battle/Astroid/SappireAstroid");
                break;
            case 5:
                astroidPrefab = Resources.Load<GameObject>("Battle/Astroid/AmberAstroid");
                break;
            case 6:
                astroidPrefab = Resources.Load<GameObject>("Battle/Astroid/EmeraldAstroid");
                break;
            case 7:
                astroidPrefab = Resources.Load<GameObject>("Battle/Astroid/BlackAstroid");
                break;
            default:
                Debug.LogError("Invalid asteroid type.");
                return null;
        }

        var astroid = Instantiate(astroidPrefab).GetComponent<Astroid>();
        if (astroid != null)
        {
            astroid.SetPool(pool);
        }
        else
        {
            Debug.LogError("Astroid instantiation failed.");
        }

        return astroid;
    }

    public void SetSpawner(AstroidSpawner spawner)
    {
        this.spawner = spawner;
    }
}
