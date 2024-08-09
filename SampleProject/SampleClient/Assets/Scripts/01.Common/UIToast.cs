using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIToast : UIBase
{
    [Header("[ Bind Property ]")]
    [SerializeField] private Text _Text_Desc = null;

    protected override void OnOpenStart()
    {
        Debug.Log($"Open Toast : {Time.realtimeSinceStartup}");
        PlayTween();
    }

    public void SetText(string InText)
    {
        Debug.Log($"Set Toast : {Time.realtimeSinceStartup}");
        _Text_Desc.SetText(InText);
    }

    public void SetText(int InTextID)
    {
        Debug.Log($"Set Toast : {Time.realtimeSinceStartup}");
        _Text_Desc.SetText(InTextID);
    }
}
