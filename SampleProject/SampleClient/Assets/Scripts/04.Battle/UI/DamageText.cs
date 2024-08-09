using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    [Header("[ Bind Property ]")]
    [SerializeField] private TextMeshPro _Text_Damage = null;
	[SerializeField] private List<GameObject> _TweenObjList = null;

    private Transform _Trans = null;

    [HideInInspector] public ObjectPoolSimple<DamageText> _Pool = null;

    private List<DOTweenAnimation> _TweenList = new List<DOTweenAnimation>();

    private void Awake()
    {
		_Trans = gameObject.GetComponent<Transform>();

        for (int i = 0; i < _TweenObjList.Count; i++)
        {
            var tween = _TweenObjList[i].GetComponent<DOTweenAnimation>();
            if (tween != null)
            {
                _TweenList.Add(tween);
            }
        }
    }

    public void Init(ObjectPoolSimple<DamageText> InPool)
    {
        _Pool = InPool;
    }

    public void Play(Vector3 InPos, BattleDamage InDamage)
    {
		_Trans.localPosition = InPos;

		_Text_Damage.SetText(InDamage.Damage.ToString());

		gameObject.SetActive(true);

        PlayTween();
    }

    protected void PlayTween()
    {
        for (int i = 0; i < _TweenList.Count; i++)
        {
            _TweenList[i].DORestart();
        }
    }

    public void OnComplete()
    {
        _Pool.Push(this);
    }

    static public DamageText MakeFactory(ObjectPoolSimple<DamageText> InPool)
    {
        var _DamageText = Instantiate(RESOURCE.Inst().GetObject<DamageText>("Battle/battle_damage_text"));

        _DamageText.Init(InPool);

        return _DamageText;
    }
}
