using UnityEngine;
using UnityEngine.UI;

public class UIUpgradeMineral : UIUpgradeBase.Content
{
    [Header("[ Bind Property ]")]
    [SerializeField] private Text _Text_Title = null;
    [SerializeField] private UIFixedGridGeneric<UIUpgradeMineralItem> _Fixed_Goods = null;
    [SerializeField] private UIText _Text_Gold = null;


    override public void Open()
    {
        _Text_Title.SetText($"나는 미네랄 {Time.realtimeSinceStartup}");

        _Fixed_Goods.Make(TABLE.config_setting.UpgradeMineralList.Length, (index, item) =>
        {
            item.Set(index, TABLE.config_setting.UpgradeMineralList[index]);
        });

        _Text_Gold.SetText($": " + USER.fund.GetFund(ETB_FUND.GOLD).ToString());
    }

    protected void OnRefresh()
    {
        _Text_Gold.SetText($": " + USER.fund.GetFund(ETB_FUND.GOLD).ToString());
    }

    public void OnClick_Plus(UIUpgradeMineralItem InItem)
    {
        if (InItem.IndexInList < 0 || InItem.IndexInList >= TABLE.config_setting.UpgradeMineralList.Length)
        {
            return;
        }

        FTB_UpgradeMineral marketData = TABLE.config_setting.UpgradeMineralList[InItem.IndexInList];

        if (USER.fund.GetFund(ETB_FUND.GOLD) < 200)
        {
            UIM.ShowToast($"돈 부족 : {marketData.FundType}");
            return;
        }
        GoodsCenter.Inst().Fund_Change_Gold_To_Stone(marketData);
        _Fixed_Goods.RefreshOnly();
        //_Fixed_Goods.Execute((index, item) => { item.Refresh(); });

        UIM.Inst().RefreshUI(EUI_RefreshType.FUND);
        OnRefresh();

        //USER.fund.AddFund(marketData.FundType, -100);

        // 서버

    }

    public void OnClick_Minus(UIUpgradeMineralItem InItem)
    {
        if (InItem == null)
        {
            Debug.LogError("InItem is null");
            return;
        }

        if (InItem.IndexInList < 0 || InItem.IndexInList >= TABLE.config_setting.UpgradeMineralList.Length)
        {
            Debug.LogError("InItem.IndexInList is out of range");
            return;
        }

        if (TABLE.config_setting.UpgradeMineralList == null)
        {
            Debug.LogError("UpgradeMineralList is null");
            return;
        }

        FTB_UpgradeMineral marketData = TABLE.config_setting.UpgradeMineralList[InItem.IndexInList];

        if (USER.fund == null)
        {
            Debug.LogError("USER.fund is null");
            return;
        }

        if (USER.fund.GetFund(marketData.FundType) < 1)
        {
            UIM.ShowToast($"광석 부족 : {marketData.FundType}");
            return;
        }

        if (GoodsCenter.Inst() == null)
        {
            Debug.LogError("GoodsCenter instance is null");
            return;
        }

        GoodsCenter.Inst().Fund_change_Stone_To_Gold(marketData);

        if (_Fixed_Goods == null)
        {
            Debug.LogError("_Fixed_Goods is null");
            return;
        }

        _Fixed_Goods.RefreshOnly();

        if (UIM.Inst() == null)
        {
            Debug.LogError("UIM instance is null");
            return;
        }

        UIM.Inst().RefreshUI(EUI_RefreshType.FUND);
        OnRefresh();
    }

}