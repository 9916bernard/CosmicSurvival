/*
 * DynamicScrollViewEx.cs
 * 
 * from mos frame (thanks)
 * @author mos frame / https://github.com/mosframe
 * 
 */


using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public static class RectTransformEx
{
    public static Vector2 GetSize(this RectTransform self)
    {
        return self.rect.size;
    }

    public static void SetSize(this RectTransform self, Vector2 newSize)
    {

        var pivot = self.pivot;
        var dist = newSize - self.rect.size;
        self.offsetMin -= new Vector2(dist.x * pivot.x, dist.y * pivot.y);
        self.offsetMax += new Vector2(dist.x * (1f - pivot.x), dist.y * (1f - pivot.y));
    }
}


/// <summary>
/// Dynamic Scroll View
/// </summary>
[RequireComponent(typeof(ScrollRect))]
public partial class UIDynamicScrollView : UIBehaviour
{
    public class ItemPack
    {
        public List<RectTransform> items = new();

        public void Init()
        {
            items.Clear();
        }

        public void AddItem(RectTransform item)
        {
            items.Add(item);
        }
    }

    public enum Direction
    {
        Vertical,
        Horizontal,
    }


    [SerializeField] private Direction mDirection = Direction.Vertical;
    [SerializeField] private RectTransform mViewportRect = null;
    [SerializeField] private RectTransform mContentRect = null;
    [SerializeField] private RectTransform mItemPrototype = null;
    [SerializeField] private int mColumnCount = 1;
    [SerializeField] private int mAddedSeedCount = 3;


    private bool mIsMakeSeed = false;

    public Action<int, GameObject> mAction_Update = null;

    private int mPrevItemCount = 0;
    private int mCurrItemCount = 0;

    private readonly LinkedList<ItemPack> mContainers = new();

    private float mPrevAnchoredPosition = 0;
    private int mNextInsertItemNo = 0; // item index from left-top of viewport at next insert


    public void Make(int InCount, Action<int, GameObject> InAction_Update)
    {
        mAction_Update = InAction_Update;

        mCurrItemCount = InCount;

        InitDirection();

        MakeSeedData();

        if (mPrevItemCount != mCurrItemCount)
        {
            mPrevItemCount = mCurrItemCount;
            ResizeContent();
        }

        Refresh();
    }

    private void MakeSeedData()
    {
        if (mIsMakeSeed == true)
        {
            return;
        }

        mIsMakeSeed = true;

        // min column count
        if (mColumnCount < 1)
        {
            mColumnCount = 1;
        }

        // hide prototype
        mItemPrototype.gameObject.SetActive(false);

        // instantiate items
        var itemCount = (int)(mFunc_GetViewportSize() / mFunc_GetItemSize()) + mAddedSeedCount;

        float columnPosStart = 0.0f;

        if (mDirection == Direction.Vertical)
        {
            columnPosStart = -(mFunc_ColumnSize() * (mColumnCount - 1) * 0.5f);
        }
        else
        {
            columnPosStart = (mFunc_ColumnSize() * (mColumnCount - 1) * 0.5f);
        }

        for (var i = 0; i < itemCount; ++i)
        {
            var newPack = new ItemPack();

            for (var j = 0; j < mColumnCount; ++j)
            {
                var itemRect = Instantiate(mItemPrototype);

                itemRect.SetParent(mContentRect, false);

                itemRect.name = string.Format("{0}_{1}", i, j);

                if (mDirection == Direction.Vertical)
                {
                    float columnPos = columnPosStart + (mFunc_ColumnSize() * j);
                    itemRect.anchoredPosition = new Vector2(columnPos, -mFunc_GetItemSize() * i);
                }
                else
                {
                    float columnPos = columnPosStart - (mFunc_ColumnSize() * j);
                    itemRect.anchoredPosition = new Vector2(mFunc_GetItemSize() * i, columnPos);
                }

                newPack.AddItem(itemRect);

                // Remove ?
                itemRect.gameObject.SetActive(true);

                UpdateItem(i, j, itemRect.gameObject);
            }

            mContainers.AddLast(newPack);
        }


        // resize content
        //ResizeContent();

        // refresh
        //Refresh();
    }

