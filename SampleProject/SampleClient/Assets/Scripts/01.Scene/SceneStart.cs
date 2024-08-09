using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public partial class SceneStart : MonoBehaviour
{


    void Start()
    {
        Managers.Inst().Init();
        TABLE.Inst().LoadAll(true);
        USER.Inst().MakeDefaultData();

        // Move to the next scene after Firebase initialization
        SceneManager.LoadScene("SceneMain");
    }

    
                
           
}
