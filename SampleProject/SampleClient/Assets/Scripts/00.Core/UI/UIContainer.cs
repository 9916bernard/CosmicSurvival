using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIContainer
{
    private Stack<UIBase> _ActiveStack = new Stack<UIBase>();

    private Transform _TransRoot = null;

    private UIBase _Host = null;


	public void SetRoot(GameObject InObject)
    {
        RectTransform transRoot = InObject.GetComponent<RectTransform>();

        transRoot.anchorMin = Vector2.zero;
        transRoot.anchorMax = Vector2.one;
        transRoot.offsetMin = Vector2.zero;
        transRoot.offsetMax = Vector2.zero;

        _TransRoot = InObject.transform;
    }

    public void SetHost(UIBase InHost)
    {
		_Host = InHost;

	}

    public void Clear()
    {
        _ActiveStack.Clear();

        _TransRoot = null;
	}

    public void Push(UIBase InUI)
    {
        _ActiveStack.Push(InUI);

        InUI.transform.SetParent(_TransRoot);

        InUI.transform.SetAsLastSibling();

        Util.SetActive(InUI, true);
    }

    public void Pop()
    {
        if (_ActiveStack.Count > 0 )
        {
            UIBase popUI = _ActiveStack.Pop();

            Util.SetActive(popUI, false);
        }
    }

    public UIBase Peek()
    {
        if (_ActiveStack.Count > 0)
        {
            return _ActiveStack.Peek();
        }

        return null;
    }

    public UIBase Host()
    {
        return _Host;
    }

	public void RefreshActive(params EUI_RefreshType[] InRefreshTypeArray)
    {
        foreach (var ui in _ActiveStack)
        {
            if (ui.gameObject.activeSelf == true)
            {
                ui.Refresh(InRefreshTypeArray);
            }
        }
    }

    public void RefreshAll(params EUI_RefreshType[] InRefreshTypeArray)
    {
        foreach (var ui in _ActiveStack)
        {
            ui.Refresh(InRefreshTypeArray);
        }
    }

    public void RefreshActiveForce()
    {
        foreach (var ui in _ActiveStack)
        {
            if (ui.gameObject.activeSelf == true)
            {
                ui.RefreshForce();
            }
        }
    }

    public void RefreshAllForce()
    {
        foreach (var ui in _ActiveStack)
        {
            ui.RefreshForce();
        }
    }
}