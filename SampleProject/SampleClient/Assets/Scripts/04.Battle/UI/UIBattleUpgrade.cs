using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
//using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleUpgrade : UIBase
{
    [Header("[ Bind Property ]")]
    [SerializeField] private Graphic _Curtain = null;
    [SerializeField] private Text _Text_Desc = null;
    [SerializeField] private UIFixedListGeneric<UIBattleUpgradeItem> _Fixed_Items = null;

    private BattleManager _Battle = null;
    private PlayerController _Player = null;
    private Base _Base = null;

    private List<int> _ItemList = new();

    private bool _IsClosing = false;

    private string type;

    protected override void OnOpenStart()
    {
        _Battle = GetOpenParam<BattleManager>("BattleManager");
        _Player = GetOpenParam<PlayerController>("Player");
        type = GetOpenParam<string>("Type");
        _Base = _Battle.baseStation;

        _Curtain.raycastTarget = true;

        Time.timeScale = 0.0f;

        Set(type);

        _IsClosing = false;

        _Fixed_Items.Execute((InIndex, InItem) =>
        {
            InItem.PlayOpenTween();
        });
    }

    private void Set(string type)
    {
        _ItemList.Clear();

        if (type == "player")
        {
            if (_Player.AllInventoryWeaponsMaxed() && _Player.InventoryMaxed())
            {
                _ItemList.Add(999); // Restore Health
                _ItemList.Add(1000); // Gain Gold
            }
            else if (_Player.InventoryMaxed())
            {
                List<int> upgradableWeapons = _Player.GetUpgradableWeaponsWhenFull();
                upgradableWeapons.Shuffle();

                for (int i = 0; i < Math.Min(3, upgradableWeapons.Count); i++)
                {
                    _ItemList.Add(upgradableWeapons[i]);
                }
            }
            else
            {
                List<int> upgradableWeapons = new();
                for (int i = 1001; i <= 1008; i++)
                {
                    upgradableWeapons.Add(i);
                }
                upgradableWeapons.Shuffle();

                for (int i = 0; i < Math.Min(3, upgradableWeapons.Count); i++)
                {
                    _ItemList.Add(upgradableWeapons[i]);
                }
            }
        }
        else if (type == "base")
        {
            if (_Base.AllInventoryTurretMaxed() && _Base.isInventoryFull)
            {
                _ItemList.Add(999); // Restore Health
                _ItemList.Add(1000); // Gain Gold
            }
            else if (_Base.isInventoryFull)
            {
                List<int> upgradableWeapons = _Base.GetUpgradableTurretsWhenFull();
                upgradableWeapons.Shuffle();

                for (int i = 0; i < Math.Min(3, upgradableWeapons.Count); i++)
                {
                    _ItemList.Add(upgradableWeapons[i]);
                }
            }
            else
            {
                List<int> upgradableWeapons = new();
                for (int i = 2001; i <= 2005; i++) // Assuming base weapons have IDs 2001 to 2007
                {
                    upgradableWeapons.Add(i);
                }
                upgradableWeapons.Shuffle();

                for (int i = 0; i < Math.Min(3, upgradableWeapons.Count); i++)
                {
                    _ItemList.Add(upgradableWeapons[i]);
                }

            }
        }

        _Fixed_Items.Make(_ItemList.Count, (InIndex, InItem) =>
        {
            InItem.Set(InIndex, _ItemList[InIndex], _Player, _Base, type);
        });
    }


    private void MakeItem(int InIndex, UIBattleUpgradeItem InItem)
    {
        InItem.Set(InIndex, _ItemList[InIndex], _Player, _Base ,type);
    }

    protected override void OnRefresh()
    {
        Set(type);
    }

    public override bool OnBackButton()
    {
        return true;
    }

    protected override void OnCloseStart()
    {
        _Curtain.raycastTarget = true;

        Time.timeScale = 1.0f;
    }

    public void OnClick_Item(UIBattleUpgradeItem InItem)
    {
        if (_IsClosing == true)
        {
            return;
        }

        if (InItem.IndexInList >= 0 && InItem.IndexInList < _ItemList.Count)
        {
            int weaponID = _ItemList[InItem.IndexInList];

            // Handle the special options
            if (weaponID == 999)
            {
                // Restore Health
                _Player.health++;
                _Battle.hpBar.SetHp(_Player.health);
                
            }
            else if (weaponID == 1000)
            {
                // Gain Gold
                //_Battle.GainGold(100); // Assuming GainGold is a method in BattleManager
                //Debug.Log("Gained gold.");
            }
            else
            {
                _Player.AddWeapon(weaponID);
                
            }

            StartCoroutine(CloseProc());
        }
    }

    public void OnClick_Item_Base(UIBattleUpgradeItem InItem)
    {
        if (_IsClosing == true)
        {
            return;
        }

        if (InItem.IndexInList >= 0 && InItem.IndexInList < _ItemList.Count)
        {
            int turretID = _ItemList[InItem.IndexInList];

            // Handle the special options
            if (turretID == 999)
            {
                // Restore Health
                _Player.health++;
                _Battle.hpBar.SetHp(_Player.health);

            }
            else if (turretID == 1000)
            {
                // Gain Gold
                //_Battle.GainGold(100); // Assuming GainGold is a method in BattleManager
                //Debug.Log("Gained gold.");
            }
            else
            {

            
                    _Base.AddTurret(turretID);
                
            }

            StartCoroutine(CloseProc());
        }
    }


    IEnumerator CloseProc()
    {
        _IsClosing = true;

        _Curtain.raycastTarget = false;

        _Fixed_Items.Execute((InIndex, InItem) =>
        {
            InItem.PlayCloseTween();
        });

        yield return new WaitForSecondsRealtime(0.7f);

        Close();
    }
}
