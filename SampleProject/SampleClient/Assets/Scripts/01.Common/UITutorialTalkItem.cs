

using UnityEngine;
using UnityEngine.UI;

public class UITutorialTalkItem : MonoBehaviour
{
    [SerializeField] private Image ai_image;
    [SerializeField] private UIText text;

    private RectTransform _trans = null;

    public void Set(Vector2 InPosition, int InDesc)
    {
        if (_trans == null)
            _trans = GetComponent<RectTransform>();

        _trans.anchoredPosition = InPosition;

        text.SetText(InDesc);
    }
}
