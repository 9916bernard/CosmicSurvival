using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : UIBase
{
    private Text timerText; // Reference to the Text component
    private float startTime; // Time when the game started
    private BattleManager battleManager; // Reference to the BattleManager script

    void Start()
    {
        battleManager = FindAnyObjectByType<BattleManager>();
        timerText = GetComponent<Text>();
        
    }

    void Update()
    {
        float elapsedTime = battleManager.battleTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60); // Calculate minutes
        int seconds = Mathf.FloorToInt(elapsedTime % 60); // Calculate seconds
       

        // Update the Text component with the elapsed time
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
