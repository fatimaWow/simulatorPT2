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

    public FadeScreen fadeScreen;
    public FadeScreen fadeScreenSide;
    public FadeScreen fadeScreenTop;

    bool topButtonpressed = false;
    bool sideButtonpressed = false;



    void Start()
    {
        fadeScreen.fadeDuration = 0.5f;
        fadeScreenTop.fadeDuration = 0.5f;
        fadeScreenSide.fadeDuration = 0.5f;
        ButtonTopcam.action.started += ButtonPressedTop;
        ButtonTopcam.action.canceled += ButtonReleasedTop;

        ButtonSidecam.action.started += ButtonPressedSide;
        ButtonSidecam.action.canceled += ButtonReleasedSide;

    }

    public void Fade()
    {
        StartCoroutine(fadeRoutine());
    }
    IEnumerator fadeRoutine()
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(fadeScreen.fadeDuration); // wiat for screen to fade completely then exit
        
        
        if (topButtonpressed)
        {
            topCamSwitch();
            Debug.Log("camera swithed");

            fadeScreenTop.FadeIn();
            yield return new WaitForSeconds(fadeScreenTop.fadeDuration);
        }
    }

    void ButtonPressedTop(InputAction.CallbackContext context)
    {

        topButtonpressed = true;
        Fade();
 
    }

    void topCamSwitch()
    {
        sideCam.SetActive(false);
        topCam.SetActive(true);
        OGcam.SetActive(false);
    }


    void ButtonReleasedTop(InputAction.CallbackContext context)
    {
        sideCam.SetActive(false);
        topCam.SetActive(false);
        OGcam.SetActive(true);
        topButtonpressed = false;
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
