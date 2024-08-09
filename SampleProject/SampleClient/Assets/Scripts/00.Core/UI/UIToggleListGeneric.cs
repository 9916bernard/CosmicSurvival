using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public partial class UIToggleListGeneric<T> where T : MonoBehaviour
{
    [SerializeField]
    private List<T> mList;

    public void SetList(List<T> InList)
    {
        if (InList == null)
        {
            return;
        }

        if (mList == null)
        {
            mList = new List<T>();
        }

        mList.AddRange(InList);
    }

    public void Set(string InName)
    {
        for (int i = 0; i < mList.Count; ++i)
        {
            if (mList[i] != null)
            {
                mList[i].gameObject.SetActive(mList[i].name == InName);
            }
        }
    }

    public void Set(int InIndex)
    {
        for (int i = 0; i < mList.Count; ++i)
        {
            if (mList[i] != null)
            {
                mList[i].gameObject.SetActive(i == InIndex);
            }
        }
    }

    public T SetAndGet(string InName)
    {
        T res = null;

        for (int i = 0; i < mList.Count; ++i)
        {
            if (mList[i].name == InName)
            {
                mList[i].gameObject.SetActive(true);
                res = mList[i];
            }
            else
            {
                mList[i].gameObject.SetActive(false);
            }
        }

        return res;
    }

    public T SetAndGet(int InIndex)
    {
        T res = null;

        for (int i = 0; i < mList.Count; ++i)
        {
            if (i == InIndex)
            {
                mList[i].gameObject.SetActive(true);
                res = mList[i];
            }
            else
            {
                mList[i].gameObject.SetActive(false);
            }
        }

        return res;
    }

    public T Get(int InIndex)
    {
        if (InIndex < 0 || InIndex >= mList.Count)
        {
            return null;
        }

        return mList[InIndex];
    }

    public T Get(string InName)
    {
        for (int i = 0; i < mList.Count; ++i)
        {
            if (mList[i].name == InName)
            {
                return mList[i];
            }
        }

        return null;
    }

    public void SetActive(bool InActive)
    {
        for (int i = 0; i < mList.Count; ++i)
        {
            mList[i].gameObject.SetActive(InActive);
        }
    }

    public void AddItem(T InItem)
    {
        if (mList == null)
        {
            mList = new List<T>();
        }

        mList.Add(InItem);
    }

    public int GetCount()
    {
        return mList.Count;
    }
}