    public void Refresh()
    {
        var index = 0;
        if (mFunc_GetContentsAnchoredPosition() != 0)
        {
            index = (int)(-mFunc_GetContentsAnchoredPosition() / mFunc_GetItemSize());
        }

        foreach (var pack in mContainers)
        {
            for (int j = 0; j < pack.items.Count; ++j)
            {
                var itemRect = pack.items[j];

                // set item position
                var pos = mFunc_GetItemSize() * index;

                if (mDirection == Direction.Vertical)
                {
                    var columnX = itemRect.anchoredPosition.x;
                    itemRect.anchoredPosition = new Vector2(columnX, -pos);
                }
                else
                {
                    var columnY = itemRect.anchoredPosition.y;
                    itemRect.anchoredPosition = new Vector2(pos, columnY);
                }

                UpdateItem(index, j, itemRect.gameObject);
            }

            ++index;
        }

        mNextInsertItemNo = index - mContainers.Count;
        mPrevAnchoredPosition = (int)(mFunc_GetContentsAnchoredPosition() / mFunc_GetItemSize()) * mFunc_GetItemSize();
    }

    private void Update()
    {
        if (mIsMakeSeed == false)
        {
            return;
        }

        if (mCurrItemCount != mPrevItemCount)
        {
            mPrevItemCount = mCurrItemCount;

            // check scroll bottom

            var isBottom = false;

            if (mFunc_GetViewportSize() - mFunc_GetContentsAnchoredPosition() >= mFunc_GetContentsSize() - mFunc_GetItemSize() * 0.5f)
            {
                isBottom = true;
            }

            ResizeContent();

            // move scroll to bottom

            if (isBottom)
            {
                mAction_SetContentsAnchoredPosition(mFunc_GetViewportSize() - mFunc_GetContentsSize());
            }

            Refresh();
        }

        // [ Scroll up ]

        while (mFunc_GetContentsAnchoredPosition() - mPrevAnchoredPosition < -mFunc_GetItemSize() * 2)
        {

            mPrevAnchoredPosition -= mFunc_GetItemSize();

            // move a first item to last

            var first = mContainers.First;
            if (first == null) break;
            var pack = first.Value;
            mContainers.RemoveFirst();
            mContainers.AddLast(pack);

            // set item position

            for (int j = 0; j < pack.items.Count; ++j)
            {
                var itemRect = pack.items[j];

                var pos = mFunc_GetItemSize() * (mContainers.Count + mNextInsertItemNo);

                if (mDirection == Direction.Vertical)
                {
                    var columnX = itemRect.anchoredPosition.x;
                    itemRect.anchoredPosition = new Vector2(columnX, -pos);
                }
                else
                {
                    var columnY = itemRect.anchoredPosition.y;
                    itemRect.anchoredPosition = new Vector2(pos, columnY);
                }

                // update item

                UpdateItem(mContainers.Count + mNextInsertItemNo, j, itemRect.gameObject);
            }

            mNextInsertItemNo++;
        }

        // [ Scroll down ]

        while (mFunc_GetContentsAnchoredPosition() - mPrevAnchoredPosition > 0)
        {

            mPrevAnchoredPosition += mFunc_GetItemSize();

            // move a last item to first

            var last = mContainers.Last;
            if (last == null) break;
            var pack = last.Value;
            mContainers.RemoveLast();
            mContainers.AddFirst(pack);

            mNextInsertItemNo--;

            // set item position

            for (int j = 0; j < pack.items.Count; ++j)
            {
                var itemRect = pack.items[j];

                var pos = mFunc_GetItemSize() * mNextInsertItemNo;

                if (mDirection == Direction.Vertical)
                {
                    var columnX = itemRect.anchoredPosition.x;
                    itemRect.anchoredPosition = new Vector2(columnX, -pos);
                }
                else
                {
                    var columnY = itemRect.anchoredPosition.y;
                    itemRect.anchoredPosition = new Vector2(pos, columnY);
                }

                // update item

                UpdateItem(mNextInsertItemNo, j, itemRect.gameObject);
            }
        }
    }

