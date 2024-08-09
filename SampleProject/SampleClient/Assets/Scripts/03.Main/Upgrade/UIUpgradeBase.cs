using UnityEngine;


public class UIUpgradeBase : UIBase
{
    public class Content : MonoBehaviour
    {
        public virtual void Open() { }
    }

    [Header("[ Bind Property ]")]
    [SerializeField] private UIDynamicScrollView _ScrollWorld = null;
    //[SerializeField] UIToggleList _Toggle_Contents = null;
    [SerializeField] UIToggleListGeneric<Content> _ToggleGeneral_Contents = null;
    [SerializeField] UIToggleList _Toggle_Selected = null;

    private int _SelectedTabIndex = 0;

    protected override void OnOpenStart()
    {
        //OnRefresh();
        
        OnClick_Tab(0);
    }

    protected override void OnOpenTuto()
    {
        if (!USER.player.IsTutorialCompleted("LOBBY_UPGRADE"))
        {
            UIM.ShowOverlay("ui_tutorial", EUI_LoadType.COMMON, new() { { "TutorialType", ETB_TUTORIAL.LOBBY_UPGRADE } });
            USER.player.SetTutorialCompleted("LOBBY_UPGRADE");
        }
        OnClick_Tab(0);
    }

    protected override void OnRefresh()
    {
        //OnClick_Count(2);
        //_ScrollWorld.Make(2, (InIndex, InItem) =>
        //{
        //    InItem.GetComponent<UIUpgradeItem>().Set(InIndex);
        //});
    }

    //public void OnClick_Count(int InCount)
    //{
    //    _ScrollWorld.Make(InCount, (InIndex, InItem) =>
    //    {
    //        UIUpgradeItem upgradeItem = InItem.GetComponent<UIUpgradeItem>();
    //        var actualIndex = InIndex * _Page;
    //        upgradeItem.Set(actualIndex);

    //        // Assuming each UIUpgradeItem has a button component
    //        Button button = upgradeItem.GetComponent<Button>();
    //        if (button != null)
    //        {
    //            button.onClick.AddListener(() => OnClick_Upgrade(actualIndex));
    //        }
    //    });
    //}

    public void OnClick_Tab(int InTabIndex)
    {
        _SelectedTabIndex = InTabIndex;

        //var selectedObject = _Toggle_Contents.SetAndGet(_SelectedTabIndex);

        var selectContent = _ToggleGeneral_Contents.SetAndGet(_SelectedTabIndex);

        selectContent.Open();

        //selectedObject

        _Toggle_Selected.Set(_SelectedTabIndex);
        //_Page = InPage;
    }


    //public void OnClick_Upgrade(int InIndex)
    //{
    //    switch(
    //        InIndex)
    //    {
    //        case 0:
    //            if(USER.fund.GetGold() >= 100)
    //            {
    //                USER.fund.AddGold(-100);
    //                USER.fund.AddPearl(1);
    //                USER.Inst().SaveData();
    //            }
    //            else
    //            {
    //                Debug.Log("Not enough gold");
    //            }
    //            break;
    //        case 1:
    //            if(USER.fund.GetPearl() >= 1)
    //            {
    //                USER.fund.AddPearl(-1);
    //                USER.fund.AddGold(100);
    //                USER.Inst().SaveData();
    //            }
    //            else
    //            {
    //                Debug.Log("Not enough pearl");
    //            }
    //            break;
          
    //    }
    //}
}
