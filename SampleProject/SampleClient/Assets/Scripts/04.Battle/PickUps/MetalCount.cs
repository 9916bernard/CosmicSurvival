using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MetalCount : MonoBehaviour
{
    [HideInInspector] public Text MetalText; // Reference to the Text component
    // Start is called before the first frame update
    public void Init()
    {
        MetalText = GetComponent<Text>();

    }
}
