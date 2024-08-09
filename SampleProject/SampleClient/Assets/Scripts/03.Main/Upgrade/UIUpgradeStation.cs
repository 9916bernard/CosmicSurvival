using UnityEngine;
using UnityEngine.UI;

public class UIUpgradeStation : UIUpgradeBase.Content
{
    [Header("[ Bind Property ]")]
    //[SerializeField] private UIFixedListGeneric<>
    [SerializeField] private UIFixedGridGeneric<UIUpgradeStationItem> _Fixed_Goods = null;
    private UTBUpgrade_Record _UpRec = null;

    override public void Open()
    {

        _Fixed_Goods.Make(TABLE.config_setting.UpgradeMineralList.Length-3, (index, item) =>
        {
            item.Set(index, TABLE.config_setting.UpgradeMineralList[index]);
        });



    }

    public void OnClick_Buy(UIUpgradeStationItem InItem)
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

        _UpRec = TABLE.upgrade.Find(2000 + InItem.IndexInList);

        FTB_UpgradeMineral marketData = TABLE.config_setting.UpgradeMineralList[InItem.IndexInList];

        Debug.Log($"InItem.IndexInList: {InItem.IndexInList}");
        Debug.Log($"marketData.FundType: {marketData.FundType}");
        Debug.Log($"marketData: {marketData}");
        Debug.Log($"_UpRec: {_UpRec}");

        if (USER.fund == null)
        {
            Debug.LogError("USER.fund is null");
            return;
        }

        if (USER.fund.GetFund(marketData.FundType) < _UpRec.UpgradeCost)
        {
            UIM.ShowToast($"Not Enough Mineral : {marketData.FundType}");
            return;
        }

        if (GoodsCenter.Inst() == null)
        {
            Debug.LogError("GoodsCenter instance is null");
            return;
        }

        GoodsCenter.Inst().Fund_Buy_Upgrade(marketData, _UpRec);

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
        UIM.ShowToast("Upgrade Success!");
    }


}
