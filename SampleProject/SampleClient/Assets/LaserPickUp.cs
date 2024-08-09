using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPickUp : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Transform player;
    private bool isRiding = false;
    private float rideDuration = 10f;
    private float fadeDuration = 2f;
    private Color initialColor;
    private Color targetColor;
    private bool isDestroyed = false;
    private bool isFadingOut = false;  // New variable to track if fading out

    public event Action onDestroyed;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialColor = spriteRenderer.color;
        targetColor = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Prevent triggering if already riding or fading out
        if (isRiding || isFadingOut) return;  // Modified line

        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.transform;
            StartCoroutine(RideSequence());
        }
    }

    private IEnumerator RideSequence()
    {
        isRiding = true;
        float rideTime = 0f;

        // Activate the child object
        if (transform.childCount > 0)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }

        // Gradually change the sprite color to white over 1 second
        while (rideTime < 1f)
        {
            rideTime += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(initialColor, targetColor, rideTime / 1f);
            yield return null;
        }

        // Continue riding for the remaining duration
        yield return new WaitForSeconds(rideDuration - 1f);

        // Stop riding
        isRiding = false;

        // Start fading out
        isFadingOut = true;  // Modified line

        // Gradually fade out the sprite over 2 seconds
        float fadeTime = 0f;
        Color startColor = spriteRenderer.color;
        while (fadeTime < fadeDuration)
        {
            fadeTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, fadeTime / fadeDuration);
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        gameObject.SetActive(false);
        isDestroyed = true;
        isFadingOut = false;  // Modified line

        // Invoke the onDestroyed event
        onDestroyed?.Invoke();
    }

    private void Update()
    {
        if (isRiding && player != null)
        {
            // Set the position to follow the player and adjust the z position to -1
            transform.position = new Vector3(player.position.x, player.position.y, -1);

            // Rotate with the player and make it upside down
            transform.rotation = player.rotation * Quaternion.Euler(0, 0, 180);
        }
    }

    public void ResetPickup()
    {
        spriteRenderer.color = initialColor;
        if (transform.childCount > 0)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        gameObject.SetActive(true);
        isDestroyed = false;
        isFadingOut = false;  // Reset fading state
    }
}
