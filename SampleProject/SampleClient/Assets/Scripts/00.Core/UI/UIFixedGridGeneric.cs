using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class UIFixedGridGeneric<T> where T : MonoBehaviour
{
    [SerializeField]
    private ScrollRect mScrollRect = null;

    [SerializeField]
    private T mBaseObject = null;

    [SerializeField]
    private Transform mRootTrans = null;

    [SerializeField]
    private RectTransform mContentsTrans = null;

    [SerializeField]
    private RectTransform mEmptyTrans = null;

    [SerializeField]
    private Vector2Int mCellSize = Vector2Int.zero;

    [SerializeField]
    private Vector2Int mPadding = Vector2Int.zero;

    [SerializeField]
    private bool mHorizonCenter = false;

    [SerializeField]
    private bool mVerticalCenter = false;

    [SerializeField]
    private int mWidthCountFix = 0;

    [SerializeField]
    private int mWidthCountMin = 0;



    private List<T> mObjList = new List<T>();

    private int mCount = 0;

    private Action<int, T> mUpdateAction = null;

    private Vector2Int mTotalSize = Vector2Int.zero;
    public Vector2Int TotalSize { get { return mTotalSize; } }
    public Vector2 Padding { get { return mPadding; } }



    public void Make(int InCount, Action<int, T> InUpdateAction, bool InStartActive = true)
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
            int childCount = mRootTrans.childCount;

            if (mEmptyTrans != null)
            {
                --childCount;
            }

            if (childCount <= 1)
            {
                mObjList.Add(mBaseObject);
            }
            else
            {
                for (int i = 0; i < childCount; i++)
                {
                    T obj = mRootTrans.GetChild(i).transform.GetComponent<T>();

                    if (obj == mEmptyTrans)
                    {
                        continue;
                    }

                    mObjList.Add(obj);
                }
            }
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

        Refresh(InStartActive);

        SetPosition();
    }

    public T GetItemObj(int InIndex)
    {
        if (InIndex < 0 || InIndex >= mCount || InIndex >= mObjList.Count)
        {
            return null;
        }

        return mObjList[InIndex];
    }

    public void ResetScrollStatus()
    {
        if (mScrollRect == null)
        {
            return;
        }

        mScrollRect.normalizedPosition = Vector2.up;
    }

    private void SetPosition()
    {
        if (mCount <= 0)
        {
            mTotalSize = Vector2Int.zero;

            if (mEmptyTrans != null)
            {
                mEmptyTrans.gameObject.SetActive(true);

                mTotalSize.y = (int)mEmptyTrans.rect.height;
            }

            if (mContentsTrans != null)
            {
                mContentsTrans.sizeDelta = new Vector2(0.0f, mTotalSize.y);
            }

            return;
        }

        if (mEmptyTrans != null)
        {
            mEmptyTrans.gameObject.SetActive(false);
        }

        int cellWidth = mCellSize.x + mPadding.x;
        int cellHeight = mCellSize.y + mPadding.y;

        int widthCount = (int)mRootTrans.gameObject.GetComponent<RectTransform>().rect.width / cellWidth;

        if (mWidthCountFix > 0)
        {
            widthCount = mWidthCountFix;
        }
        else if (mWidthCountMin > 0)
        {
            if (widthCount < mWidthCountMin)
            {
                widthCount = mWidthCountMin;
            }
        }

        int heightCount = mCount / widthCount;

        if (mCount % widthCount > 0)
        {
            heightCount += 1;
        }

        mTotalSize.x = (widthCount * cellWidth) - mPadding.x;
        mTotalSize.y = (heightCount * cellHeight) - mPadding.y;

        if (mContentsTrans != null)
        {
            mContentsTrans.sizeDelta = new Vector2(0.0f, mTotalSize.y);
        }

        float widthHalf = ((float)mTotalSize.x * 0.5f);
        float widthOne = (float)cellWidth;
        widthHalf -= (widthOne * 0.5f);
        widthHalf += (mPadding.x * 0.5f);
        float heightOne = (float)cellHeight;


        int idx = 0;
        Vector2 pos = Vector2.zero;
        float posY = 0.0f;

        int remainCount = mCount;

        for (int i = 0; i < heightCount; ++i)
        {
            float posX = -widthHalf;

            if (mHorizonCenter == true && remainCount < widthCount)
            {
                float centerWidth = (remainCount * cellWidth) - mPadding.x;
                float centerWidthHalf = centerWidth * 0.5f;
                centerWidthHalf -= (widthOne * 0.5f);
                centerWidthHalf += ((float)mPadding.x * 0.5f);
                posX = -centerWidthHalf;
            }

            if (mVerticalCenter == true)
            {
                float centerHeight = (-mRootTrans.GetComponent<RectTransform>().rect.height / 2) + (mCellSize.y / 2);

                int HeightHalfCount = heightCount / 2;

                if (heightCount % 2 == 0)
                {
                    int count = HeightHalfCount - i - 1;

                    float y = centerHeight;

                    y += mPadding.y / 2 + mPadding.y * count;
                    y += mCellSize.y / 2 + mCellSize.y * count;

                    posY = y;
                }
                else
                {
                    int count = HeightHalfCount - i;

                    float y = centerHeight;

                    y += mPadding.y * count;
                    y += mCellSize.y * count;

                    posY = y;
                }
            }

            for (int j = 0; j < widthCount; ++j)
            {
                RectTransform rectTrans = mObjList[idx].GetComponent<RectTransform>();

                pos.x = posX;
                pos.y = posY;

                rectTrans.anchoredPosition = pos;

                posX += widthOne;

                idx++;

                if (idx >= mCount)
                {
                    break;
                }
            }

            posY -= heightOne;

            remainCount -= widthCount;
        }
    }

    public void Refresh(bool InActive = true)
    {
        for (int i = 0; i < mObjList.Count; ++i)
        {
            if (i < mCount)
            {
                mObjList[i].gameObject.SetActive(InActive);
                mUpdateAction?.Invoke(i, mObjList[i]);
            }
            else
            {
                mObjList[i].gameObject.SetActive(false);
            }
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

    public void SetActive(bool InActive)
    {
        mRootTrans.gameObject.SetActiveEx(InActive);
    }
}
