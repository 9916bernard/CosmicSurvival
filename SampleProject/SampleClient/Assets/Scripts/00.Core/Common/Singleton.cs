using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private bool s_DontDestroyOnLoad = false;

    private static T msInstance = null;

    static public T Inst()
    {
        return msInstance;
    }

    private void Awake()
    {
        if (msInstance == null)
        {
            msInstance = this as T;
        }

        //Debug.Log($"Singleton Awake {GetType().Name}");

        if (s_DontDestroyOnLoad == true)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public virtual void Init()
    {
    }

    public virtual void RestoreStaticModule()
    {
        msInstance = this as T;
    }
}