using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;  // Make sure you have the Cysharp.Threading.Tasks namespace available for UniTask

public class Managers : Singleton<Managers>
{
    public override void Init()
    {
        // Debug.Log($"Init {GetType().Name}");

        // Initialize other managers
        USER.Inst().Init();
        RESOURCE.Inst().Init();
        UIM.Inst().Init();
        SOUND.Inst().Init();
        //GoodsCenter.Inst().Init();

        Application.targetFrameRate = 60;
    }

    public override void RestoreStaticModule()
    {
        base.RestoreStaticModule();

        TABLE.Inst().RestoreStaticModule();
        USER.Inst().RestoreStaticModule();
        RESOURCE.Inst().RestoreStaticModule();
        UIM.Inst().RestoreStaticModule();
        SOUND.Inst().RestoreStaticModule();
    }

    private void OnApplicationQuit()
    {
        SaveDataOnQuit().Forget();
    }

    private async UniTaskVoid SaveDataOnQuit()
    {
        USER.Inst().SaveData();
        // Save data to Firestore
        await FirebaseManager.Instance.SaveAllDataToFirestore();
    }
}
