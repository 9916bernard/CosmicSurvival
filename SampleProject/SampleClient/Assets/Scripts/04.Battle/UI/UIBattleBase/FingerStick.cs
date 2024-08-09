using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.GlobalIllumination;

public class FingerStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private RectTransform _Trans_Frame = null;
    [SerializeField] private RectTransform _Trans_Stick = null;

    private Vector2 _StartPos = Vector2.zero;
    private Vector2 _FarthestPos = Vector2.zero; // Store the farthest position
    private List<Vector2> movementHistory = new List<Vector2>();
    private bool isTracking = false;

    private float totalTime = 0.0f;
    private float maxTimeSkill = 0.5f;
    private bool isFirstPress = true; // Track the first press
    private float maxDistance = 0.0f; // Track the maximum distance

    private float stepTime = 0.0f;
    private Vector2 direction = Vector2.zero; // Direction of the joystick movement
    private PlayerController player = null;

    private void Start()
    {
        _Trans_Frame.gameObject.SetActive(false);
        player = FindObjectOfType<PlayerController>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 pos = eventData.position;

        pos.y = pos.y * 1440.0f / Screen.height;
        pos.x = pos.x * 1440.0f / Screen.height;

        _StartPos = pos;

        _Trans_Stick.anchoredPosition = Vector2.zero;
        _Trans_Frame.anchoredPosition = pos;
        _Trans_Frame.gameObject.SetActive(true);

        // Start tracking the movement
        if (isFirstPress)
        {
            movementHistory.Clear();
            isTracking = true;
            totalTime = 0.0f;
            isFirstPress = false;
            pointList.Clear();
            //Debug.Log("Tracking started");

            stepTime = 0.0f;
        }
        movementHistory.Add(Vector2.zero);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _Trans_Frame.gameObject.SetActive(false);
        direction = Vector2.zero; // Reset direction when finger is lifted
        // Check if the movement history matches the skill activation pattern
        if (IsLeftToRightMovement() && isTracking)
        {
            //ActivateAvoid();
            ActivateBoost();
            // Stop tracking the movement
            isTracking = false;
            isFirstPress = true;
            totalTime = 0.0f;
            movementHistory.Clear();
        }
        //else if (IsRighToLeftMovement() && isTracking)
        //{
        //    //ActivateAvoid();
        //    ActivateBoost();
        //    // Stop tracking the movement
        //    isTracking = false;
        //    isFirstPress = true;
        //    totalTime = 0.0f;
        //    movementHistory.Clear();
        //}
        else if (IsDoubleTabMovement() && isTracking)
        {
            ActivateBoost();

            // Stop tracking the movement
            isTracking = false;
            isFirstPress = true;
            totalTime = 0.0f;
            movementHistory.Clear();
        }
    }
    List<Vector2> pointList = new();
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos = eventData.position;

        pos.y = pos.y * 1440.0f / Screen.height;
        pos.x = pos.x * 1440.0f / Screen.height;

        Vector2 dir = pos - _StartPos;

        if (dir.magnitude > 80.0f)
        {
            dir = dir.normalized * 80.0f;
        }

        _Trans_Stick.anchoredPosition = dir;
        direction = dir; // Update direction based on drag

        if (isTracking)
        {
            movementHistory.Add(dir);
        }
    }

    public Vector2 GetDirection()
    {
        return direction;
    }

    private bool IsLeftToRightMovement()
    {
        if (movementHistory.Count < 2)
            return false;

        if (movementHistory[0].x > movementHistory[movementHistory.Count - 1].x || movementHistory[0].x > 0)
        {
            return false;
        }

        for (int i = 0; i < movementHistory.Count; i++)
        {
            if (movementHistory[i].y > 40 || movementHistory[i].y < -40)
            {
                return false;
            }
        }

        return true;
    }

    private bool IsRighToLeftMovement()
    {
        if (movementHistory.Count < 2)
            return false;

        if (movementHistory[0].x < movementHistory[movementHistory.Count - 1].x || movementHistory[0].x < 0)
        {
            return false;
        }

        for (int i = 0; i < movementHistory.Count; i++)
        {
            if (movementHistory[i].y > 40 || movementHistory[i].y < -40)
            {
                return false;
            }
        }

        return true;
    }


    private bool IsDoubleTabMovement()
    {
        if (movementHistory.Count < 2)
            return false;


        for (int i = 0; i < movementHistory.Count; i++)
        {
            if (movementHistory[i].x != 0 || movementHistory[i].y != 0)
            {
                return false;
            }
        }

        return true;
    }
    // not yet implemented
    private bool UturnMovement()
    {
        if (movementHistory.Count < 2)
            return false;

        if (movementHistory[0].y < -15)
        {
            return false;
        }

        for (int i = 0; i < movementHistory.Count; i++)
        {
            if (movementHistory[i].y > 40 || movementHistory[i].y < -40)
            {
                return false;
            }
        }

        return true;
    }

    private void ActivateBoost()
    {
        Debug.Log("Boost Activated!");
        

        player.activateDash();

        // Implement skill activation logic here
    }

    public void ActivateAvoid()
    {

       Debug.Log("Avoid Activated!");

        player.SetInvincible();


    }

    private void Update()
    {
        if (isTracking)
        {
            totalTime += Time.deltaTime;

            stepTime += Time.deltaTime;
            while (stepTime > 0.05f)
            {
                pointList.Add(_Trans_Stick.anchoredPosition);
                stepTime -= 0.025f;
            }

            if (totalTime > maxTimeSkill)
            {
                movementHistory.Clear();
                isTracking = false;
                isFirstPress = true;
                totalTime = 0.0f;
                //Debug.Log("Time exceeded maxTimeSkill, reset");

                string log = "";
                for (int i = 0; i < pointList.Count; ++i)
                {
                    log += $"{pointList[i].x}, {pointList[i].y}\n";
                }
            }
        }
    }
}
