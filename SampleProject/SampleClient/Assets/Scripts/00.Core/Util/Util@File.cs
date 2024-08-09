using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Cysharp.Threading.Tasks;
using UnityEngine;

public partial class Util
{
    public static void SaveTextFile(string InPath, string InData)
    {
        File.WriteAllText(InPath, InData);
    }


    public static string LoadTextFile(string InPath)
    {
        string data = string.Empty;

        if (File.Exists(InPath))
        {
            data = File.ReadAllText(InPath);
        }

        return data;
    }


    public static async Task<string> LoadTextFileAsync(string InPath)
    {
        Task<string> task = null;

        if (File.Exists(InPath))
        {
            task = File.ReadAllTextAsync(InPath);

            await task;

            return task.Result;
        }

        return null;
    }


    public static void SaveJsonFile<T>(string InDirectory, string InJsonName, T InData)
    {
        string jsonData = JsonConvert.SerializeObject(InData);

        string path = string.Format("{0}/{1}.json", InDirectory, InJsonName);

#if (UNITY_WEBGL && !UNITY_EDITOR)
        PlayerPrefs.SetString(path, jsonData);
#else
        SaveTextFile(path, jsonData);
#endif
    }


    public static T LoadJsonFile<T>(string InDirectory, string InJsonName) where T : new()
    {
        string path = string.Format("{0}/{1}.json", InDirectory, InJsonName);

#if (UNITY_WEBGL && !UNITY_EDITOR)
        string jsonString = PlayerPrefs.GetString(path);
#else
        string jsonString = LoadTextFile(path);
#endif

        if (!string.IsNullOrEmpty(jsonString))
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
        else
        {
            return new T();
        }
    }


    public static async UniTask<T> LoadJsonFileAsync<T>(string InDirectory, string InJsonName) where T : new()
    {
        string path = string.Format("{0}/{1}.json", InDirectory, InJsonName);

#if (UNITY_WEBGL && !UNITY_EDITOR)
        var loadTextTask = UniTask.FromResult(PlayerPrefs.GetString(path));
#else
        var loadTextTask = LoadTextFileAsync(path);
#endif

        string jsonString = await loadTextTask;

        if (!string.IsNullOrEmpty(jsonString))
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
        else
        {
            return new T();
        }
    }
}