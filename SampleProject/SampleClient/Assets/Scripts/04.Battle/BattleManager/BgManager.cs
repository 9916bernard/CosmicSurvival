using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgManager : MonoBehaviour
{
    private List<BackgroundUnit> _BackgroundUnitList = null;

    [HideInInspector] public GameObject[] bgs; // Assign the bg panels in the Inspector
    private GameObject player; // Assign the player in the Inspector
    public Camera mainCamera; // Assign the main camera in the Inspector

    public int rowNumber; // Set the number of rows
    public int columnNumber; // Set the number of columns

    private int panelWidth = 10;
    private int panelHeight = 10;
    private int lastPanelX = 0;
    private int lastPanelY = 0;

    private BackgroundUnit currentBgUnit = null;
    private bool isStart = true;

    [SerializeField] private BattleManager battleManager = null;

    public void Init()
    {
        lastPanelX = Mathf.FloorToInt(battleManager._Field._Player.transform.position.x / panelWidth);
        lastPanelY = Mathf.FloorToInt(battleManager._Field._Player.transform.position.y / panelHeight);
    }

    private void Update()
    {
        if (_BackgroundUnitList == null || battleManager._Field._Player == null)
        {
            return;
        }

        if (isStart == true)
        {
            currentBgUnit = _BackgroundUnitList[4];
            isStart = false;
        }

        Vector3 playerPos = battleManager._Field._Player.transform.position;
        MoveCamera(playerPos);

        // Determine the current panel the user is in
        int currentPanelX = Mathf.FloorToInt(playerPos.x / panelWidth);
        int currentPanelY = Mathf.FloorToInt(playerPos.y / panelHeight);

        if (lastPanelX != currentPanelX)
        {
            int nextIndex = 0;

            if (lastPanelX < currentPanelX)
            {
                nextIndex = currentBgUnit.IndexLeft;
            }
            else
            {
                nextIndex = currentBgUnit.IndexRight + (columnNumber - 3);
            }

            Vector2 pos = currentBgUnit.Pos;

            pos.x += (lastPanelX < currentPanelX ? panelWidth * (columnNumber - 1) : -panelWidth * 2);

            _BackgroundUnitList[nextIndex].MoveToPosition(pos);

            var nextUnit = _BackgroundUnitList[nextIndex];

            pos = nextUnit.Pos;
            pos.y += panelHeight;
            _BackgroundUnitList[nextUnit.IndexUp].MoveToPosition(pos);

            pos = nextUnit.Pos;
            pos.y -= panelHeight;
            _BackgroundUnitList[nextUnit.IndexDown].MoveToPosition(pos);

            if (lastPanelX < currentPanelX)
            {
                currentBgUnit = _BackgroundUnitList[currentBgUnit.IndexRight];
            }
            else
            {
                currentBgUnit = _BackgroundUnitList[currentBgUnit.IndexLeft];
            }
            lastPanelX = currentPanelX;
        }

        if (lastPanelY != currentPanelY)
        {
            int nextIndex = 0;

            if (lastPanelY < currentPanelY)
            {
                nextIndex = currentBgUnit.IndexDown;
            }
            else
            {
                nextIndex = currentBgUnit.IndexUp;
            }

            Vector2 pos = currentBgUnit.Pos;

            pos.y += (lastPanelY < currentPanelY ? panelHeight * 2 : -panelHeight * 2);

            _BackgroundUnitList[nextIndex].MoveToPosition(pos);

            var nextUnit = _BackgroundUnitList[nextIndex];

            pos = nextUnit.Pos;
            pos.x += panelWidth;
            _BackgroundUnitList[nextUnit.IndexRight].MoveToPosition(pos);

            pos = nextUnit.Pos;
            pos.x -= panelWidth;
            _BackgroundUnitList[nextUnit.IndexLeft].MoveToPosition(pos);

            if (lastPanelY < currentPanelY)
            {
                currentBgUnit = _BackgroundUnitList[currentBgUnit.IndexUp];
            }
            else
            {
                currentBgUnit = _BackgroundUnitList[currentBgUnit.IndexDown];
            }
            lastPanelY = currentPanelY;
        }
    }

    public void SetBackgroundUnitList(List<BackgroundUnit> backgroundUnitList)
    {
        _BackgroundUnitList = backgroundUnitList;
    }

    void MoveCamera(Vector3 playerPos)
    {
        mainCamera.transform.position = new Vector3(playerPos.x, playerPos.y, mainCamera.transform.position.z);
    }

    public void MovePlayerToPosition(Vector3 targetPosition, float duration)
    {
        StartCoroutine(SmoothMovePlayer(targetPosition, duration));
    }

    private IEnumerator SmoothMovePlayer(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = battleManager._Field._Player.transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            battleManager._Field._Player.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            yield return null;
        }

        battleManager._Field._Player.transform.position = targetPosition;
        lastPanelX = Mathf.FloorToInt(targetPosition.x / panelWidth);
        lastPanelY = Mathf.FloorToInt(targetPosition.y / panelHeight);
    }

}