    private void ResizeContent()
    {
        var div = mCurrItemCount / mColumnCount;
        var remain = mCurrItemCount % mColumnCount;

        div += (remain > 0 ? 1 : 0);

        var size = mContentRect.GetSize();

        if (mDirection == Direction.Vertical) size.y = mFunc_GetItemSize() * div;
        else size.x = mFunc_GetItemSize() * div;
        mContentRect.SetSize(size);
    }


    private void UpdateItem(int index, int packindex, GameObject itemObj)
    {
        int realindex = (index * mColumnCount) + packindex;

        if (realindex < 0 || realindex >= (mCurrItemCount))
        {
            itemObj.SetActive(false);
        }
        else
        {
            itemObj.SetActive(true);

            mAction_Update(realindex, itemObj);
        }
    }

    public void MoveToLastPos()
    {
        GetComponent<ScrollRect>().StopMovement();

        mAction_SetContentsAnchoredPosition(mFunc_GetViewportSize() - mFunc_GetContentsSize());

        Refresh();
    }

    public void MoveToItemIndex(int itemIndex)
    {
        GetComponent<ScrollRect>().StopMovement();

        var columnIndex = itemIndex / mColumnCount;

        var div = mCurrItemCount / mColumnCount;
        var remain = mCurrItemCount % mColumnCount;

        div += (remain > 0 ? 1 : 0);

        var totalLen = mFunc_GetContentsSize();
        var itemLen = totalLen / div;
        var pos = itemLen * columnIndex;

        mAction_SetContentsAnchoredPosition(-pos);
    }

    public GameObject GetFirstItemObject()
    {
        if (mContainers.First.Value.items.Count > 0)
        {
            return mContainers.First.Value.items[0].gameObject;
        }

        return null;
    }

    public GameObject GetItemObject(int InIndex)
    {
        var index = 0;

        if (mFunc_GetContentsAnchoredPosition() != 0)
        {
            // 1
            index = (int)(-mFunc_GetContentsAnchoredPosition() / mFunc_GetItemSize());
        }

        foreach (var pack in mContainers)
        {
            if (index * mColumnCount > InIndex)
            {
                return pack.items[InIndex - (index * mColumnCount)].gameObject;
            }

            ++index;
        }

        return null;
    }


    //=======================================================================
    // Direction
    //=======================================================================
    private Func<float> mFunc_GetContentsAnchoredPosition = null;
    private Action<float> mAction_SetContentsAnchoredPosition = null;
    private Func<float> mFunc_GetContentsSize = null;
    private Func<float> mFunc_GetViewportSize = null;
    private Func<float> mFunc_GetItemSize = null;
    private Func<float> mFunc_ColumnSize = null;

    private void InitDirection()
    {
        if (mDirection == Direction.Horizontal)
        {
            mFunc_GetContentsAnchoredPosition = () => { return mContentRect.anchoredPosition.x; };
            mAction_SetContentsAnchoredPosition = (InValue) => { mContentRect.anchoredPosition = new Vector2(InValue, mContentRect.anchoredPosition.y); };
            mFunc_GetContentsSize = () => { return mContentRect.rect.width; };
            mFunc_GetViewportSize = () => { return mViewportRect.rect.width; };
            mFunc_GetItemSize = () => { return mItemPrototype.rect.width; };
            mFunc_ColumnSize = () => { return mItemPrototype.rect.height; };
        }
        else
        {
            mFunc_GetContentsAnchoredPosition = () => { return -mContentRect.anchoredPosition.y; };
            mAction_SetContentsAnchoredPosition = (InValue) => { mContentRect.anchoredPosition = new Vector2(mContentRect.anchoredPosition.x, -InValue); };
            mFunc_GetContentsSize = () => { return mContentRect.rect.height; };
            mFunc_GetViewportSize = () => { return mViewportRect.rect.height; };
            mFunc_GetItemSize = () => { return mItemPrototype.rect.height; };
            mFunc_ColumnSize = () => { return mItemPrototype.rect.width; };
        }
    }
}
