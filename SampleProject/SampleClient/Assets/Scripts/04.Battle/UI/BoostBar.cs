using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostBar : MonoBehaviour
{
    [SerializeField] private Slider boostSlider;

    public void SetMaxBoost(int maxboost)
    {
        boostSlider.maxValue = maxboost;
        boostSlider.value = 100;
    }

    public void SetBoost(int boost)
    {
        boostSlider.value = boost;
    }
}
