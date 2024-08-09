using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class ClassPoolUnit<T> where T : class, new()
{
    private int _UseCount = 0;
    private List<T> _ClassList = new List<T>();

    public int UseCount { get { return _UseCount; } }
    public int Capacity { get { return _ClassList.Count; } }

    public T Pop()
    {
        if (_UseCount > _ClassList.Count)
        {
            Debug.Log("_ClassList : Invalid _UseCount");
            return default;
        }

        T popClass = default;

        if (_UseCount == _ClassList.Count)
        {
            popClass = new T();

            _ClassList.Add(popClass);
        }
        else
        {
            popClass = _ClassList[_UseCount];

            _ClassList[_UseCount] = null;
        }

        _UseCount++;

        return popClass;
    }

    public void Push(T InClass)
    {
        _UseCount--;

        if (_UseCount < 0)
        {
            _ClassList.Add(InClass);

            _UseCount++;
        }
        else
        {
            _ClassList[_UseCount] = InClass;
        }
    }
}

public class ClassDictionaryPool<K, T> where T : class, new()
{
    private Dictionary<K, ClassPoolUnit<T>> _PoolDic = new Dictionary<K, ClassPoolUnit<T>>();

    public T Pop(K InKey)
    {
        ClassPoolUnit<T> pool = default;

        if (_PoolDic.TryGetValue(InKey, out pool) == false)
        {
            pool = new ClassPoolUnit<T>();

            _PoolDic.Add(InKey, pool);
        }

        return pool.Pop();
    }

    public void Push(K InKey, T InClass)
    {
        ClassPoolUnit<T> pool = default;

        if (_PoolDic.TryGetValue(InKey, out pool) == false)
        {
            pool = new ClassPoolUnit<T>();

            _PoolDic.Add(InKey, pool);
        }

        pool.Push(InClass);
    }
}