using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using System.IO;
using Unity.VisualScripting;

public partial class DownloadBGM : MonoBehaviour
{
    public string pageUrl = "https://rejukim.tistory.com/entry/%EC%9D%B4%EC%8A%A4-%EC%9D%B4%EC%8A%A42-%EC%9D%B4%ED%84%B0%EB%84%90-OST-Ys-Ys-II-ETERNAL-Original-Sound-Track";

    public void OnClick_Download()
    {
        SendGetRequestUniTaskText(pageUrl).Forget();
    }

    public static async UniTask SendGetRequestUniTaskText(string InUrl)
    {
        var a = await UnityWebRequest.Get(InUrl).SendWebRequest();

        List<(string, string)> fileList = new();

        var b = a.downloadHandler.text.AllIndexesOf("data-src");
        foreach (var bb in b)
        {
            var start = a.downloadHandler.text.IndexOf('\"', bb);
            var end = a.downloadHandler.text.IndexOf('\"', start + 1);
            var url = a.downloadHandler.text.Substring(start + 1, end - start - 1);

            var name_start = end + 1;
            var name_end = a.downloadHandler.text.IndexOf("</a>", name_start);
            string name_name = a.downloadHandler.text.Substring(name_start + 1, name_end - name_start - 1);

            fileList.Add((url, name_name));
        }

        await SendGetRequestUniTask(fileList, Application.temporaryCachePath);
    }

    public static async UniTask SendGetRequestUniTask(List<(string, string)> InUrls, string InPath)
    {
        for (int i = 0; i < InUrls.Count; i++)
        {
            try
            {
                var a = await UnityWebRequest.Get(InUrls[i].Item1).SendWebRequest();

                int fileNum = i + 1;

                UIM.ShowToast($"down {fileNum}");

                string fileName = $"{InPath}/{InUrls[i].Item2}";

                //File.WriteAllBytes($"{InPath}/{fileNum:00}.mp3", a.downloadHandler.data);
                File.WriteAllBytes(fileName, a.downloadHandler.data);
            }
            catch (System.Exception e)
            {
            }
        }

        UIM.ShowToast("완료!!");
    }
}