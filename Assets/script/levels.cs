using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;

public class levels : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public ChargeManager manager;
    List<Charge> charges;
    public GameObject testCharge;
    public TMP_Text textDist;
    public TMP_Text questionText;
    float distance;
    public ElectrostaticGrid grid;
    bool level1 = true;
    bool level2 = false;
    bool level3 = false;

    public GameObject rayGun;
    public GameObject rightControllermodel;

    public bool correct = false;



    String currQuestion = "";

    void Awake()
    {
        charges = ChargeManager.charges;
        currQuestion = "Given a radius of 5m from the test charge, what value of (Q) will give ≈ <color=#D000FF>5V voltage</color>?";
        questionText.text = currQuestion.ToString();
        //  questionText.text = "Find the correct radius and charge value to get a voltage of approx. 5.5 V".ToString();
    }
    public float calcdistance(Vector3 positionA, Vector3 positionB)
    {
      
        Vector3 vectorDifference = positionB - positionA;
        vectorDifference.y = 0f;
        float distance = vectorDifference.magnitude;

        return distance;
    }

    // Update is called once per frame
    IEnumerator DelayedLevelswitch(float waitTime)
    {
        Debug.Log("Action started at timestamp: " + Time.time);

        questionText.text = "Correct!".ToString();
   
        yield return new WaitForSeconds(waitTime);//waitfor seconds

        rayGun.SetActive(false);
        rightControllermodel.SetActive(true);

        if (level2)
        {
            Debug.Log("level 2");

            //questionText.text = "Find the right charge pair and radius to acheive an attractive force of 2N x 10^9".ToString();
            currQuestion = "Given charges +4 and -2 , find the correct radius to acheive an attractive force of ≈ <color=#D000FF>0.7N x 10^9</color>";
            questionText.text = currQuestion.ToString();

        }
        else if (level3)
        {
          
            Debug.Log("level3");
            currQuestion = "Find the righx 10^9";
            questionText.text = currQuestion.ToString();

        }
        
       
            grid.reset();

        // Code here will execute after the wait time has passed
        Debug.Log("Action resumed after waiting. Current timestamp: " + Time.time);
    }

    IEnumerator DelayedIncorrect(float waitTime)
    {
        Debug.Log("Action started at timestamp: " + Time.time);

        questionText.text = "Try Again!".ToString();

        yield return new WaitForSeconds(waitTime);//waitfor seconds
      

        questionText.text = currQuestion.ToString();

        grid.reset();

        // Code here will execute after the wait time has passed
        Debug.Log("Action resumed after waiting. Current timestamp: " + Time.time);
    }


    void Update()
    {
        if (level1)
        {
            Debug.Log("level 1 loop");
            if (charges.Count == 3)
            {
               
                distance = calcdistance(charges[2].gameObject.transform.position, testCharge.gameObject.transform.position);
                textDist.text =  Math.Round(distance).ToString();
            }

            if (charges.Count == 3)
            {
                if (ChargeManager.detectChange == true)
                {
                    //distance = calcdistance(charges[2].gameObject.transform.position, midRef.gameObject.transform.position);
                    if (distance < 5.8 && distance > 4.2 && charges[2].charge == 3)
                    {
                        textDist.text = "".ToString();
                        testCharge.SetActive(false);
                        level1 = false;
                        level2 = true;
                        StartCoroutine(DelayedLevelswitch(10.0f));

                        rayGun.SetActive(true);
                        rightControllermodel.SetActive(false);


                    }
                    else
                    {
                        StartCoroutine(DelayedIncorrect(6.0f));
                    }
                }
            }
        }

        if (level2) {

            Debug.Log("level 2 loop");
            if (charges.Count == 4)
            {
                
                distance = calcdistance(charges[2].gameObject.transform.position, charges[3].gameObject.transform.position);
                textDist.text = Math.Round(distance).ToString();

                if (charges[2].charge == -2 && charges[3].charge == 4 || charges[3].charge == -2 && charges[2].charge == 4)
                {
                    if (ChargeManager.detectChange == true)
                    {
                        if (distance > 9.2  && distance < 10.7)
                        {
                            
                            level2 = false;
                            level3 = true;
                            StartCoroutine(DelayedLevelswitch(6.0f));

                            rayGun.SetActive(true);
                            rightControllermodel.SetActive(false);

                        }
                        else
                        {
                            StartCoroutine(DelayedIncorrect(6.0f));
                        }
                    }
                } //
            }


        
        }

        if (level3)
        {
            Debug.Log("level 3 loop");

        }
           

       // if(ChargeManager.detectChange == true)
       // {
           
            //Debug.Log("distance between: " + distance);

            //float chargeMultiply = Math.Abs(charges[2].charge * midRef.charge);

           // Debug.Log("chargeMultiply: " + chargeMultiply);

          //  float final = (chargeMultiply / (float)Math.Pow(distance, 2)) * (float)8.99;
           // Debug.Log("final force: " + final);
        //}
        
    }
}
