using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEditor;

public class switchCamera : MonoBehaviour
{
    public GameObject camera1;
    public GameObject camera2;
    public GameObject OGCamera;
    //public int manager;
    public InputActionProperty switchCam;
    public InputActionProperty switchCam2;
    public InputActionProperty switchCamOG;
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

    }

    void Update()
    {
        if (switchCam.action.WasPressedThisFrame())
        {
            cam_1();
        }

        if (switchCam2.action.WasPressedThisFrame())
        {
            cam_2();
        }

        if (switchCamOG.action.WasPressedThisFrame())
        {
            cam_og();
        }
    }

    public void cam_og()
    {
        OGCamera.SetActive(true);
        camera1.SetActive(false);
        camera2.SetActive(false);

        //gameManager.head = camera2.transform;
    }

    public void cam_2()
    {
        camera2.SetActive(true);
        camera1.SetActive(false);
        OGCamera.SetActive(false);

        //gameManager.head = camera2.transform;
    }


    public void cam_1()
    {
        camera1.SetActive(true);
        camera2.SetActive(false);
        OGCamera.SetActive(false);


    }
}
