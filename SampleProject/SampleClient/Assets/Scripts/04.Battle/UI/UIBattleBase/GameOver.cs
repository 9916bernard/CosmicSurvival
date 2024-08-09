using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject fingerStick; // Reference to the FingerStick GameObject
    [SerializeField] private GameObject gameOverUI; // Reference to the Game Over UI GameObject
    private RectTransform rect;

    private void Start()
    {
        rect = gameOverUI.GetComponent<RectTransform>();
    }

    public void Show()
    {
        rect.localScale = Vector3.one;
        if (fingerStick != null)
        {
            fingerStick.SetActive(false);
        }
        
        Time.timeScale = 0f; // Pause the game
    }

}
