using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOUND : Singleton<SOUND>
{
    private class AudioUnit
    {
        public AudioClip _Clip = null;
        public float _LastTime = .0f;
    }

    [SerializeField] private AudioSource _SourceSfx = null;
    [SerializeField] private AudioSource _SourceBgm = null;

    // SystemSFX
    private List<AudioUnit> _SystemSfxList = new();

    // BGM
    private Dictionary<EUI_BGM, AudioUnit> _BgmDic = new();

    public override void Init()
    {
        // SFX
        string[] sfxNames = Enum.GetNames(typeof(EUI_SFX));

        _SystemSfxList.Add(new());

        for (int i = 1; i < sfxNames.Length; i++)
        {
            _SystemSfxList.Add(new() { _Clip = Resources.Load<AudioClip>($"Sound/Sfx/{sfxNames[i]}"), } );
        }

        // BGM
        _SourceBgm.loop = true;
    }

    public void PlaySfx(EUI_SFX InSFX)
    {
        if (Time.realtimeSinceStartup - _SystemSfxList[(int)InSFX]._LastTime < 0.05f)
        {
            return;
        }

        _SourceSfx.clip = _SystemSfxList[(int)InSFX]._Clip;

        _SystemSfxList[(int)InSFX]._LastTime = Time.realtimeSinceStartup;

        _SourceSfx.Play();
    }

    public static void Sfx(EUI_SFX InSFX)
    {
        Inst().PlaySfx(InSFX);
    }

    public void PlayBgm(EUI_BGM InBGM)
    {
        AudioUnit au = null;

        _BgmDic.TryGetValue(InBGM, out au);

        if (au == null)
        {
            string sss = $"Sound/Bgm/{InBGM}";

            au = new() { _Clip = Resources.Load<AudioClip>($"Sound/Bgm/{InBGM}"), };

            _BgmDic.Add(InBGM, au);
        }

        if (au != null)
        {
            _SourceBgm.clip = au._Clip;
            _SourceBgm.Play();
        }
    }

    public void StopBgm()
    {
        _SourceBgm.Stop();
    }
}