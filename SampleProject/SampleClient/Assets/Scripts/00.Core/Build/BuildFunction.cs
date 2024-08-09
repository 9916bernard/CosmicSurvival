#if UNITY_EDITOR
using UnityEditor;
#endif

public class BuildFunction
{
    static void TestBuild()
    {
#if UNITY_EDITOR
        BuildPipeline.BuildPlayer(
                new[] { "Assets/Scenes/SceneStart.unity", "Assets/SceneMain.unity" },
                //"D:\\GIT\\VSLike\\SampleProject\\testBuild\\" + System.DateTime.Now.ToString() + ".apk",
                "D:\\GIT\\VSLike\\SampleProject\\testBuild\\test.apk",
                //"D:/GIT/VSLike/SampleProject/testBuild/test.apk",
                BuildTarget.Android,
                BuildOptions.None);
#endif
    }
}