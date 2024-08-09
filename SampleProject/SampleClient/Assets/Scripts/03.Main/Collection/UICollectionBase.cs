using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UICollectionBase : UIBase
{
    [Header("[ Bind Property ]")]
    //[SerializeField] private Text _Text_Nickname = null;
    //[SerializeField] private Text _Text_Gold = null;
    [SerializeField] private UIFixedListGeneric<UIButton> _Fixed_Tutorials = null;
    

    private List<ETB_TUTORIAL> _keyList = null;

    protected override void OnOpenStart()
    {
        OnRefresh();

        _keyList = TABLE.tutorial.mapTable.Keys.ToList();

        _Fixed_Tutorials.Make(_keyList.Count, (index, button) => {

            var text = button.gameObject.GetComponentByName<UIText>("text");
            var Tuto_Text = button.gameObject.GetComponentByName<UIText>("Tuto_type_text");
            if (text != null)
            {
                text.SetText(43100 + index);
                Tuto_Text.SetText(_keyList[index].ToString());
            }

        });
    }

    protected override void OnRefresh()
    {
        //_Text_Nickname.SetText(USER.account.Nickname);
        //_Text_Gold.SetText(USER.fund.Gold.ToString());
    }

    public void OnClick_TutorialPlay(UIText InText)
    {
        if (Enum.TryParse(InText.text, out ETB_TUTORIAL tutorialType) == true)
        {
            if (TABLE.tutorial.Find(tutorialType) != null)
            {
                UIM.ShowOverlay("ui_tutorial", EUI_LoadType.COMMON, new() { { "TutorialType", tutorialType } });

                return;
            }
        }

        UIM.ShowToast($"{tutorialType} : 없는 데요?");
    }
}
