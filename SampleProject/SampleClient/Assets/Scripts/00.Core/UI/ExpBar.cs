using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
    [SerializeField] private Slider expSlider;

    public void SetMaxExperience(int experience)
    {
        expSlider.maxValue = experience;
        expSlider.value = 0;
    }

    public void SetExperience(int experience)
    {
        expSlider.value = experience;
    }
}
