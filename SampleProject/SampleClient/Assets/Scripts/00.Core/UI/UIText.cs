using System;
using System.Collections.Generic;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(CanvasRenderer))]
    /// <summary>
    /// The default Graphic to draw font data to screen.
    /// </summary>
    public class UIText : Text
    {
        [SerializeField]
        protected int _TextID = 0;

        public int GetTextID()
        {
            return _TextID;
        }

        public void RefreshText()
        {
            if (_TextID > 0)
            {
                text = UIM.Inst().GetText(_TextID);
            }
        }

        protected override void Start()
        {
            base.Start();

#if UNITY_EDITOR
            if (Application.isPlaying == true)
            {
                RefreshText();
            }
#else
            RefreshText();
#endif
        }

        public void OnClick_SetProperty()
        {
            verticalOverflow = VerticalWrapMode.Overflow;

            raycastTarget = false;
        }
    }
}