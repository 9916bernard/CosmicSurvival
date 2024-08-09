using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class UIUpgradeStationItem : MonoBehaviour
{
    [Header("[ Bind Property ]")]
    [SerializeField] private Image Image_Mineral = null;
    [SerializeField] private Text _Text_Amount = null;
    [SerializeField] private Text _Text_Upgrade_Name = null;
    [SerializeField] private Text _Text_Upgrade_Level = null;
    [SerializeField] private Text _Text_Stone_Cost = null;

    private int _IndexInList = 0;
    public int IndexInList { get { return _IndexInList; } }

    private UTBFund_Record _Rec = null;

    private UTBUpgrade_Record _UpRec = null;
    public UTBFund_Record Rec { get { return _Rec; } }

    public void Set(int InIndexInList, FTB_UpgradeMineral InData)
    {
        _IndexInList = InIndexInList;

        _Rec = TABLE.fund.Find(InData.FundType);


        _UpRec = TABLE.upgrade.Find(2000 + InIndexInList);



        if (_Rec == null)
            return;

        var _Mineral = USER.fund.GetFund(InData.FundType).ToString().Trim();

        // Remove the prefix "MINERAL_" if it exists, ignoring case and spaces
        const string prefix = "MINERAL_";
        //if (_Mineral.StartsWith(prefix, System.StringComparison.OrdinalIgnoreCase))
        //{
        //    _Mineral = _Mineral.Substring(prefix.Length).Trim();
        //}

        var fs = _Rec.FundType.ToString();
        if (fs.StartsWith(prefix, System.StringComparison.OrdinalIgnoreCase))
        {
            fs = fs.Substring(prefix.Length).Trim();
        }

        var UpgradeText = _UpRec.UpgradeType.ToString();

        const string upPrefix = "STATION_";

        if (UpgradeText.StartsWith(upPrefix, System.StringComparison.OrdinalIgnoreCase))
        {
            UpgradeText = UpgradeText.Substring(upPrefix.Length).Trim();
        }

        //fs = fs.Replace(prefix, "");

        _Text_Upgrade_Name.SetText(41015 + InIndexInList);
        Image_Mineral.SetSprite(EUI_AtlasType.UI, _Rec.Name);
        //_Text_Name.SetText(_Rec.Name);
        _Text_Amount.SetText(_Mineral.ToString() + " /");
        _Text_Stone_Cost.SetText(_UpRec.UpgradeCost.ToString());
        _Text_Upgrade_Level.SetText("LV. " + USER.upgrade.GetUpgradeEffectLevel(_UpRec.UpgradeType).ToString());

        // Refresh();
    }

    public void Refresh()
    {
        // µ· °°Àº°Å
    }


}
