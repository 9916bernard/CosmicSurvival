/*
 * UIDynamicScrollViewIrregular.cs
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using DG.Tweening;

/// <summary>
/// Dynamic Scroll View Irregular
/// </summary>
[RequireComponent(typeof(ScrollRect))]
public partial class UIDynamicScrollViewIrregular : UIBehaviour
{
    public class Info
    {
        private int _ID = 0;
        private int _Height = 0;
        private int _CurrentPos = 0;
        private bool _ReserveDestroy = false;

        public Info(int InID, int InHeight)
        {
            _ID = InID;
            _Height = InHeight;
        }

        public int GetID()
        {
            return _ID;
        }

        public int GetHeight()
        {
            return _Height;
        }

        public int GetCurrentPos()
        {
            return _CurrentPos;
        }

        public bool GetReserveDestroy()
        {
            return _ReserveDestroy;
        }

        public void SetHeight(int InHeight)
        {
            _Height = InHeight;
        }

        public void SetCurrentPos(int InPos)
        {
            _CurrentPos = InPos;
        }

        public void SetReserveDestroy(bool InIsReserveDestroy)
        {
            _ReserveDestroy = InIsReserveDestroy;
        }
    }

    public class Item : MonoBehaviour
    {
        [SerializeField] protected RectTransform _Trans = null;

        private int _PoolIndex = 0;
        protected Info _Info = null;

        public LinkedListNode<Info> _LLNode = null;

        public int PoolIndex { get { return _PoolIndex; } set { _PoolIndex = value; } }

        public virtual void OnPop(Info InInfo, int InItemWidth)
        {
            _Info = InInfo;
        }

        public virtual void OnPush()
        {
            _LLNode = null;
        }

        public void SetPosition(Vector2 InPos)
        {
            _Trans.anchoredPosition = InPos;
        }

        public Info GetInfo() { return _Info; }
    }

    [SerializeField] private ScrollRect mScrollRect = null;
    [SerializeField] private RectTransform mViewportRect = null;
    [SerializeField] private RectTransform mContentRect = null;
    [SerializeField] private int mPaddingY = 0;
    [SerializeField] private ListObjectPool<Item> _Pool_Item = null;

    private LinkedList<Info> _InfoList = null;
    private LinkedList<Item> _ItemList = new();

    private int mLimitCount = 0;
    private int mItemWidth = 0;

    private Tween _Tween = null;

    public RectTransform ViewportRect { get { return mViewportRect; } }

    public void Init(int InLimitCount, int InItemWidth)
    {
        mLimitCount = InLimitCount;

        mItemWidth = InItemWidth;

        _Pool_Item.Init();
    }

    public void Make(LinkedList<Info> InList, bool InMoveLast)
    {
        ClearItem();

        mContentRect.anchoredPosition = Vector2.zero;

        _InfoList = InList;

        int totalHeight = 0;

        foreach (var info in _InfoList)
        {
            info.SetCurrentPos(-totalHeight);

            totalHeight += info.GetHeight();

            totalHeight += mPaddingY;
        }

        totalHeight -= mPaddingY;

        var size = mContentRect.GetSize();
        size.y = totalHeight;
        mContentRect.SetSize(size);

        if (InMoveLast == true && mContentRect.rect.height > mViewportRect.rect.height)
        {
            mContentRect.anchoredPosition = new Vector2(0.0f, mContentRect.rect.height - mViewportRect.rect.height);
        }

        float viewTopY = -mContentRect.anchoredPosition.y;
        float viewBottomY = viewTopY - mViewportRect.rect.height;

        LinkedListNode<Info> currentNode = _InfoList.First;
        while (currentNode != null)
        {
            var info = currentNode.Value;

            var topY = info.GetCurrentPos();
            var bottomY = info.GetCurrentPos() - info.GetHeight();

            if (bottomY > viewTopY)
            {
                currentNode = currentNode.Next;
                continue;
            }

            if (topY < viewBottomY)
                break;

            var item = _Pool_Item.Pop(info.GetID());
            item.SetPosition(new Vector2(0.0f, info.GetCurrentPos()));
            item.OnPop(info, mItemWidth);
            item._LLNode = currentNode;
            item.gameObject.SetActive(true);
            _ItemList.AddLast(item);

            currentNode = currentNode.Next;
        }
    }

    public void AddLastLimit(Info InInfo)
    {
        var infoFirst = _InfoList.First();

        infoFirst.SetReserveDestroy(true);

        float removeHeight = infoFirst.GetHeight() + mPaddingY;

        var currPos = mContentRect.anchoredPosition;
        currPos.y -= removeHeight;
        mContentRect.anchoredPosition = currPos;


        _InfoList.RemoveFirst();

        _InfoList.AddLast(InInfo);


        int totalHeight = 0;

        foreach (var info in _InfoList)
        {
            info.SetCurrentPos(-totalHeight);

            totalHeight += info.GetHeight();

            totalHeight += mPaddingY;
        }

        totalHeight -= mPaddingY;

        var size = mContentRect.GetSize();
        size.y = totalHeight;
        mContentRect.SetSize(size);

        var nodeFirst = _ItemList.First;
        if (nodeFirst != null && nodeFirst.Value.GetInfo().GetReserveDestroy() == true)
        {
            _ItemList.RemoveFirst();

            nodeFirst.Value.OnPush();
            nodeFirst.Value.gameObject.SetActive(false);
            _Pool_Item.Push(nodeFirst.Value.GetInfo().GetID(), nodeFirst.Value);
        }

        foreach (var item in _ItemList)
        {
            item.SetPosition(new Vector2(0.0f, item.GetInfo().GetCurrentPos()));
        }


        float lastPos = mContentRect.rect.height - mViewportRect.rect.height;

        if (lastPos - mContentRect.anchoredPosition.y < 400.0f)
        {
            mScrollRect.StopMovement();
            _Tween?.Kill();
            _Tween = mContentRect.DOAnchorPosY(mContentRect.rect.height - mViewportRect.rect.height, 0.35f).OnComplete(() => { _Tween = null; });
        }
        else
        {
            float viewTopY = -mContentRect.anchoredPosition.y;
            viewTopY -= removeHeight;
            float viewBottomY = viewTopY - mViewportRect.rect.height;

            LinkedListNode<Info> currentNode = _ItemList.Last.Value._LLNode.Next;
            while (currentNode != null)
            {
                var info = currentNode.Value;

                var topY = info.GetCurrentPos();

                if (topY < viewBottomY)
                    break;

                var item = _Pool_Item.Pop(info.GetID());
                item.SetPosition(new Vector2(0.0f, info.GetCurrentPos()));
                item.OnPop(info, mItemWidth);
                item._LLNode = currentNode;
                item.gameObject.SetActive(true);
                _ItemList.AddLast(item);

                currentNode = currentNode.Next;
            }
        }
    }

    public void AddLast(Info InInfo)
    {
        if (_InfoList == null)
            _InfoList = new();

        if (mLimitCount > 0 && _InfoList.Count >= mLimitCount)
        {
            AddLastLimit(InInfo);
            return;
        }

        int totalHeight = 0;

        if (_InfoList.Count > 0)
        {
            var infoLast = _InfoList.Last();

            totalHeight = infoLast.GetCurrentPos() - infoLast.GetHeight() - mPaddingY;
        }

        InInfo.SetCurrentPos(totalHeight);

        totalHeight -= InInfo.GetHeight();

        _InfoList.AddLast(InInfo);

        var size = mContentRect.GetSize();
        size.y = -totalHeight;
        mContentRect.SetSize(size);

        float viewTopY = -mContentRect.anchoredPosition.y;
        float viewBottomY = viewTopY - mViewportRect.rect.height;

        var topY = InInfo.GetCurrentPos();

        if (topY >= viewBottomY)
        {
            var item = _Pool_Item.Pop(InInfo.GetID());
            item.SetPosition(new Vector2(0.0f, InInfo.GetCurrentPos()));
            item.OnPop(InInfo, mItemWidth);
            item._LLNode = _InfoList.Last;
            item.gameObject.SetActive(true);
            _ItemList.AddLast(item);
        }

        if (mContentRect.rect.height > mViewportRect.rect.height)
        {
            float lastPos = mContentRect.rect.height - mViewportRect.rect.height;

            if (lastPos - mContentRect.anchoredPosition.y < 400.0f)
            {
                mScrollRect.StopMovement();
                _Tween?.Kill();
                _Tween = mContentRect.DOAnchorPosY(mContentRect.rect.height - mViewportRect.rect.height, 0.35f).OnComplete(()=> { _Tween = null; });
            }
        }
    }

    public void ClearItem()
    {
        LinkedListNode<Item> itemNode = _ItemList.First;

        while (itemNode != null)
        {
            itemNode.Value.OnPush();
            itemNode.Value.gameObject.SetActive(false);
            _Pool_Item.Push(itemNode.Value.GetInfo().GetID(), itemNode.Value);

            itemNode = itemNode.Next;
        }

        _ItemList.Clear();
    }



    private void Update_Top_Add(float viewTopY, float viewBottomY)
    {
        // SCROLL DOWN
        var prevNode = _ItemList.First.Value._LLNode.Previous;

        while (prevNode != null)
        {
            var info = prevNode.Value;

            var topY = info.GetCurrentPos();
            var bottomY = topY - info.GetHeight();

            if (bottomY > viewTopY)
                break;

            if (topY < viewBottomY)
            {
                prevNode = prevNode.Previous;
                continue;
            }

            var item = _Pool_Item.Pop(info.GetID());
            item.SetPosition(new Vector2(0.0f, info.GetCurrentPos()));
            item.OnPop(info, mItemWidth);
            item._LLNode = prevNode;
            item.gameObject.SetActive(true);
            _ItemList.AddFirst(item);

            //var tinfo = info as BottomChatting.ChatInfo;
            //if (prevNode.Previous != null)
            //    Debug.Log($"===== Top Add : {tinfo._Message} => View[{viewTopY}, {viewBottomY}] Prev[{prevNode.Previous.Value.GetCurrentPos()}, {prevNode.Previous.Value.GetHeight()}]");
            //else
            //    Debug.Log($"===== Top Add : {tinfo._Message} => View[{viewTopY}, {viewBottomY}]");

            prevNode = prevNode.Previous;
        }
    }

    private void Update_Top_Remove(float viewTopY, float viewBottomY)
    {
        if (_ItemList.Count <= 0)
            return;

        // SCROLL UP
        var firstNode = _ItemList.First.Value._LLNode;

        while (firstNode != null)
        {
            var info = firstNode.Value;

            var bottomY = info.GetCurrentPos() - info.GetHeight();

            if (bottomY <= viewTopY)
                break;

            var removeNode = _ItemList.First;

            _ItemList.RemoveFirst();

            removeNode.Value.OnPush();
            removeNode.Value.gameObject.SetActive(false);
            _Pool_Item.Push(info.GetID(), removeNode.Value);

            //var tinfo = info as BottomChatting.ChatInfo;
            //if (_ItemList.First != null)
            //    Debug.Log($"===== Top Remove : {tinfo._Message} => View[{viewTopY}, {viewBottomY}] First[{_ItemList.First.Value._LLNode.Value.GetCurrentPos()}, {_ItemList.First.Value._LLNode.Value.GetHeight()}]");
            //else
            //    Debug.Log($"===== Top Remove : {tinfo._Message} => View[{viewTopY}, {viewBottomY}]");

            if (_ItemList.Count <= 0)
                break;

            firstNode = _ItemList.First.Value._LLNode;
        }
    }

    private void Update_Bottom_Add(float viewTopY, float viewBottomY)
    {
        // SCROLL UP
        var nextNode = _ItemList.Last.Value._LLNode.Next;

        while (nextNode != null)
        {
            var info = nextNode.Value;

            var topY = info.GetCurrentPos();
            var bottomY = topY - info.GetHeight();

            if (topY < viewBottomY)
                break;

            if (bottomY > viewTopY)
            {
                nextNode = nextNode.Next;
                continue;
            }

            var item = _Pool_Item.Pop(info.GetID());
            item.SetPosition(new Vector2(0.0f, info.GetCurrentPos()));
            item.OnPop(info, mItemWidth);
            item._LLNode = nextNode;
            item.gameObject.SetActive(true);
            _ItemList.AddLast(item);

            //var tinfo = info as BottomChatting.ChatInfo;
            //if (nextNode.Next != null)
            //    Debug.Log($"===== Bottom Add : {tinfo._Message} => View[{viewTopY}, {viewBottomY}] Next[{nextNode.Next.Value.GetCurrentPos()}, {nextNode.Next.Value.GetHeight()}]");
            //else
            //    Debug.Log($"===== Bottom Add : {tinfo._Message} => View[{viewTopY}, {viewBottomY}]");

            nextNode = nextNode.Next;
        }
    }

    private void Update_Bottom_Remove(float viewTopY, float viewBottomY)
    {
        if (_ItemList.Count <= 0)
            return;

        // SCROLL DOWN
        var lastNode = _ItemList.Last.Value._LLNode;

        while (lastNode != null)
        {
            var info = lastNode.Value;

            var topY = info.GetCurrentPos();

            if (topY >= viewBottomY)
                break;

            var removeNode = _ItemList.Last;

            _ItemList.RemoveLast();

            removeNode.Value.OnPush();
            removeNode.Value.gameObject.SetActive(false);
            _Pool_Item.Push(info.GetID(), removeNode.Value);

            //var tinfo = info as BottomChatting.ChatInfo;
            //if (_ItemList.Last != null)
            //    Debug.Log($"===== Bottom Remove : {tinfo._Message} => View[{viewTopY}, {viewBottomY}] Last[{_ItemList.Last.Value._LLNode.Value.GetCurrentPos()}, {_ItemList.Last.Value._LLNode.Value.GetHeight()}]");
            //else
            //    Debug.Log($"===== Bottom Remove : {tinfo._Message} => View[{viewTopY}, {viewBottomY}]");

            if (_ItemList.Count <= 0)
                break;

            lastNode = _ItemList.Last.Value._LLNode;
        }
    }

    private void Update()
    {
        if (_ItemList.Count <= 0)
            return;

        var vr = mViewportRect.rect.height;
        var cr = mContentRect.rect.height;

        if (cr <= vr)
            return;

        float viewTopY = -mContentRect.anchoredPosition.y;
        float viewBottomY = viewTopY - mViewportRect.rect.height;

        // Scroll Up
        Update_Top_Remove(viewTopY, viewBottomY);
        // Scroll Down
        Update_Bottom_Remove(viewTopY, viewBottomY);

        // Scroll Down
        Update_Top_Add(viewTopY, viewBottomY);
        // Scroll Up
        Update_Bottom_Add(viewTopY, viewBottomY);
    }
}
