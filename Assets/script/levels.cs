using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class levels : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public ChargeManager manager;
    List<Charge> charges;
    public GameObject midRef;
    public TMP_Text textDist;
    float distance;
    bool correct;

    void Awake()
    {
        charges = manager.charges;
    }
    public float calcdistance(Vector3 positionA, Vector3 positionB)
    {
        // 1. Calculate the vector difference between the two positions.
        Vector3 vectorDifference = positionB - positionA;

        // 2. Ignore the Y component by setting it to zero.
        vectorDifference.y = 0f;

        // 3. The distance on the XZ plane is the magnitude of this new vector.
        float distance = vectorDifference.magnitude;

        return distance;
    }

    // Update is called once per frame
    void Update()
    {
        if(charges.Count == 3)
        {
            distance = calcdistance(charges[2].gameObject.transform.position, midRef.gameObject.transform.position);
            textDist.text = Math.Round(distance,1).ToString();
        }

        if (charges.Count == 3)
        {
            if (ChargeManager.detectChange == true)
            {
                //distance = calcdistance(charges[2].gameObject.transform.position, midRef.gameObject.transform.position);
                if (distance < 5.8 && distance > 4.2 && charges[2].charge == 3)
                {
                    correct = true;
                    Debug.Log(correct);
                }
            }
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
