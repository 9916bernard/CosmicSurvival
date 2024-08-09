using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public partial class UIFixedListGeneric<T> where T : MonoBehaviour
{
    [SerializeField] private ScrollRect mScrollRect = null;
    [SerializeField] private T mBaseObject = null;
    [SerializeField] private Transform mRootTrans = null;
    [SerializeField] private RectTransform mContentsTrans = null;
    [SerializeField] private RectTransform mFocusicngTrans = null;
    [SerializeField] private Vector2Int mCellSize = Vector2Int.zero;
    [SerializeField] private float mPadding = 0.0f;

    private List<T> mObjList = new List<T>();

    private int mCount = 0;

    private Action<int, T> mUpdateAction = null;



    public void Make(int InCount, Action<int, T> InUpdateAction)
    {
        if (mBaseObject == null)
        {
            return;
        }

        if (mRootTrans == null)
        {
            return;
        }

        mBaseObject.SetActiveEx(false);

        if (mObjList.Count == 0)
        {
            mObjList.Add(mBaseObject);
        }

        if (mObjList.Count < InCount)
        {
            int addCount = InCount - mObjList.Count;

            for (int i = 0; i < addCount; ++i)
            {
                T newObj = GameObject.Instantiate(mBaseObject, mRootTrans);

                mObjList.Add(newObj);
            }
        }

        mCount = InCount;

        mUpdateAction = InUpdateAction;

        Refresh();
    }

    private void SetContentRectSize()
    {
        float totalX = 0;

        int activeCount = 0;

        foreach (var item in mObjList)
        {
            if (item.gameObject.activeSelf == true) activeCount++;
        }

        int cellsSizeSum = mCellSize.x * activeCount;

        float paddingSum = mPadding * (activeCount - 1);

        totalX = cellsSizeSum + paddingSum;

        mContentsTrans.sizeDelta = new Vector2(totalX, mContentsTrans.sizeDelta.y);
    }

    public void SetCount(int InCount)
    {
        mCount = InCount;
    }

    public void ResetScrollStatus()
    {
        if (mScrollRect == null)
        {
            return;
        }

        mScrollRect.normalizedPosition = Vector2.up;
    }

    public void Refresh()
    {
        for (int i = 0; i < mObjList.Count; ++i)
        {
            if (i < mCount)
            {
                mObjList[i].gameObject.SetActive(true);
                mUpdateAction?.Invoke(i, mObjList[i]);
            }
            else
            {
                mObjList[i].gameObject.SetActive(false);
            }
        }

        if (mContentsTrans != null)
        {
            SetContentRectSize();
        }
    }

    public void RefreshOnly()
    {
        if (mCount > mObjList.Count)
        {
            return;
        }

        for (int i = 0; i < mCount; ++i)
        {
            mUpdateAction?.Invoke(i, mObjList[i]);
        }
    }

    public void Execute(Action<int, T> InAction)
    {
        for (int i = 0; i < mCount; ++i)
        {
            InAction(i, mObjList[i]);
        }
    }

    public T GetItem(int InIndex)
    {
        if (InIndex < 0 || InIndex >= mCount)
        {
            return null;
        }

        return mObjList[InIndex];
    }


    public void SetPadding(float InPaddingValue)
    {
        mPadding = InPaddingValue;
    }

    public void FocusingToIndex(int InItemIndex)
    {
        if (mFocusicngTrans == null)
        {
            return;
        }

        if (InItemIndex >= mObjList.Count)
        {
            return;
        }

        if (InItemIndex < 0)
        {
            return;
        }

        float transPos = 0;

        if (InItemIndex - 3 < 0)
        {
            mFocusicngTrans.anchoredPosition = new Vector2(mFocusicngTrans.anchoredPosition.x, 0);

            return;
        }

        if (InItemIndex + 3 >= mObjList.Count)
        {
            transPos += mCellSize.y * mObjList.Count - 3;
            transPos += mPadding * mObjList.Count - 4;

            mFocusicngTrans.anchoredPosition = new Vector2(mFocusicngTrans.anchoredPosition.x, transPos);

            return;
        }

        transPos += mCellSize.y * (InItemIndex - 2);

        transPos += mPadding * (InItemIndex - 3);

        mFocusicngTrans.anchoredPosition = new Vector2(mFocusicngTrans.anchoredPosition.x, transPos);
    }
}
