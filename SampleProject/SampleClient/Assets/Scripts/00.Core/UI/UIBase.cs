using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class UIBase : MonoBehaviour
{
    [Header("[ UIBase ]")]
    [SerializeField] private List<EUI_RefreshType> _RefreshTypeList = null;
    [SerializeField] private List<GameObject> _TweenObjList = null;
    [Header("")]

    private bool _IsManaged = false;

    private EUI_AreaType _AreaType = EUI_AreaType.BASE;

    private Dictionary<string, object> _OpenParam = null;

    private Animator _Anim = null;

    private List<DOTweenAnimation> _TweenList = new List<DOTweenAnimation>();

    private bool _IsBusy = false;
    public bool IsBusy { get { return _IsBusy; } set { _IsBusy = value; } }


    private void Awake()
    {
        _Anim = GetComponent<Animator>();

        for (int i = 0; i < _TweenObjList.Count; i++)
        {
            var tween = _TweenObjList[i].GetComponent<DOTweenAnimation>();
            if (tween != null)
            {
                _TweenList.Add(tween);
            }
        }
    }

    public void SetManaged()
    {
        _IsManaged = true;
    }

    public virtual void Init(BattleManager battleManager)
    {
        
    }

    protected void PlayAnim(string InStateName)
    {
        if (_Anim != null)
        {
            _Anim.Play(InStateName);
        }
    }

    protected void PlayTween()
    {
        for (int i = 0; i < _TweenList.Count; i++)
        {
            _TweenList[i].DORestart();
        }
    }

    protected virtual void OnOpenStart()
    {
    }

    protected virtual void OnOpenTuto()
    {

    }

    protected virtual void OnOpenEnd()
    {
    }

    protected virtual void OnCloseStart()
    {
        for (int i = 0; i < _TweenList.Count; i++)
        {
            _TweenList[i].DOPlayBackwards();
        }
    }

    protected virtual void OnCloseEnd()
    {
    }

    protected virtual void OnRefresh()
    {
    }

    public virtual bool OnBackButton()
    {
        return _IsBusy;
    }

    public void Open(EUI_AreaType InAreaType, Dictionary<string, object> InOpenParam = null)
    {
        _AreaType = InAreaType;
        _OpenParam = InOpenParam;

        OnOpenStart();
        OnOpenEnd();
    }

    public void OpenTab(EUI_AreaType InAreaType, Dictionary<string, object> InOpenParam = null)
    {
        _AreaType = InAreaType;
        _OpenParam = InOpenParam;

        OnOpenStart();
        OnOpenEnd();

        OnOpenTuto();
    }

    public void Close()
    {
        CloseStart();
    }

    public object GetOpenParam(string InKey)
    {
        object outValue = null;
        _OpenParam.TryGetValue(InKey, out outValue);
        return outValue;
    }
    public T GetOpenParam<T>(string InKey)
    {
        object outValue = null;
        if (_OpenParam.TryGetValue(InKey, out outValue) == true)
        {
            return (T)outValue;
        }
        return default;
    }

    public void Refresh(params EUI_RefreshType[] InRefreshTypeArray)
    {
        for (int i = 0; i < InRefreshTypeArray.Length; i++)
        {
            if (_RefreshTypeList.Contains(InRefreshTypeArray[i]) == true)
            {
                OnRefresh();
                break;
            }
        }
    }

    public void RefreshForce()
    {
        OnRefresh();
    }

    private void CloseStart()
    {
        OnCloseStart();

        //UIM.Inst().CloseUI(_AreaType);

        if (_IsManaged == false)
            UIM.Inst().CloseUI(_AreaType);
        else
            gameObject.SetActive(false);
    }
}