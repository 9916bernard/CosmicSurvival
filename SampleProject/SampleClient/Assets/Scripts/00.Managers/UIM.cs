using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Cysharp.Threading.Tasks;
using System.Xml.Linq;

public partial class UIM : Singleton<UIM>
{
    [SerializeField] private Vector2 _BaseResolution = new Vector2(720.0f, 1480.0f);
    [SerializeField] private float _BaseWidthRatio = 1.0f;

    private Canvas _InCanvas = null;

    private Dictionary<string, UIBase> _LoadedUI = new Dictionary<string, UIBase>();

    private UIContainer[] _Containers = new UIContainer[(int)EUI_AreaType.MAX]
    {
        new UIContainer(),
        new UIContainer(),
        new UIContainer(),
        new UIContainer()
    };

    private string[] _LoadPath = new string[(int)EUI_LoadType.MAX]
    {
        "UI/Common/{0}",    // COMMON
        "UI/Main/{0}",      // MAIN
        "UI/Ranking/{0}", // RANKING
        "UI/Upgrade/{0}",     // UPGRADE
        "UI/Collection/{0}",   // COLLECTION
        "UI/Shop/{0}",      // SHOP
        "UI/Setting/{0}",   // SETTING
        "UI/Battle/{0}",    // BATTLE
        
    };

    private UIToast _Toast = null;
    public UIToast Toast { get { return _Toast; } }

    public override void Init()
    {
        InitLanguageFunc(true);
	}

    public void SetCanvas(Canvas InCanvas)
    {
        _InCanvas = InCanvas;

		__MakeArea("area_base", _Containers[(int)EUI_AreaType.BASE]);
        __MakeArea("area_popup", _Containers[(int)EUI_AreaType.POPUP]);
        __MakeArea("area_overlay", _Containers[(int)EUI_AreaType.OVERLAY]);
        __MakeArea("area_managed", _Containers[(int)EUI_AreaType.MANAGED]);

        // 토스트 미리 만들기
        _Toast = __LoadUI("ui_toast", EUI_LoadType.COMMON) as UIToast;
        _Containers[(int)EUI_AreaType.MANAGED].Push(_Toast);
        _Toast.SetManaged();
        _Toast.SetActiveEx(false);

        __CheckResizeUniTask(0.5f).Forget();

    }

    public void SetContainerHost(EUI_AreaType InAreaType, UIBase InHost)
    {
        _Containers[(int)InAreaType].SetHost(InHost);
	}

    public void ClearCanvas()
    {
        StopAllCoroutines();

        for (int i = 0; i < _Containers.Length; i++)
        {
            _Containers[i].Clear();
        }

        _LoadedUI.Clear();

        Resources.UnloadUnusedAssets();

        _Toast = null;

        _InCanvas = null;
    }

    public UIBase ShowUI(string InName, EUI_AreaType InAreaType, EUI_LoadType InLoadType, Dictionary<string, object> InOpenParam = null)
    {
        UIBase uiObj = __LoadUI(InName, InLoadType);

        if (uiObj != null)
        {
            _Containers[(int)InAreaType].Push(uiObj);

            uiObj.Open(InAreaType, InOpenParam);
        }

        return uiObj;
    }

    public void CloseUI(EUI_AreaType InAreaType)
    {
        _Containers[(int)InAreaType].Pop();
    }

    public void RefreshUI(params EUI_RefreshType[] InRefreshTypeArray)
    {
        _Containers[(int)EUI_AreaType.BASE].RefreshActive(InRefreshTypeArray);

        _Containers[(int)EUI_AreaType.POPUP].RefreshAll(InRefreshTypeArray);
    }

    public void RefreshUIForce()
    {
        _Containers[(int)EUI_AreaType.BASE].RefreshActiveForce();

        _Containers[(int)EUI_AreaType.POPUP].RefreshAllForce();
    }

    private void Update()
    {
#if UNITY_IOS
#else
        if (Input.GetKeyUp(KeyCode.Escape) == true)
        {
            __OnBackButton();
        }
#endif
    }

	//private void OnEnable()
	//{
	//	RenderPipelineManager.beginFrameRendering += RenderPipelineManager_beginFrameRendering;
	//}

