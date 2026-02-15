using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public FadeScreen fadeScreen;

    public void GoToScene(int sceneIndex)
    {
        StartCoroutine(GoToSceneRoutine(sceneIndex));
    }
    IEnumerator GoToSceneRoutine(int sceneIndex)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(fadeScreen.fadeDuration); // wiat for screen to fade completely then exit

        //launch new scene
        SceneManager.LoadScene(sceneIndex);

    }
}

