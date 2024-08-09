using Cysharp.Threading.Tasks;
using Firebase.Firestore;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIRankingBase : UIBase
{
    [Header("[ Bind Property ]")]
    [SerializeField] private UIFixedGridGeneric<UIRankUnit> _Fixed_Panels = null;

    [SerializeField] private UIDynamicScrollView _Scroll = null;

    [SerializeField] private UIRankUnit myRank = null;

    protected override void OnOpenStart()
    {
        

        FetchAndDisplayRanking().Forget();
    }

    protected override void OnOpenTuto()
    {
        if (!USER.player.IsTutorialCompleted("LOBBY_RANKING"))
        {
            UIM.ShowOverlay("ui_tutorial", EUI_LoadType.COMMON, new() { { "TutorialType", ETB_TUTORIAL.LOBBY_RANKING } });
            USER.player.SetTutorialCompleted("LOBBY_RANKING");
        }

        if (_Fixed_Panels == null)
        {
            Debug.LogError("_Fixed_Panels is not assigned in the inspector.");
            return;
        }

        FetchAndDisplayRanking().Forget();
    }

    private async UniTaskVoid FetchAndDisplayRanking()
    {
        if (FirebaseManager.Instance?.CurrentUser == null)
        {
            Debug.LogError("No user is signed in or FirebaseManager.Instance is null.");
            return;
        }

        try
        {
            Debug.Log("Fetching records...");
            QuerySnapshot recordsSnapshot = await FirebaseManager.Instance.Firestore.Collection("record").GetSnapshotAsync();
            List<(string username, int bestRecord, string userId)> userRecords = new List<(string, int, string)>();

            foreach (DocumentSnapshot document in recordsSnapshot.Documents)
            {
                if (document.Exists)
                {
                    string username = document.ContainsField("userName") ? document.GetValue<string>("userName") : "Unknown";
                    int bestRecord = document.ContainsField("bestRecord") ? document.GetValue<int>("bestRecord") : 0;
                    userRecords.Add((username, bestRecord, document.Id));
                }
                else
                {
                    Debug.LogWarning($"Record document not found: {document.Id}");
                }
            }

            Debug.Log($"Fetched {userRecords.Count} user records.");

            userRecords.Sort((x, y) => y.bestRecord.CompareTo(x.bestRecord));

            if (_Fixed_Panels != null)
            {
                _Fixed_Panels.Make(userRecords.Count, (index, item) =>
                {
                    var userRecord = userRecords[index];
                    item.SetRank(index + 1, userRecord.username, userRecord.bestRecord);
                });
            }
            else
            {
                Debug.LogError("_Fixed_Panels is null.");
            }

            // Display my rank
            var myUserId = FirebaseManager.Instance.CurrentUser.UserId;
            var myRecord = userRecords.Find(record => record.userId == myUserId);
            if (myRecord != default)
            {
                int myRankIndex = userRecords.IndexOf(myRecord);
                if (myRank != null)
                {
                    myRank.SetRank(myRankIndex + 1, myRecord.username, myRecord.bestRecord);
                }
                else
                {
                    Debug.LogError("myRank is not assigned in the inspector.");
                }
            }
            else
            {
                Debug.LogWarning("Current user's record not found in the ranking list.");
            }

            Debug.Log($"Displaying {userRecords.Count} user records.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to fetch and display rankings: " + ex);
        }
    }
}