	//private void OnDisable()
	//{
	//	RenderPipelineManager.beginFrameRendering -= RenderPipelineManager_beginFrameRendering;
	//}

	//private void RenderPipelineManager_beginFrameRendering(ScriptableRenderContext context, Camera[] camera)
	//{
	//	//if (camera != Camera.main)
	//	//{
	//	//    return;
	//	//}
	//	GL.Clear(true, true, Color.blue);
	//}

    public GameObject LoadUIResource(string InName, EUI_LoadType InLoadType)
    {
        string uiPath = string.Format(_LoadPath[(int)InLoadType], InName);

        return Instantiate(Resources.Load(uiPath) as GameObject, _InCanvas.transform);
    }

    private async UniTask __CheckResizeUniTask(float InDiff)
    {
        int lastWidth = 0;
        int lastHeight = 0;

        float ratioWidth = _BaseResolution.x / _BaseResolution.y;
        float ratioHeight = _BaseResolution.y / _BaseResolution.x;

        TimeSpan waitSec = TimeSpan.FromSeconds(InDiff);
        Rect camRect = Rect.zero;

        while (true)
        {
            if (lastWidth != Screen.width || lastHeight != Screen.height)
            {
                lastWidth = Screen.width;
                lastHeight = Screen.height;

                float ratio = (float)lastWidth / (float)lastHeight;

                if (ratio < ratioWidth)
                {
                    float per = lastWidth * ratioHeight / lastHeight;
                    camRect.height = per;
                    camRect.y = (1.0f - per) * 0.5f;
                    camRect.width = 1.0f;
                    camRect.x = 0.0f;
                }
                else if (ratio > _BaseWidthRatio)
                {
                    float per = ((float)lastHeight * _BaseWidthRatio) / (float)lastWidth;
                    camRect.width = per;
                    camRect.x = (1.0f - per) * 0.5f;
                    camRect.height = 1.0f;
                    camRect.y = 0.0f;
                }
                else
                {
                    camRect.height = 1.0f;
                    camRect.y = 0.0f;
                    camRect.width = 1.0f;
                    camRect.x = 0.0f;
                }

                Camera[] camArray = FindObjectsOfType<Camera>();
                if (camArray != null)
                {
                    for (int i = 0; i < camArray.Length; i++)
                    {
                        camArray[i].rect = camRect;
                    }
                }
            }

            await UniTask.Delay(waitSec);
        }
    }

	private void __MakeArea(string InAreaName, UIContainer InContainer)
    {
        GameObject go = new GameObject(InAreaName, typeof(RectTransform));

        go.transform.SetParent(_InCanvas.transform);

        go.transform.SetAsLastSibling();

        go.transform.localScale = Vector3.one;
        go.transform.localPosition = Vector3.zero;
        go.transform.rotation = Quaternion.identity;

        InContainer.SetRoot(go);
    }

    private UIBase __LoadUI(string InName, EUI_LoadType InLoadType)
    {
        UIBase loadedUI = null;

        _LoadedUI.TryGetValue(InName, out loadedUI);

        if (loadedUI != null)
        {
            return loadedUI;
        }

        string uiPath = string.Format(_LoadPath[(int)InLoadType], InName);

        loadedUI = Instantiate(Resources.Load(uiPath) as GameObject, _InCanvas.transform).GetComponent<UIBase>();

        if (loadedUI == null)
        {
            int abc = 0;
        }

        loadedUI.name = InName;

        _LoadedUI.Add(InName, loadedUI);

        return loadedUI;
    }

    private void __OnBackButton()
    {
        if (_InCanvas == null)
        {
            return;
        }

        UIBase peekUI = _Containers[(int)EUI_AreaType.POPUP].Peek();

        if (peekUI != null)
        {
            if (peekUI.OnBackButton() == false)
            {
                peekUI.Close();
            }

            return;
        }

        UIBase hostUI = _Containers[(int)EUI_AreaType.BASE].Host();

        if (hostUI != null && hostUI.OnBackButton() == true)
        {
			return;
		}

        ShowChoice2(Inst().GetText(10002), () => {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });
    }
}