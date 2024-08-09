//using UnityEngine;
//using UnityEngine.UI;

//public class LevelUpCanvas : MonoBehaviour
//{
//    public GameObject leftButton;
//    public GameObject middleButton;
//    public GameObject rightButton;
//    public GameObject canvas;

//    private PlayerController playerController;

//    void Start()
//    {

//        // Add listeners to the buttons
//        leftButton.GetComponent<Button>().onClick.AddListener(OnLeftButtonClick);
//        middleButton.GetComponent<Button>().onClick.AddListener(OnMiddleButtonClick);
//        rightButton.GetComponent<Button>().onClick.AddListener(OnRightButtonClick);
//    }

//    public void SetPlayerController(PlayerController controller)
//    {
//        playerController = controller;
//    }

//    public void ShowCanvas()
//    {
//        canvas.SetActive(true);
//        if (playerController != null)
//        {
//            playerController.DisableFingerStick();
//        }
//    }

//    private void HideCanvas()
//    {
//        canvas.SetActive(false);
//        if (playerController != null)
//        {
//            playerController.EnableFingerStick();
//        }
//    }

//    private void OnLeftButtonClick()
//    {
//        Debug.Log("Attack");
//        HideCanvas();
//    }

//    private void OnMiddleButtonClick()
//    {
//        Debug.Log("Movement");
//        HideCanvas();
//    }

//    private void OnRightButtonClick()
//    {
//        Debug.Log("Recover");
//        HideCanvas();
//    }
//}
