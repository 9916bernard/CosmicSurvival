using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleBase : UIBase
{
    public ExpBar expBar;
    public HpBar hpBar;
    public BoostBar boostBar;
    public GameObject baseEngineIdle;
    public GameObject baseEngineBoost;
    public GameOver gameOver;
    public DamageUi damageUi;
    public FingerStick fingerStick;
    public NavigationArrow navigationArrow;
    [SerializeField] private GameObject top_panel;


    [HideInInspector] public BattleManager battleManager;
    public MetalCount metalCount;
    public Text goldText;
    public Text astroidText;

    protected override void OnOpenStart()
    {
        battleManager = GetOpenParam<BattleManager>("BattleManager");

        // Subscribe to banner visibility changes
        AdMobManager.Instance.OnBannerVisibilityChanged += OnBannerVisibilityChanged;
    }

    protected override void OnRefresh()
    {

    }

    public void OnClick_TestUpgrade()
    {
        // 파란 버튼
        battleManager.GainExperience(20);
        //OnBannerVisibilityChanged(true);
    }

    public void OnClick_TestFinish()
    {
        // 빨간 버튼
        //UIM.ShowPopup("ui_battle_upgrade", EUI_LoadType.BATTLE);

        //Time.timeScale = 3.0f;

        battleManager.BaseLevelUp();
        //OnBannerVisibilityChanged(false);
    }

    public void OnClick_die()
    {
        //UIM.ShowPopup("ui_battle_upgrade", EUI_LoadType.BATTLE);

        battleManager.getDamage(10);
    }

    public void OnClick_time()
    {
        if (!battleManager.isTimeFast)
        {
            Time.timeScale = 3.0f;
            battleManager.isTimeFast = true;
        }
        else
        {
            Time.timeScale = 1.0f;
            battleManager.isTimeFast = false;
        }

    }

    private void OnBannerVisibilityChanged(bool isVisible)
    {
        RectTransform topPanelRect = top_panel.GetComponent<RectTransform>();
        if (isVisible)
        {
            topPanelRect.anchoredPosition = new Vector2(topPanelRect.anchoredPosition.x, -180f);
        }
        else
        {
            topPanelRect.anchoredPosition = new Vector2(topPanelRect.anchoredPosition.x, -81f);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        goldText.text = battleManager.gold.ToString();
        astroidText.text = (battleManager.Ruby + battleManager.Sapphire + battleManager.pearl + battleManager.Amber + battleManager.Emerald + battleManager.Black).ToString();
    }

    private void OnDestroy()
    {
        // Unsubscribe to avoid potential memory leaks
        AdMobManager.Instance.OnBannerVisibilityChanged -= OnBannerVisibilityChanged;
    }
}
