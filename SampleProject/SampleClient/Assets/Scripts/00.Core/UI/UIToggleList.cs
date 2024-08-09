using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public partial class UIToggleList
{
    [SerializeField]
    private List<GameObject> mList;

    public void SetList(List<GameObject> InList)
    {
        if (InList == null)
        {
            return;
        }

        if (mList == null)
        {
            mList = new List<GameObject>();
        }

        mList.AddRange(InList);
    }

    public void Set(string InName)
    {
        for (int i = 0; i < mList.Count; ++i)
        {
            if (mList[i] != null)
            {
                mList[i].SetActive(mList[i].name == InName);
            }
        }
    }

    public void Set(int InIndex)
    {
        for (int i = 0; i < mList.Count; ++i)
        {
            if (mList[i] != null)
            {
                mList[i].SetActive(i == InIndex);
            }
        }
    }

    public void InverseSet(int InIndex)
    {
        for (int i = 0; i < mList.Count; ++i)
        {
            if (mList[i] != null)
            {
                mList[i].SetActive(i != InIndex);
            }
        }
    }

    public GameObject SetAndGet(string InName)
    {
        GameObject res = null;

        for (int i = 0; i < mList.Count; ++i)
        {
            if (mList[i].name == InName)
            {
                mList[i].SetActive(true);
                res = mList[i];
            }
            else
            {
                mList[i].SetActive(false);
            }
        }

        return res;
    }

    public GameObject SetAndGet(int InIndex)
    {
        GameObject res = null;

        for (int i = 0; i < mList.Count; ++i)
        {
            if (i == InIndex)
            {
                mList[i].SetActive(true);
                res = mList[i];
            }
            else
            {
                mList[i].SetActive(false);
            }
        }

        return res;
    }

    public GameObject Get(int InIndex)
    {
        if (InIndex < 0 || InIndex >= mList.Count)
        {
            return null;
        }

        return mList[InIndex];
    }

    public GameObject Get(string InName)
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
            mList[i].SetActive(InActive);
        }
    }

    public void AddItem(GameObject InItem)
    {
        if (mList == null)
        {
            mList = new List<GameObject>();
        }

        mList.Add(InItem);
    }

    public int GetCount()
    {
        return mList.Count;
    }
}
