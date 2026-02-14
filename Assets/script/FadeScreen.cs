using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool fadeOnStart = true;
    public float fadeDuration = 2;
    public Color fadeColor;
    private Renderer rend;

    void OnEnable()
    {
        rend = GetComponent<Renderer>();
        if(fadeOnStart)
        {
            FadeIn();
        }

    }

    public void FadeIn()
    {
        Fade(1, 0);

    }

    public void FadeOut()
    {

        Fade(0, 1);
    }


    public void Fade(float alphaIn, float alphaOut)
    {
        gameObject.SetActive(true);

        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
     

    }

    public IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
       
        float timer = 0;
        while (timer <= fadeDuration)
        {







            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut,timer/fadeDuration);
            rend.material.SetColor("_Color",newColor);

            timer += Time.deltaTime; // add time
            yield return null; // once timer > fadeduration will exit loop
        }

        Color newColor2 = fadeColor;
        newColor2.a = alphaOut;

        rend.material.SetColor("_Color", newColor2);
        gameObject.SetActive(false);


    }


}
