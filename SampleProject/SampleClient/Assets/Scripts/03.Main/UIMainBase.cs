using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIMainBase : UIBase
{
    [Header("[ Profile : Bind Property ]")]
    [SerializeField] private UIText _AccountName = null;
    [SerializeField] private UIText _Gold = null;
    [SerializeField] private UIText _Diamond = null;

    [Header("[ Middle : Bind Property ]")]
    [SerializeField] private GameObject _topOriginObj = null;
    [SerializeField] private GameObject _topObj = null;
    [SerializeField] private GameObject _MiddleObj = null;
    [SerializeField] private UIText _recordText = null;
    [SerializeField] private Text _money_used = null;

    [Header("[ Tab Buttons : Bind Property ]")]
    [SerializeField] private GameObject _TabObj = null;
    [SerializeField] private UIToggleList _BaseBtns = null;
    [SerializeField] private UIToggleList _CloseBtn = null;

    private UIToggleListGeneric<UIBase> _BaseList = new UIToggleListGeneric<UIBase>();
    private UIBattleBase _uiBattle = null;
    private int _CurrentActiveTab = -1;
    private Action _StartAction = null;

    protected override void OnOpenStart()
    {
        _StartAction = GetOpenParam<Action>("StartAction");
        var battleManager = GetOpenParam<BattleManager>("BattleManager");

        // One-time initialization
        _BaseList.AddItem(UIM.ShowBase("ui_ranking_base", EUI_LoadType.RANKING));
        _BaseList.AddItem(UIM.ShowBase("ui_upgrade_base", EUI_LoadType.UPGRADE));
        _BaseList.AddItem(UIM.ShowBase("ui_collection_base", EUI_LoadType.COLLECTION));
        _BaseList.AddItem(UIM.ShowBase("ui_shop_base", EUI_LoadType.SHOP));

        _uiBattle = UIM.ShowBase("ui_battle_base", EUI_LoadType.BATTLE, true, new() { { "BattleManager", battleManager } }) as UIBattleBase;
        _uiBattle.SetActiveEx(false);

        OnClick_Close();

        // Subscribe to banner visibility changes
        AdMobManager.Instance.OnBannerVisibilityChanged += OnBannerVisibilityChanged;

        OnRefresh();
    }

    public UIBattleBase GetBattleUI()
    {
        return _uiBattle;
    }

    public void OnClick_Tab(int InIndex)
    {
        _BaseBtns.InverseSet(InIndex);
        _CloseBtn.Set(InIndex);

        var ui = _BaseList.SetAndGet(InIndex);
        ui.OpenTab(EUI_AreaType.BASE);

        _CurrentActiveTab = InIndex;
    }

    public void OnClick_Close()
    {
        _BaseBtns.SetActive(true);
        _CloseBtn.SetActive(false);

        _BaseList.SetActive(false);

        _CurrentActiveTab = -1;
    }

    public void OnClick_Profile()
    {
        UIM.ShowPopup("ui_setting", EUI_LoadType.SETTING, new() { { "Desc", "설명 입니다." }, { "Name", 11005 } });
    }

    public void OnClick_Start()
    {
        DOTween.Restart(_topObj);
        DOTween.Restart(_MiddleObj);
        DOTween.Restart(_TabObj);

        _uiBattle.SetActiveEx(true);

        _StartAction?.Invoke();
    }

    public void SetBattleEnd()
    {
        _uiBattle.SetActiveEx(false);

        DOTween.Rewind(_topObj);
        DOTween.Rewind(_MiddleObj);
        DOTween.Rewind(_TabObj);
        OnRefresh();
    }

    protected override void OnRefresh()
    {
        // Profile
        _AccountName.SetText(USER.account.data.AccountName);

        // Fund
        _Gold.SetText($": {USER.fund.GetFund(ETB_FUND.GOLD)}");
        _Diamond.SetText($": {USER.fund.GetFund(ETB_FUND.DIAMOND)}");

        // Player
        int totalSeconds = (int)USER.player.GetRecord();
        TimeSpan timeSpan = TimeSpan.FromSeconds(totalSeconds);
        string formattedTime = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        _recordText.SetText($"Best Record : {formattedTime}");
        _money_used.SetText($"Money Used: {GoodsCenter.Inst().moneyUsed} KRW");

        //Debug.Log(">>>>>>>>>>>>>>>> UIMainBase Refreshed.");
    }

    private void OnBannerVisibilityChanged(bool isVisible)
    {
        RectTransform topObjRect = _topOriginObj.GetComponent<RectTransform>();
        if (isVisible)
        {
            topObjRect.anchoredPosition = new Vector2(topObjRect.anchoredPosition.x, -193f);
        }
        else
        {
            topObjRect.anchoredPosition = new Vector2(topObjRect.anchoredPosition.x, -93f);
        }
    }

    public override bool OnBackButton()
    {
        if (_CurrentActiveTab < 0)
        {
            return false;
        }

        OnClick_Close();

        return true;
    }

    private void OnDestroy()
    {
        // Unsubscribe to avoid potential memory leaks
        AdMobManager.Instance.OnBannerVisibilityChanged -= OnBannerVisibilityChanged;
    }
}
