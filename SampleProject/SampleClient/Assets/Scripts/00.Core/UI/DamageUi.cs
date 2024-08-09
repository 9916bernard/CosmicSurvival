using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUi : MonoBehaviour
{
    private RectTransform rect;

    // Start is called before the first frame update
    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    // Method to show the UI for 0.2 seconds
    public void ShowDamageUI()
    {
        StartCoroutine(ShowDamageUICoroutine());
    }

    private IEnumerator ShowDamageUICoroutine()
    {
        rect.localScale = Vector3.one;
        yield return new WaitForSeconds(0.5f);
        rect.localScale = Vector3.zero;
    }
}
