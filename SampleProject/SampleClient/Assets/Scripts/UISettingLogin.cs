using System;
using UnityEngine;
using UnityEngine.UI;

public class UISettingLogin : UIBase
{



    protected override void OnOpenStart()
    {

    }

    public void OnClick_Google()
    {
     
    }

    public void OnClick_Email()
    {
        UIM.ShowPopup("ui_setting_login_email", EUI_LoadType.SETTING);
    }

}
