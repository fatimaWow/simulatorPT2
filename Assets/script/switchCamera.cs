using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEditor;

public class switchCamera : MonoBehaviour
{
    public GameObject sideCam;
    public GameObject topCam;
    public GameObject OGcam;
    //public int manager;
    public InputActionReference ButtonSidecam;
    public InputActionReference ButtonTopcam;

 
    // public GameMenuManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //public void manageCams()
    //  {
    //      if (manger == 0)
    //      {
    //          cam_2();
    //          manager = 1;

    //      }
    //      else
    //      {
    //          cam_1();
    //          manager = 0;
    //      }
    //  }

    //void Start()
    //{
    //    gameManager = GetComponent<GameMenuManager>();
    //}


    void Start()
    {
        ButtonTopcam.action.started += ButtonPressedTop;
        ButtonTopcam.action.canceled += ButtonReleasedTop;

        ButtonSidecam.action.started += ButtonPressedSide;
        ButtonSidecam.action.canceled += ButtonReleasedSide;

    }

    void ButtonPressedTop(InputAction.CallbackContext context)
    {
        Debug.Log("top view pressed");
        sideCam.SetActive(false);
        topCam.SetActive(true);
        OGcam.SetActive(false);
    }


    void ButtonReleasedTop(InputAction.CallbackContext context)
    {
        sideCam.SetActive(false);
        topCam.SetActive(false);
        OGcam.SetActive(true);
    }


    void ButtonPressedSide(InputAction.CallbackContext context)
    {
        sideCam.SetActive(true);
        topCam.SetActive(false);
        OGcam.SetActive(false);
    }


    void ButtonReleasedSide(InputAction.CallbackContext context)
    {
        sideCam.SetActive(false);
        topCam.SetActive(false);
        OGcam.SetActive(true);
    }


}
