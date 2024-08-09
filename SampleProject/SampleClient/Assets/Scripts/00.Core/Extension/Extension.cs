using System.Collections.Generic;
using UnityEngine;

public static partial class Extension
{
    //=================================================================================
    // MonoBehaviour
    //=================================================================================
    public static void SetActiveEx(this MonoBehaviour InBehaviour, bool InActive)
    {
        if (InBehaviour != null)
        {
            InBehaviour.gameObject.SetActive(InActive);
        }
    }

    //=================================================================================
    // GameObject
    //=================================================================================
    public static void SetActiveEx(this GameObject InObj, bool InActive)
    {
        if (InObj != null)
        {
            InObj.SetActive(InActive);
        }
    }

    public static T GetComponentByName<T>(this GameObject InObject, string InName)
    {
        foreach (Transform child in InObject.transform)
        {
            if (child.name == InName)
            {
                return child.GetComponent<T>();
            }
            else
            {
                T found = GetComponentByName<T>(child.gameObject, InName);
                if (found != null)
                {
                    return found;
                }
            }
        }

        return default;
    }

    public static T GetComponentByNameToLower<T>(this GameObject InObject, string InName)
    {
        string InFindName = InName.ToLower();

        foreach (Transform child in InObject.transform)
        {
            string childName = child.name.ToLower();

            if (childName == InFindName)
            {
                return child.GetComponent<T>();
            }
            else
            {
                T found = GetComponentByName<T>(child.gameObject, InName);
                if (found != null)
                {
                    return found;
                }
            }
        }

        return default;
    }

    public static GameObject GetByName(this GameObject InObject, string InName)
    {
        foreach (Transform child in InObject.transform)
        {
            if (child.name == InName)
            {
                return child.gameObject;
            }
            else
            {
                var found = GetByName(child.gameObject, InName);
                if (found != null)
                {
                    return found;
                }
            }
        }

        return null;
    }

    //=================================================================================
    // List
    //=================================================================================
    public static void Shuffle<T>(this IList<T> InList)
    {
        System.Random rng = new System.Random();

        int n = InList.Count;

        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = InList[k];
            InList[k] = InList[n];
            InList[n] = value;
        }
    }

    // GPT
    //public static void Shuffle2<T>(this IList<T> list)
    //{
    //    for (int i = 0; i < list.Count; i++)
    //    {
    //        T temp = list[i];
    //        int randomIndex = UnityEngine.Random.Range(i, list.Count);
    //        list[i] = list[randomIndex];
    //        list[randomIndex] = temp;
    //    }
    //}

    public static T ClampLeft<T>(this IList<T> InList)
    {
        T value = InList[0];

        for (int i = 1; i < InList.Count; i++)
        {
            InList[i - 1] = InList[i];
        }

        InList.RemoveAt(InList.Count - 1);

        return value;
    }

    public static void AddFirst<T>(this IList<T> InList, T InFirst)
    {
        InList.Add(default);

        for (int i = 0; i < InList.Count - 1; i++)
        {
            InList[i + 1] = InList[i];
        }

        InList[0] = InFirst;
    }

    //=================================================================================
    // FTB_Stat
    //=================================================================================
    public static void Reset(this FTB_Stat InStat)
    {
        InStat.Attack = 0;
        InStat.AttackRate = 0;
        InStat.Defense = 0;
        InStat.DefenseRate = 0;
        InStat.Life = 0;
        InStat.LifeRate = 0;
        InStat.Speed = 0;
        InStat.CriticalRate = 0;
        InStat.CriticalDamage = 0;
        InStat.Continuous = 0;
        InStat.DrainLife = 0;
        InStat.Counter = 0;
        InStat.Stun = 0;
        InStat.Evade = 0;
    }

    public static void Add(this FTB_Stat InStat, FTB_Stat InAdd)
    {
        InStat.Attack += InAdd.Attack;
        InStat.AttackRate += InAdd.AttackRate;
        InStat.Defense += InAdd.Defense;
        InStat.DefenseRate += InAdd.DefenseRate;
        InStat.Life += InAdd.Life;
        InStat.LifeRate += InAdd.LifeRate;
        InStat.Speed += InAdd.Speed;
        InStat.CriticalRate += InAdd.CriticalRate;
        InStat.CriticalDamage += InAdd.CriticalDamage;
        InStat.Continuous += InAdd.Continuous;
        InStat.DrainLife += InAdd.DrainLife;
        InStat.Counter += InAdd.Counter;
        InStat.Stun += InAdd.Stun;
        InStat.Evade += InAdd.Evade;
    }
}