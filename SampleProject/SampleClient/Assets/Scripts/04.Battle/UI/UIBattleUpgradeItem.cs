using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleUpgradeItem : MonoBehaviour
{
    [Header("[ Bind Property ]")]
    [SerializeField] private List<DOTweenAnimation> _OpenTween = null;
    [SerializeField] private List<DOTweenAnimation> _CloseTween = null;
    [SerializeField] private RectTransform _MovableTrans = null;
    [SerializeField] private Image _Img_Icon = null;
    [SerializeField] private Text _Text_Title = null;
    [SerializeField] private Text _Text_Desc = null;

    [SerializeField] private AnimationCurve _Curve = null;

    private int _IndexInList = 0;
    public int IndexInList { get { return _IndexInList; } }


    public void Set(int InIndex, int InEquipmentID, PlayerController player, Base baseStation, string type)
    {
        _IndexInList = InIndex;

        var data = TABLE.equipment.Find(InEquipmentID);

         _Img_Icon.SetSprite(EUI_AtlasType.EQUIPMENT, data.ResID);

        // Set the base name first
        _Text_Title.SetText(data.Name);
        _Text_Desc.SetText(data.Desc);

        bool weaponFound = false;
        int weaponLevel = 0;

        if (type == "player" && player._dicWeapon.TryGetValue(InEquipmentID, out Weapon weapon))
        {
            weaponLevel = weapon._level;
            weaponFound = true;
        }
        else if (type == "base" && baseStation._dicTurret.TryGetValue(InEquipmentID, out Turret turret))
        {
            weaponLevel = turret._level;
            weaponFound = true;
        }

        if (weaponFound)
        {
            _Text_Title.SetText(_Text_Title.text + " Lv." + (weaponLevel + 1));

            // Set the description based on the weapon level
            if (weaponLevel == 4)
            {
                _Text_Desc.SetText("<b><color=yellow>Additional Upgrade!</color></b> " + _Text_Desc.text);
            }
            else if (weaponLevel == 0)
            {
                _Text_Desc.SetText("<b><color=green>New!</color></b> " + _Text_Desc.text);
            }
            else if (weaponLevel == 9)
            {
                _Text_Desc.SetText("<b><color=red>Final Upgrade!</color></b> " + _Text_Desc.text);
            }
        }
        else
        {
            _Text_Desc.SetText("<b><color=green>New!</color></b> " + _Text_Desc.text);
        }
    }





    public void PlayOpenTween()
    {
        //if (_IndexInList < 0 || _IndexInList >= _OpenTween.Count)
        //{
        //    return;
        //}

        //_OpenTween[_IndexInList].DORestart();

        switch (_IndexInList)
        {
            case 0:
                _MovableTrans.anchoredPosition = new Vector2(-1000, 0);
                //_MovableTrans.DOAnchorPos(Vector2.zero, 0.5f).SetDelay(0.0f).SetUpdate(true).SetEase(_Curve);
                _MovableTrans.DOAnchorPos(Vector2.zero, 0.5f).SetDelay(0.0f).SetUpdate(true);
                break;
            case 1:
                _MovableTrans.anchoredPosition = new Vector2(0, 1000);
                _MovableTrans.DOAnchorPos(Vector2.zero, 0.5f).SetDelay(0.1f).SetUpdate(true);
                break;
            case 2:
                _MovableTrans.anchoredPosition = new Vector2(1000, 0);
                _MovableTrans.DOAnchorPos(Vector2.zero, 0.5f).SetDelay(0.2f).SetUpdate(true);
                break;
        }
    }

    public void PlayCloseTween()
    {
        //if (_IndexInList < 0 || _IndexInList >= _CloseTween.Count)
        //{
        //    return;
        //}

        //_CloseTween[_IndexInList].DORestart();

        _MovableTrans.anchoredPosition = Vector2.zero;

        switch (_IndexInList)
        {
            case 0:
                _MovableTrans.DOAnchorPos(new Vector2(0, -1000), 0.5f).SetDelay(0.0f).SetUpdate(true).SetEase(_Curve);
                break;
            case 1:
                _MovableTrans.DOAnchorPos(new Vector2(0, -1000), 0.5f).SetDelay(0.1f).SetUpdate(true).SetEase(_Curve);
                break;
            case 2:
                _MovableTrans.DOAnchorPos(new Vector2(0, -1000), 0.5f).SetDelay(0.2f).SetUpdate(true).SetEase(_Curve);
                break;
        }
    }
}
