using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public partial class UIM : Singleton<UIM>
{
    public static UIBase ShowBase(string InName, EUI_LoadType InLoadType, bool InIsHost = false, Dictionary<string, object> InOpenParam = null)
    {
		var ui = Inst().ShowUI(InName, EUI_AreaType.BASE, InLoadType, InOpenParam);

		if (InIsHost == true)
		{
            Inst().SetContainerHost(EUI_AreaType.BASE, ui);
		}

		return ui;
    }

    public static UIBase ShowBattleUI(string InName, EUI_LoadType InLoadType, bool InIsHost = false, Dictionary<string, object> InOpenParam = null)
    {
        var ui = Inst().ShowUI(InName, EUI_AreaType.BASE, InLoadType, InOpenParam);

        if (InIsHost == true)
        {
            Inst().SetContainerHost(EUI_AreaType.BASE, ui);
        }

        return ui;
    }


    public static UIBase ShowPopup(string InName, EUI_LoadType InLoadType, Dictionary<string, object> InOpenParam = null)
    {
        return Inst().ShowUI(InName, EUI_AreaType.POPUP, InLoadType, InOpenParam);
    }

    public static UIBase ShowOverlay(string InName, EUI_LoadType InLoadType, Dictionary<string, object> InOpenParam = null)
    {
        return Inst().ShowUI(InName, EUI_AreaType.OVERLAY, InLoadType, InOpenParam);
    }

    public static void ShowToast(string InText)
    {
        UIToast toast = Inst().Toast;
        if (toast != null)
        {
            toast.SetActiveEx(true);
            toast.SetText(InText);
            toast.Open(EUI_AreaType.MANAGED);
        }
    }

    public static void ShowToast(int InTextID)
    {
        UIToast toast = Inst().Toast;
        if (toast != null)
        {
            toast.SetActiveEx(true);
            toast.SetText(InTextID);
            toast.Open(EUI_AreaType.MANAGED);
        }
    }

    public static void ShowChoice2(int InTextID, Action InOnYes, Action InOnNo = null)
    {
        ShowChoice2(Inst().GetText(InTextID), InOnYes, InOnNo);
    }

    public static void ShowChoice2(string InText, Action InOnYes, Action InOnNo = null)
    {
        Inst().ShowUI("ui_choice_2", EUI_AreaType.POPUP, EUI_LoadType.COMMON, new() {
            { "Desc", InText },
            { "OnYes", InOnYes },
            { "OnNo", InOnNo },
        });
    }
}