using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;

    private PlayerController _Player = null;

    public void SetInitialHp(int maxhealth)
    {

        healthSlider.maxValue = maxhealth;
        healthSlider.value = maxhealth;
    }


    public void SetHp(int health)
    {
        healthSlider.value = health;
    }
}
