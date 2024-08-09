using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public partial class SceneTest : MonoBehaviour
{
    public class MyInfo : UIDynamicScrollViewIrregular.Info
    {
        public string _Message = "";

        public MyInfo(int InID, int InHeight) : base(InID, InHeight)
        {
        }
    }

    [SerializeField] private Canvas _Canvas = null;
    [SerializeField] private UIDynamicScrollViewIrregular _Scroll = null;
    [SerializeField] private UIText _Status = null;

    private int _UID = 0;


    void Start()
    {
        Managers.Inst().Init();
        TABLE.Inst().LoadAll(true);
        USER.Inst().MakeDefaultData();

        UIM.Inst().SetCanvas(_Canvas);

        _Scroll.Init(24, 0);

        _Status.SetText("시작 된 것임");
    }

    public void OnClick_Add(int InType)
    {
        if (InType == 100)
        {
            InType = Random.Range(0, 3);
        }

        _UID += 1;

        int itemHeight = Random.Range(100, 310);

        _Scroll.AddLast(new MyInfo(InType, itemHeight) { _Message = $"{_UID} : {itemHeight}" });

        UIM.ShowToast($"추가 타입 : {InType}");
    }
}
