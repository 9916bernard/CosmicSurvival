

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITutorial : UIBase
{
    [SerializeField] private UITutorialTalkItem _TalkItem = null;


    private int _CurrIndex = 0;
    private List<UTBTutorial_Record> _TutorialList = null;

    private float _lastClick = 0.0f;

    // Start is called before the first frame update
    override protected void OnOpenStart()
    {
        var tutorialType = GetOpenParam<ETB_TUTORIAL>("TutorialType");

        _TutorialList = TABLE.tutorial.Find(tutorialType);

        if (_TutorialList == null || _TutorialList.Count <= 0)
        {
            UIM.ShowToast($"{tutorialType} : 없는 데요2?");
            Close();
            return;
        }

        _CurrIndex = 0;

        _lastClick = Time.realtimeSinceStartup;

        Play();
    }

    private void Play()
    {
        var currRec = _TutorialList[_CurrIndex];

        _TalkItem.Set(currRec.Position, currRec.Desc);
    }

    public void OnClick_Next()
    {
        if (Mathf.Abs(Time.realtimeSinceStartup - _lastClick) < 0.6f)
            return;

        _lastClick = Time.realtimeSinceStartup;

        _CurrIndex += 1;

        if (_CurrIndex >= _TutorialList.Count)
        {
            Close();
            return;
        }

        Play();
    }
}
