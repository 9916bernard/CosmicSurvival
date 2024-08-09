using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TimeBtn : MonoBehaviour
{
    public Image baseImage;
    public Image pressedImage;
    private Sprite originalBaseImage;
    private Sprite originalPressedImage;
    public Sprite timeBtnOnBaseImage;
    public Sprite timeBtnOnPressedImage;

    private void Start()
    {
        // Save the original images
        originalBaseImage = baseImage.sprite;
        originalPressedImage = pressedImage.sprite;

        // Load the new images from the Resources folder

    }

    private void Update()
    {
        if (Time.timeScale > 1)
        {
            // Change to the new images if timeScale is greater than 1
            baseImage.sprite = timeBtnOnBaseImage;
            pressedImage.sprite = timeBtnOnPressedImage;
        }
        else if (Time.timeScale <= 1)
        {
            // Revert to the original images if timeScale is equal or smaller than 1
            baseImage.sprite = originalBaseImage;
            pressedImage.sprite = originalPressedImage;
        }
    }
}
