using System.Collections.Generic;
using System;
using UnityEngine;


public class ObjectPoolSimple<T> where T : MonoBehaviour
{
    class PoolUnit
    {
        T unit = default(T);
        float lasttiem;
    }

    private Transform _RootTrans = null;
    private Func<ObjectPoolSimple<T>, T> _Factory = null;
    private int _UseCount = 0;
    private List<T> _ObjectList = new List<T>();

    public void Init(Transform InRoot, Func<ObjectPoolSimple<T>, T> InFactory, int InPreMakeCount = 0)
    {
        _RootTrans = InRoot;
        _Factory = InFactory;

        for (int i = 0; i < InPreMakeCount; ++i)
        {
            T popType = _Factory.Invoke(this);

            if (popType != null)
            {
                popType.gameObject.SetActive(false);
                popType.gameObject.transform.SetParent(_RootTrans);
                _ObjectList.Add(popType);
            }
        }
    }

    public T Pop()
    {
        T popType = null;

        if (_UseCount == _ObjectList.Count)
        {
            popType = _Factory.Invoke(this);

            if (popType == null)
            {
                return null;
            }


            popType.gameObject.transform.SetParent(_RootTrans);

            _ObjectList.Add(popType);
        }
        else
        {
            popType = _ObjectList[_UseCount];
        }

        _UseCount++;
       
        return popType;
    }

    public void Push(T InObj)
    {
        InObj.gameObject.SetActive(false);

        _UseCount--;

        if (_UseCount < 0)
        {
            _ObjectList.Add(InObj);

            _UseCount++;
        }
        else
        {
            _ObjectList[_UseCount] = InObj;
        }
    }
}


public class ObjectPool<K, T> where T : MonoBehaviour
{
    private Transform _RootTrans = null;
    private Func<K, ObjectPool<K, T>, T> _Factory = null;
    private K _Key = default;
    private int _UseCount = 0;
    private readonly List<T> _ObjectList = new();

    public K Key { get { return _Key; } }

    public int UseCount { get { return _UseCount; } }

    public void Init(Transform InRoot, Func<K, ObjectPool<K, T>, T> InFactory, K InKey, int InPreMakeCount = 0)
    {
        _RootTrans = InRoot;
        _Factory = InFactory;
        _Key = InKey;

        for (int i = 0; i < InPreMakeCount; ++i)
        {
            T popType = _Factory.Invoke(default, this);

            if (popType != null)
            {
                popType.gameObject.SetActive(false);
                popType.gameObject.transform.SetParent(_RootTrans);
                _ObjectList.Add(popType);
            }
        }
    }

    public T Pop(K InKey)
    {
        T popType = null;

        if (_UseCount == _ObjectList.Count)
        {
            popType = _Factory.Invoke(InKey, this);

            if (popType == null)
            {
                return null;
            }

            popType.gameObject.transform.SetParent(_RootTrans);
            popType.gameObject.transform.localScale = Vector3.one;

            _ObjectList.Add(popType);
        }
        else
        {
            popType = _ObjectList[_UseCount];
        }

        _UseCount++;

        return popType;
    }

    public void Push(T InObj)
    {
        InObj.gameObject.SetActive(false);

        _UseCount--;

        if (_UseCount < 0)
        {
            _ObjectList.Add(InObj);

            _UseCount++;
        }
        else
        {
            _ObjectList[_UseCount] = InObj;
        }
    }
}

public class ObjectDictionaryPool<K, T> where T : MonoBehaviour
{
    private Transform _RootTrans = null;
    private Func<K, ObjectPool<K, T>, T> _Factory = null;
    private readonly Dictionary<K, ObjectPool<K, T>> _PoolDic = new();

    public void Init(Transform InRootTrans, Func<K, ObjectPool<K, T>, T> InFactory)
    {
        _RootTrans = InRootTrans;
        _Factory = InFactory;
    }

    public T Pop(K InKey)
    {
        if (_PoolDic.TryGetValue(InKey, out ObjectPool<K, T> pool) == false)
        {
            pool = new();

            pool.Init(_RootTrans, _Factory, InKey);

            _PoolDic.Add(InKey, pool);
        }

        return pool.Pop(InKey);
    }

    public void Push(K InKey, T InObj)
    {
        if (_PoolDic.TryGetValue(InKey, out ObjectPool<K, T> pool) == false)
        {
            pool = new();

            pool.Init(_RootTrans, _Factory, InKey);

            _PoolDic.Add(InKey, pool);
        }

        pool.Push(InObj);
    }
}
























[Serializable]
public class ListObjectPoolUnit<T> where T : MonoBehaviour
{
    [SerializeField] private Transform _RootTrans = null;
    [SerializeField] private T _ProtoTypeObject = null;

    private int _PoolIndex = 0;
    private int _UseCount = 0;
    private List<T> _ObjectList = new();

