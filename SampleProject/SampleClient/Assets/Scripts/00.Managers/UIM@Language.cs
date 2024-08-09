using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public partial class UIM : Singleton<UIM>
{
    private EUI_LanguageType _LanguageType = EUI_LanguageType.NONE;
    private Func<int, string> _TextFuncGet = null;

    private void InitLanguageFunc(bool InIsFirst)
    {
        if (InIsFirst == true)
        {
            _LanguageType = USER.setting.data.LanguageType;

            if (_LanguageType == EUI_LanguageType.NONE)
            {
                _LanguageType = EUI_LanguageType.ENGLISH;

                switch (Application.systemLanguage)
                {
                    case SystemLanguage.Korean: _LanguageType = EUI_LanguageType.KOREAN; break;
                    case SystemLanguage.English: _LanguageType = EUI_LanguageType.ENGLISH; break;
                    case SystemLanguage.Japanese: _LanguageType = EUI_LanguageType.JAPANESE; break;
                    case SystemLanguage.ChineseTraditional: _LanguageType = EUI_LanguageType.CHINESE_TRADITIONAL; break;
                    case SystemLanguage.ChineseSimplified: _LanguageType = EUI_LanguageType.CHINESE_SIMPLIFIED; break;
                }

                USER.setting.data.LanguageType = _LanguageType;
            }
        }

        switch (_LanguageType)
        {
            case EUI_LanguageType.KOREAN: _TextFuncGet = GetText_Ko; break;
            case EUI_LanguageType.ENGLISH: _TextFuncGet = GetText_En; break;
            case EUI_LanguageType.JAPANESE: _TextFuncGet = GetText_Ja; break;
            case EUI_LanguageType.CHINESE_TRADITIONAL: _TextFuncGet = GetText_Cht; break;
            case EUI_LanguageType.CHINESE_SIMPLIFIED: _TextFuncGet = GetText_Chs; break;
        }
    }

    public void TextPreloading()
    {
        byte[] buffer = new byte[4096];

        switch (_LanguageType)
        {
            case EUI_LanguageType.KOREAN: TABLE.Inst().o_string_common_ko.Load(buffer, true); TABLE.string_common_ko = TABLE.Inst().o_string_common_ko; break;
            case EUI_LanguageType.ENGLISH: TABLE.Inst().o_string_common_en.Load(buffer, true); TABLE.string_common_en = TABLE.Inst().o_string_common_en; break;
            case EUI_LanguageType.JAPANESE: TABLE.Inst().o_string_common_ja.Load(buffer, true); TABLE.string_common_ja = TABLE.Inst().o_string_common_ja; break;
            case EUI_LanguageType.CHINESE_TRADITIONAL: TABLE.Inst().o_string_common_cht.Load(buffer, true); TABLE.string_common_cht = TABLE.Inst().o_string_common_cht; break;
            case EUI_LanguageType.CHINESE_SIMPLIFIED: TABLE.Inst().o_string_common_chs.Load(buffer, true); TABLE.string_common_chs = TABLE.Inst().o_string_common_chs; break;
        }
    }

    public EUI_LanguageType GetCurrentLanguage()
    {
        return _LanguageType;
    }

    public string GetText(int InID)
    {
        return _TextFuncGet?.Invoke(InID);
    }

    //--------------------------------------------//
    // Change Language
    //--------------------------------------------//
    public void ChangeLanguage(EUI_LanguageType InLanguageType)
    {
        if (_LanguageType == InLanguageType)
        {
            return;
        }

        _LanguageType = InLanguageType;

        USER.setting.data.LanguageType = _LanguageType;

        InitLanguageFunc(false);

        // 로딩이 된 모든 애들의 UIN_Text 찾아서 다시 세팅 해주기
        List<UIText> textList = new List<UIText>();

        foreach (var uipair in _LoadedUI)
        {
            uipair.Value.GetComponentsInChildren<UIText>(true, textList);

            foreach (var uitext in textList)
            {
                uitext.RefreshText();
            }

            textList.Clear();
        }


        List<UITextMeshProUGUI> tmpList = new List<UITextMeshProUGUI>();

        foreach (var uipair in _LoadedUI)
        {
            uipair.Value.GetComponentsInChildren<UITextMeshProUGUI>(true, tmpList);

            foreach (var uitext in tmpList)
            {
                uitext.RefreshText();
            }

            tmpList.Clear();
        }

        // 활성화 되어 있는 애들 Refresh 해주기 : 파라미터 없이 전체 리프레시 해줌
        RefreshUIForce();
    }

    //--------------------------------------------//
    // Text Func
    //--------------------------------------------//
    private string GetText_Ko(int InID)
    {
        if (TABLE.string_common_ko == null)
        {
#if UNITY_EDITOR
            return InID.ToString();
#else
            return "";
#endif
        }

        UTBString_Common_Ko_Record srec = TABLE.string_common_ko.Find(InID);

        if (srec == null)
        {
#if UNITY_EDITOR
            return InID.ToString();
#else
            return "";
#endif
        }

        if (srec.Text == null)
        {
            return "";
        }

        return srec.Text;
    }

    private string GetText_En(int InID)
    {
        if (TABLE.string_common_en == null)
        {
#if UNITY_EDITOR
            return InID.ToString();
#else
            return "";
#endif
        }

        UTBString_Common_En_Record srec = TABLE.string_common_en.Find(InID);

        if (srec == null)
        {
#if UNITY_EDITOR
            return InID.ToString();
#else
            return "";
#endif
        }

        if (srec.Text == null)
        {
            return "";
        }

        return srec.Text;
    }

    private string GetText_Ja(int InID)
    {
        if (TABLE.string_common_ja == null)
        {
#if UNITY_EDITOR
            return InID.ToString();
#else
            return "";
#endif
        }

        UTBString_Common_Ja_Record srec = TABLE.string_common_ja.Find(InID);

        if (srec == null)
        {
#if UNITY_EDITOR
            return InID.ToString();
#else
            return "";
#endif
        }

        if (srec.Text == null)
        {
            return "";
        }

        return srec.Text;
    }

    private string GetText_Cht(int InID)
    {
        if (TABLE.string_common_cht == null)
        {
#if UNITY_EDITOR
            return InID.ToString();
#else
            return "";
#endif
        }

        UTBString_Common_Cht_Record srec = TABLE.string_common_cht.Find(InID);

        if (srec == null)
        {
#if UNITY_EDITOR
            return InID.ToString();
#else
            return "";
#endif
        }

        if (srec.Text == null)
        {
            return "";
        }

        return srec.Text;
    }

    private string GetText_Chs(int InID)
    {
        if (TABLE.string_common_chs == null)
        {
#if UNITY_EDITOR
            return InID.ToString();
#else
            return "";
#endif
        }

        UTBString_Common_Chs_Record srec = TABLE.string_common_chs.Find(InID);

        if (srec == null)
        {
#if UNITY_EDITOR
            return InID.ToString();
#else
            return "";
#endif
        }

        if (srec.Text == null)
        {
            return "";
        }

        return srec.Text;
    }
}