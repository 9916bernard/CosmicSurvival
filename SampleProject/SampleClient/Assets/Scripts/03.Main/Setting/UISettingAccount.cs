using System;
using UnityEngine;
using UnityEngine.UI;

public class UISettingAccount : UIBase
{
    
  

    protected override void OnOpenStart()
    {
        
    }

    public void OnClick_SignUp()
    {
        UIM.ShowPopup("ui_setting_signup_email", EUI_LoadType.SETTING);
    }

    public void OnClick_Login()
    {
        UIM.ShowPopup("ui_setting_login_email", EUI_LoadType.SETTING);
    }

    public void OnClick_Logout()
    {
        
      UIM.ShowPopup("ui_setting_logout_confirm", EUI_LoadType.SETTING);
    }
    
}