    public void Init(int InPoolIndex, Transform InRootTrans, T InPrototype)
    {
        _PoolIndex = InPoolIndex;

        if (_RootTrans == null)
        {
            _RootTrans = InRootTrans;
        }

        if (_ProtoTypeObject == null)
        {
            _ProtoTypeObject = InPrototype;
        }

        if (_ProtoTypeObject != null)
        {
            _ProtoTypeObject.gameObject.SetActive(false);
        }
    }

    public T Pop()
    {
        if (_UseCount > _ObjectList.Count)
        {
            return default;
        }

        T popObj = default;

        if (_UseCount == _ObjectList.Count)
        {
            popObj = MakeNew();

            if (popObj == null)
            {
                return null;
            }

            _ObjectList.Add(popObj);
        }
        else
        {
            popObj = _ObjectList[_UseCount];
        }

        _UseCount++;

        return popObj;
    }

    public void Push(T InObj)
    {
        _UseCount--;

        if (_UseCount < 0)
        {
            _ObjectList.Add(InObj);

            _UseCount++;
        }
        else
        {
            _ObjectList[_UseCount] = InObj;
        }
    }

    private T MakeNew()
    {
        if (_RootTrans == null || _ProtoTypeObject == null)
        {
            return default;
        }

        GameObject newObj = GameObject.Instantiate(_ProtoTypeObject.gameObject, _RootTrans);

#if UNITY_EDITOR
        newObj.name = $"item_{_PoolIndex}_{_ObjectList.Count}";
#endif

        T typeObj = newObj.GetComponent<T>();

        if (typeObj == null)
        {
            typeObj = newObj.AddComponent<T>();
        }

        return typeObj;
    }
}

[Serializable]
public class ListObjectPool<T> where T : MonoBehaviour
{
    [SerializeField] private Transform _RootTrans = null;

    [SerializeField] private List<T> _ProtoTypeObject = null;

    private List<ListObjectPoolUnit<T>> _PoolList = new();


    public void Init()
    {
        for (int i = 0; i < _ProtoTypeObject.Count; ++i)
        {
            _ProtoTypeObject[i].gameObject.SetActive(false);

            if (_PoolList.Count <= i)
            {
                var pool = new ListObjectPoolUnit<T>();
                pool.Init(i, _RootTrans, _ProtoTypeObject[i]);
                _PoolList.Add(pool);
            }
        }
    }

    public T Pop(int InIndex)
    {
        if (InIndex < 0 || InIndex >= _PoolList.Count)
        {
            return null;
        }

        return _PoolList[InIndex].Pop();
    }

    public void Push(int InIndex, T InObj)
    {
        if (InIndex < 0 || InIndex >= _PoolList.Count)
        {
            return;
        }

        _PoolList[InIndex].Push(InObj);
    }
}










//[Serializable]
//public class FiledObjectPoolUnitSimple<T> where T : MonoBehaviour
//{
//    private int _UseCount = 0;
//    private readonly List<T> _ObjectList = new List<T>();

//    private T MakeNew(Transform InRootTrans)
//    {
//        GameObject newObj = new GameObject();

//        newObj.transform.parent = InRootTrans;

//        T typeObj = newObj.AddComponent<T>();

//        if (typeObj == null)
//        {
//            //Debug.Log("BaseObject not contain T Component");
//            return null;
//        }

//        newObj.AddComponent<RectTransform>();

//        return typeObj;
//    }

//    public T Pop(Transform InRootTrans)
//    {
//        if (_UseCount > _ObjectList.Count)
//        {
//            //Debug.Log("_ObjectList : Invalid _UseCount");
//            return null;
//        }

//        T popType = null;

//        if (_UseCount == _ObjectList.Count)
//        {
//            popType = MakeNew(InRootTrans);

//            if (popType == null)
//            {
//                return null;
//            }

//            _ObjectList.Add(popType);
//        }
//        else
//        {
//            popType = _ObjectList[_UseCount];
//        }

//        _UseCount++;

//        return popType;
//    }

//    public void Push(T InObj)
//    {
//        _UseCount--;

//        if (_UseCount < 0)
//        {
//            _ObjectList.Add(InObj);

//            _UseCount++;
//        }
//        else
//        {
//            _ObjectList[_UseCount] = InObj;
//        }
//    }
//}

//[Serializable]
//public class FiledObjectPoolUnit<K, T> where T : MonoBehaviour
//{
//    [SerializeField]
//    private K _Type;
//    public K Type { get { return _Type; } }

//    [SerializeField]
//    private GameObject _BaseObject = null;
//    public GameObject BaseObject { get { return _BaseObject; } }

//    private int _UseCount = 0;
//    private List<T> _ObjectList = new List<T>();

//    private T MakeNew(Transform InRootTrans)
//    {
//        if (_BaseObject == null)
//        {
//            //Debug.Log("BaseObject is empty");
//            return null;
//        }

//        GameObject newObj = GameObject.Instantiate(_BaseObject, InRootTrans);

//        T typeObj = newObj.GetComponent<T>();

//        if (typeObj == null)
//        {
//            //Debug.Log("BaseObject not contain T Component");
//            return null;
//        }

//        return typeObj;
//    }

