using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    // This method will be called when the button is clicked
    public void OnRestartButtonClick()
    {
        Time.timeScale = 1f; // Ensure the game time scale is reset to normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }
}
