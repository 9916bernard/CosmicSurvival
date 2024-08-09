using System;
using UnityEngine;
using UnityEngine.UI;

public class UIChoice2 : UIBase
{
    [Header("[ Bind Property ]")]
    [SerializeField] private Text _Text_Desc = null;

    private Action _OnNo = null;
    private Action _OnYes = null;

    protected override void OnOpenStart()
    {
        _Text_Desc.SetText(GetOpenParam<string>("Desc"));
        _OnNo = GetOpenParam<Action>("OnNo");
        _OnYes = GetOpenParam<Action>("OnYes");
    }

    public void OnClick_No()
    {
        _OnNo?.Invoke();

        _OnNo = null;
        _OnYes = null;

        Close();
    }
    public void OnClick_Yes()
    {
        _OnYes?.Invoke();

        _OnNo = null;
        _OnYes = null;

        Close();
    }
}
