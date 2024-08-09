using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    private int damage;
    private float speed;
    private float duration = 1f; // Duration the shockwave lasts
    private float rotationSpeed;
    private float elapsedTime = 0f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Transform visualTransform;

    [HideInInspector] public ObjectPoolSimple<ShockWave> _pool = null;
    private Coroutine spinCoroutine;

    public void Init(int damage, float speed, float rotationSpeed, float duration, Vector3 direction)
    {
        this.damage = damage;
        this.speed = speed;
        this.rotationSpeed = rotationSpeed;
        this.duration = duration;
        elapsedTime = 0f;
        spriteRenderer.color = originalColor;

        visualTransform.localScale = Vector3.one;

        // Set the direction the shockwave should move
        transform.up = direction;

        gameObject.SetActive(true);
    }

    void Awake()
    {
        visualTransform = transform.GetChild(0); // Assuming the visual is the first child
        spriteRenderer = visualTransform.GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    

    void Update()
    {
        visualTransform.Rotate(0, 0, 250 * Time.deltaTime);

        elapsedTime += Time.deltaTime;

        // Move forward
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        // Fade out over time
        float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
        Color newColor = spriteRenderer.color;
        newColor.a = alpha;
        spriteRenderer.color = newColor;

        // Scale up over time
        float scale = Mathf.Lerp(1f, 4f, elapsedTime / duration);
        visualTransform.localScale = new Vector3(scale, scale, 1f);

        if (elapsedTime >= duration)
        {
            if (_pool != null)
                _pool.Push(this);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Handle collision with enemies
        if (other.CompareTag("Enemy") || other.CompareTag("Boss"))
        {
            // Apply damage to enemy
            EnemyUnit enemy = other.GetComponent<EnemyUnit>();
            if (enemy != null)
            {
                enemy._battleManager.enemyGetDamage(damage, enemy);
            }
        }
    }

    public void SetPool(ObjectPoolSimple<ShockWave> pool)
    {
        _pool = pool;
    }

    public static ShockWave MakeFactory(ObjectPoolSimple<ShockWave> pool)
    {
        var _obj = Resources.Load("Battle/Bullet/ShockWave");
        var _shockWave = Instantiate(_obj).GetComponent<ShockWave>();

        if (_shockWave != null)
        {
            _shockWave.SetPool(pool);
        }
        else
        {
            Debug.LogError("ShockWave instantiation failed.");
        }

        return _shockWave;
    }

  
}