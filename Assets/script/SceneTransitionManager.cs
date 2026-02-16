using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public FadeScreen fadeScreen;
    public GameObject xrorigin;
    public GameObject levels;

    public void teleportSandbox()
    {
        levels.SetActive(false);
        StartCoroutine(TeleportRoutine());
    }
    public void teleportQuiz()
    {
        StartCoroutine(TeleportRoutine());
    }
    IEnumerator TeleportRoutine()
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(fadeScreen.fadeDuration); // wiat for screen to fade completely then exit
        xrorigin.transform.position = new Vector3(.15f, 6f, -13f);
      
        //launch new scene
        fadeScreen.FadeIn();

    }
}

