using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public partial class UIStackList
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

    public void Set(int InCount)
    {
        for (int i = 0; i < mList.Count; ++i)
        {
            mList[i].SetActiveEx(i < InCount);
        }
    }

    public void InverseSet(int InCount)
    {
        for (int i = 0; i < mList.Count; ++i)
        {
            mList[i].SetActiveEx(i >= InCount);
        }
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
