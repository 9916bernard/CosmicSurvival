using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    public class UIButton : Button
    {
        [SerializeField] protected EUI_SFX _SfxType = EUI_SFX.NONE;

        protected RectTransform _TransRoot;

        protected override void Awake()
        {
            _TransRoot = gameObject.GetComponent<RectTransform>();
        }
   

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (_SfxType != EUI_SFX.NONE)
            {
                SOUND.Sfx(_SfxType);
            }

            base.OnPointerClick(eventData);
        }

        //public override void OnPointerDown(PointerEventData eventData)
        //{
        //    Debug.Log("DOWN");
        //    base.OnPointerDown(eventData);
        //    //mRectTr.localScale = Vector3.one * 0.9f;
        //}

        //public override void OnPointerUp(PointerEventData eventData)
        //{
        //    Debug.Log("UP");
        //    base.OnPointerUp(eventData);
        //    //mRectTr.localScale = Vector3.one;
        //}

        public void OnClick_SetProperty()
        {
#if UNITY_EDITOR
            Graphic pressedImage = gameObject.GetComponentByName<Graphic>("pressed");

            if (pressedImage != null)
            {
                targetGraphic = pressedImage;
                pressedImage.color = new Color32(209, 200, 173, 255);

                Debug.LogFormat("{0}버튼의 pressed 이미지 연결을 완료했습니다", gameObject.name);
            }
            else
            {
                targetGraphic = null;

                Debug.LogFormat("{0}버튼의 pressed 이미지를 찾지 못했습니다", gameObject.name);
            }

            ColorBlock color = colors;
            color.normalColor = new Color(1f, 1f, 1f, 0f);
            color.highlightedColor = new Color(1f, 1f, 1f, 0f);
            color.pressedColor = new Color(1f, 1f, 1f, 1f);
            color.selectedColor = new Color(1f, 1f, 1f, 0f);
            color.disabledColor = new Color(1f, 1f, 1f, 0f);
            color.colorMultiplier = 1f;
            color.fadeDuration = 0.05f;
            colors = color;

            Navigation navigationNone = navigation;
            navigationNone.mode = Navigation.Mode.None;
            navigation = navigationNone;
#endif
        }
    }
}