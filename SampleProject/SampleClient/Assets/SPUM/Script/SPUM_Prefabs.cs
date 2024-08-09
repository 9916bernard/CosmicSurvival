using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
public class SPUM_Prefabs : MonoBehaviour
{
    static public readonly Vector3 _shadowDefaultScale = new Vector3(0.0625f, 0.01875f, 1.0f);

    public float _version;
    public Transform _rootTrans;
    public SPUM_SpriteList _spriteOBj;
    public Transform _shadowTrans;
    public bool EditChk;
    public string _code;
    public Animator _anim;
    public bool _horse;
    public bool isRideHorse{
        get => _horse;
        set {
            _horse = value;
            UnitTypeChanged?.Invoke();
        }
    }
    public string _horseString;

    public UnityEvent UnitTypeChanged = new UnityEvent();
    private AnimationClip[] _animationClips;
    public AnimationClip[] AnimationClips => _animationClips;
    private int[] AnimHashArray = new int[(int)ETB_SPUM_ANIM.MAX];
    //private Dictionary<string, int> _nameToHashPair = new Dictionary<string, int>();
    private void InitAnimPair(){
        _animationClips = _anim.runtimeAnimatorController.animationClips;
        foreach (var clip in _animationClips)
        {
            ETB_SPUM_ANIM eAnim = (ETB_SPUM_ANIM)Enum.Parse(typeof(ETB_SPUM_ANIM), clip.name);

            int animIdx = (int)eAnim;

            AnimHashArray[animIdx] = Animator.StringToHash(clip.name);
        }
    }
    private void Awake() {
        InitAnimPair();
    }
    private void Start() {
        UnitTypeChanged.AddListener(InitAnimPair);
    }
    // 이름으로 애니메이션 실행
    public void PlayAnimation(string InAniName)
    {
        _anim.Play(Animator.StringToHash(InAniName), 0);
    }

    public void PlayAnimation(ETB_SPUM_ANIM InAnim)
    {
        _anim.Play(AnimHashArray[(int)InAnim], 0);
    }
    public void SetScale(float InScale)
    {
        _spriteOBj.transform.localScale = new Vector3(InScale, InScale, 1.0f);
        _shadowTrans.localScale = _shadowDefaultScale * InScale;
    }
}
