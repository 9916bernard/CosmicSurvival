using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Util
{
    static public void SetActive(GameObject InObj, bool InActive)
    {
        if (InObj != null) { InObj.SetActive(InActive); }
    }

    static public void SetActive(List<GameObject> InObjList, bool InActive)
    {
        if (InObjList != null)
        {
            for (int i = 0; i < InObjList.Count; ++i)
            {
                if (InObjList[i] != null) { InObjList[i].SetActive(InActive); }
            }
        }
    }

    static public void SetActive(List<RectTransform> InTransList, bool InActive)
    {
        if (InTransList != null)
        {
            for (int i = 0; i < InTransList.Count; ++i)
            {
                if (InTransList[i] != null) { InTransList[i].gameObject.SetActive(InActive); }
            }
        }
    }

    static public void SetActive(Component InComponent, bool InActive)
    {
        if (InComponent == null) { return; }
        if (InComponent.gameObject == null) { return; }

        InComponent.gameObject.SetActive(InActive);
    }
}