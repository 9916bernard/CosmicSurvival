using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShopBase : UIBase
{
    //[Header("[ Bind Property ]")]
    //[SerializeField] private UIButton Dia_Small_Btn;
    //[SerializeField] private UIButton Dia_Medium_Btn;
    //[SerializeField] private UIButton Dia_Large_Btn;
    //[SerializeField] private UIButton Gold_Small_Btn;
    //[SerializeField] private UIButton Gold_Medium_Btn;
    //[SerializeField] private UIButton Gold_Large_Btn;


    protected override void OnOpenStart()
    {
        OnRefresh();
    }

    protected override void OnRefresh()
    {
        //_Text_Nickname.SetText(USER.account.Nickname);
        //_Text_Gold.SetText(USER.fund.Gold.ToString());
    }

    public void OnClickSmallDia()
    {

        UIM.ShowToast("Purchase Success!");
        GoodsCenter.Inst().Fund_Buy_Diamond(50);

        UIM.Inst().RefreshUI(EUI_RefreshType.FUND);
    }

    public void OnClickMediumDia()
    {

        GoodsCenter.Inst().Fund_Buy_Diamond(110);
        UIM.ShowToast("Purchase Success!");


        UIM.Inst().RefreshUI(EUI_RefreshType.FUND);
    }

    public void OnClickLargeDia()
    {
        GoodsCenter.Inst().Fund_Buy_Diamond(230);
        UIM.ShowToast("Purchase Success!");
        

        UIM.Inst().RefreshUI(EUI_RefreshType.FUND);
    }

    public void OnClickSmallGold()
    {
        if (USER.fund.GetFund(ETB_FUND.DIAMOND) < 50)
        {
            UIM.ShowToast("Not enough Diamond");
            return;
        }

        GoodsCenter.Inst().Fund_Buy_Gold(50, 10000);
    }

    public void OnClickMediumGold()
    {
        if (USER.fund.GetFund(ETB_FUND.DIAMOND) < 100)
        {
            UIM.ShowToast("Not enough Diamond");
            return;
        }

        GoodsCenter.Inst().Fund_Buy_Gold(100, 20000);
    }

    public void OnClickLargeGold()
    {
        if (USER.fund.GetFund(ETB_FUND.DIAMOND) < 200)
        {
            UIM.ShowToast("Not enough Diamond");
            return;
        }

        GoodsCenter.Inst().Fund_Buy_Gold(200, 40000);
    }

    public void OnClickNoAds()
    {
        //AdMobManager.Instance.ToggleAds();

        if (USER.fund.GetFund(ETB_FUND.DIAMOND) < 200)
        {
            UIM.ShowToast("Not enough Diamond");
            return;
        }
        AdMobManager.Instance.DisAbleAds();
        string message = AdMobManager.Instance.CurrentAdPosition == AdPosition.Top ? "Ads Disabled" : "Ads Enabled";
        UIM.ShowToast(message);
    }

    //public void OnClickSmallDia()
    //{

    //}
}
