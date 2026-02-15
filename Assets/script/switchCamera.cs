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
        //fadeScreen.fadeDuration = 0.5f;
        fadeScreenTop.fadeDuration = 0.5f;
        fadeScreenSide.fadeDuration = 0.5f;
        ButtonTopcam.action.started += ButtonPressedTop;
        ButtonTopcam.action.canceled += ButtonReleasedTop;

        ButtonSidecam.action.started += ButtonPressedSide;
        ButtonSidecam.action.canceled += ButtonReleasedSide;

    }

    public void FadeTop()
    {
        //top view
        if (topButtonpressed)
        {
            StartCoroutine(fadeRoutineOGtoTop());
        }
        else if (topButtonpressed == false)
        {
            StartCoroutine(fadeRoutineToptoOG());
        }
        


    }

    public void FadeSide()
    {
        if (sideButtonpressed)
        {
            StartCoroutine(fadeRoutineOGtoSide());
        }
        else if (sideButtonpressed == false)
        {
            StartCoroutine(fadeRoutineSidetoOG());
        }
    }

    //top view fades

    IEnumerator fadeRoutineOGtoTop()  // regular to top
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(fadeScreen.fadeDuration); // wiat for screen to fade completely then exit
        
            topCamSwitch();

            fadeScreenTop.FadeIn();
            yield return new WaitForSeconds(fadeScreenTop.fadeDuration);
        
    }

    IEnumerator fadeRoutineToptoOG()  // top veiw to regular
    {
        fadeScreenTop.FadeOut();
        yield return new WaitForSeconds(fadeScreenTop.fadeDuration); 

            ogCamSwitch();

            fadeScreen.FadeIn();
            yield return new WaitForSeconds(fadeScreen.fadeDuration);
        
    }
    
  
    //side view fades

    IEnumerator fadeRoutineOGtoSide()  // regular to side
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(fadeScreen.fadeDuration);

        sideCamSwitch();

        fadeScreenSide.FadeIn();
        yield return new WaitForSeconds(fadeScreenSide.fadeDuration);

    }

    IEnumerator fadeRoutineSidetoOG()  // side veiw to regular
    {
        Debug.Log("side to og called");
        fadeScreenSide.FadeOut();
        yield return new WaitForSeconds(fadeScreenSide.fadeDuration);

        ogCamSwitch();

        fadeScreen.FadeIn();
        yield return new WaitForSeconds(fadeScreen.fadeDuration);

    }

    //button detectionss
    void ButtonPressedTop(InputAction.CallbackContext context)
    {
        
        topButtonpressed = true;
        FadeTop();
 
    }

    void ButtonReleasedTop(InputAction.CallbackContext context)
    {
        
        topButtonpressed = false;
        FadeTop();
    }

    void ButtonPressedSide(InputAction.CallbackContext context)
    {
        sideButtonpressed = true;
        FadeSide();

    }


    void ButtonReleasedSide(InputAction.CallbackContext context)
    {
        sideButtonpressed = false;
        FadeSide();
    }

    //camera switches emethods

    void topCamSwitch()
    {
        sideCam.SetActive(false);
        topCam.SetActive(true);
        OGcam.SetActive(false);
    }

    void sideCamSwitch()
    {
        sideCam.SetActive(true);
        topCam.SetActive(false);
        OGcam.SetActive(false);
    }

    void ogCamSwitch()
    {

        sideCam.SetActive(false);
        topCam.SetActive(false);
        OGcam.SetActive(true);
    }


 


  


}
