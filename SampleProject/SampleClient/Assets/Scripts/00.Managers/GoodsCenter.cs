using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsCenter : Singleton<GoodsCenter>
{

    public int moneyUsed = 0;
    public override void Init()
    {
        RestoreStaticModule();
    }

    public void Fund_Buy_Diamond(int diamond)
    {
        switch (diamond)
        {
        case 50:
                moneyUsed += 500;
                break;
            case 110:
                moneyUsed += 1000;
                break;
            case 230:
                moneyUsed += 2000;
                break;
            }

        USER.fund.AddFund(ETB_FUND.DIAMOND, diamond);
        USER.Inst().SaveData();
    }


    public void Fund_Change_Gold_To_Stone(FTB_UpgradeMineral InData)
    {
        USER.fund.AddFund(ETB_FUND.GOLD, -200);
        USER.fund.AddFund(InData.FundType, 1);

        USER.Inst().SaveData();
    }

    public void Fund_change_Stone_To_Gold(FTB_UpgradeMineral InData)
    {
        USER.fund.AddFund(ETB_FUND.GOLD, 100);
        USER.fund.AddFund(InData.FundType, -1);

        USER.Inst().SaveData();
    }

    public void Fund_Buy_Upgrade(FTB_UpgradeMineral InDataMineral, UTBUpgrade_Record InDataUpgrade)
    {
        USER.fund.AddFund(InDataMineral.FundType, -InDataUpgrade.UpgradeCost);
        USER.upgrade.Upgrade(InDataUpgrade.UpgradeType);

        USER.Inst().SaveData();
    }

    public void Fund_Buy_Gold(int diamond, int gold)
    {
        USER.fund.AddFund(ETB_FUND.GOLD, +gold);
        USER.fund.AddFund(ETB_FUND.DIAMOND, -diamond);

        USER.Inst().SaveData();

        UIM.Inst().RefreshUI(EUI_RefreshType.FUND);
        UIM.ShowToast("Purchased!");
    }


}
