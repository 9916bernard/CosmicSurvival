using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIUpgradeMineralItem : MonoBehaviour
{
    [Header("[ Bind Property ]")]
    [SerializeField] private Image _Image_Mineral = null;
    [SerializeField] private Text _Text_Amount = null;

    private int _IndexInList = 0;
    public int IndexInList { get { return _IndexInList; } }

    private UTBFund_Record _Rec = null;
    public UTBFund_Record Rec { get { return _Rec; } }

    public void Set(int InIndexInList, FTB_UpgradeMineral InData)
    {
        _IndexInList = InIndexInList;

        _Rec = TABLE.fund.Find(InData.FundType);

        if (_Rec == null)
            return;

        var _Mineral = USER.fund.GetFund(InData.FundType).ToString().Trim();


        // Remove the prefix "MINERAL_" if it exists, ignoring case and spaces
        const string prefix = "MINERAL_";
        if (_Mineral.StartsWith(prefix, System.StringComparison.OrdinalIgnoreCase))
        {
            _Mineral = _Mineral.Substring(prefix.Length).Trim();
        }

        var fs = _Rec.FundType.ToString();
        //if (fs.StartsWith(prefix, System.StringComparison.OrdinalIgnoreCase))
        //{
        //    fs = fs.Substring(prefix.Length).Trim();
        //}

        //fs = fs.Replace(prefix, "");

        //_Text_Name.SetText(fs);
        //_Text_Name.SetText(_Rec.Name);
        _Text_Amount.SetText(_Mineral.ToString());
        _Image_Mineral.SetSprite(EUI_AtlasType.UI, _Rec.Name);

        // Refresh();
    }

    public void Refresh()
    {
        // 돈 같은거
    }
}