//    public T Pop(Transform InRootTrans)
//    {
//        if (_UseCount > _ObjectList.Count)
//        {
//            //Debug.Log("_ObjectList : Invalid _UseCount");
//            return null;
//        }

//        T popType = null;

//        if (_UseCount == _ObjectList.Count)
//        {
//            popType = MakeNew(InRootTrans);

//            if (popType == null)
//            {
//                return null;
//            }

//            _ObjectList.Add(popType);
//        }
//        else
//        {
//            popType = _ObjectList[_UseCount];
//        }

//        _UseCount++;

//        return popType;
//    }

//    public void Push(T InObj)
//    {
//        _UseCount--;

//        if (_UseCount < 0)
//        {
//            _ObjectList.Add(InObj);

//            _UseCount++;
//        }
//        else
//        {
//            _ObjectList[_UseCount] = InObj;
//        }

//        if (_BaseObject == null)
//        {
//            _BaseObject = InObj.gameObject;
//        }
//    }
//}

//[Serializable]
//public class FieldObjectListPool<K, T> where T : MonoBehaviour
//{
//    [SerializeField]
//    private List<FiledObjectPoolUnit<K, T>> _PoolList = null;

//    private Transform _RootTrans = null;

//    public bool Init(Transform InRootTrans)
//    {
//        _RootTrans = InRootTrans;

//        int enumCount = Enum.GetValues(typeof(K)).Length;

//        if (_PoolList.Count != enumCount)
//        {
//            //Debug.Log("PoolList Count not matched");
//            return false;
//        }

//        for (int i = 0; i < _PoolList.Count; ++i)
//        {
//            if (_PoolList[i].BaseObject == null)
//            {
//                return false;
//            }
//        }

//        return true;
//    }

//    public T Pop(K InKey)
//    {
//        int index = Convert.ToInt32(InKey);

//        if (index < 0 || index >= _PoolList.Count)
//        {
//            //Debug.Log("PoolList Pop : Invalid Index");
//            return null;
//        }

//        return _PoolList[index].Pop(_RootTrans);
//    }

//    public void Push(K InKey, T InObj)
//    {
//        int index = Convert.ToInt32(InKey);

//        if (index < 0 || index >= _PoolList.Count)
//        {
//            //Debug.Log("PoolList Push : Invalid Index");
//            return;
//        }

//        _PoolList[index].Push(InObj);
//    }
//}

//[Serializable]
//public class FieldObjectDictionaryPool<K, T> where T : MonoBehaviour
//{
//    private Dictionary<K, FiledObjectPoolUnit<K, T>> _PoolDic = new Dictionary<K, FiledObjectPoolUnit<K, T>>();

//    private Transform _RootTrans = null;

//    public bool Init(Transform InRootTrans)
//    {
//        _RootTrans = InRootTrans;

//        return true;
//    }

//    public void SetParent(T InObj)
//    {
//        if (InObj.transform.parent != _RootTrans)
//        {
//            InObj.transform.parent = _RootTrans;
//        }
//    }

//    public T Pop(K InKey)
//    {
//        FiledObjectPoolUnit<K, T> pool = null;

//        if (_PoolDic.TryGetValue(InKey, out pool) == false)
//        {
//            pool = new FiledObjectPoolUnit<K, T>();

//            _PoolDic.Add(InKey, pool);
//        }

//        return pool.Pop(_RootTrans);
//    }

//    public void Push(K InKey, T InObj)
//    {
//        FiledObjectPoolUnit<K, T> pool = null;

//        if (_PoolDic.TryGetValue(InKey, out pool) == false)
//        {
//            pool = new FiledObjectPoolUnit<K, T>();

//            _PoolDic.Add(InKey, pool);
//        }

//        SetParent(InObj);

//        pool.Push(InObj);
//    }
//}

//[Serializable]
//public class FieldObjectDictionaryPoolSimple<K, T> where T : MonoBehaviour
//{
//    private Dictionary<K, FiledObjectPoolUnitSimple<T>> _PoolDic = new Dictionary<K, FiledObjectPoolUnitSimple<T>>();

//    private Transform _RootTrans = null;

//    public bool Init(Transform InRootTrans)
//    {
//        _RootTrans = InRootTrans;

//        return true;
//    }

//    public T Pop(K InKey)
//    {
//        FiledObjectPoolUnitSimple<T> pool = null;

//        if (_PoolDic.TryGetValue(InKey, out pool) == false)
//        {
//            pool = new FiledObjectPoolUnitSimple<T>();

//            _PoolDic.Add(InKey, pool);
//        }

//        return pool.Pop(_RootTrans);
//    }

//    public void Push(K InKey, T InObj)
//    {
//        FiledObjectPoolUnitSimple<T> pool = null;

//        if (_PoolDic.TryGetValue(InKey, out pool) == false)
//        {
//            pool = new FiledObjectPoolUnitSimple<T>();

//            _PoolDic.Add(InKey, pool);
//        }

//        InObj.transform.SetParent(_RootTrans);

//        pool.Push(InObj);
//    }
//}