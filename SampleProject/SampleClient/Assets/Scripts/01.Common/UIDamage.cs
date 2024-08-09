using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDamage : MonoBehaviour
{
    [Header("[ Bind Property ]")]
    [SerializeField] private List<GameObject> _TweenObjList = null;

    private RectTransform _TransRect = null;

    private ObjectPool<int, UIDamage> _Pool = null;

    private List<DOTweenAnimation> _TweenList = new List<DOTweenAnimation>();

    private void Awake()
    {
        _TransRect = gameObject.GetComponent<RectTransform>();

        for (int i = 0; i < _TweenObjList.Count; i++)
        {
            var tween = _TweenObjList[i].GetComponent<DOTweenAnimation>();
            if (tween != null)
            {
                _TweenList.Add(tween);
            }
        }
    }

    public void Init(ObjectPool<int, UIDamage> InPool)
    {
        _Pool = InPool;
    }

    public void Play(Vector2 InPos)
    {
        _TransRect.anchoredPosition = InPos;

        _TransRect.SetAsLastSibling();

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

    static public UIDamage MakeFactory(int InIndex, ObjectPool<int, UIDamage> InPool)
    {
        var uiDamage = UIM.Inst().LoadUIResource("ui_damage", EUI_LoadType.COMMON).GetComponent<UIDamage>();

        uiDamage.Init(InPool);

        return uiDamage;
    }
}
