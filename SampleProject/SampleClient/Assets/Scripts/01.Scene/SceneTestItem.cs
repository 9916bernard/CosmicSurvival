using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SceneTestItem : UIDynamicScrollViewIrregular.Item
{
    [SerializeField] private UIText _Text_Message = null;

    private SceneTest.MyInfo _MyInfo = null;

    public override void OnPop(UIDynamicScrollViewIrregular.Info InInfo, int InItemWidth)
    {
        base.OnPop(InInfo, InItemWidth);

        var size = _Trans.GetSize();
        if (InItemWidth != 0)
            size.x = InItemWidth;
        size.y = _Info.GetHeight();
        _Trans.SetSize(size);

        _MyInfo = InInfo as SceneTest.MyInfo;
        if (_MyInfo != null)
        {
            Set();
        }
    }

    private void Set()
    {
        _Text_Message.SetText(_MyInfo._Message);
    }
}