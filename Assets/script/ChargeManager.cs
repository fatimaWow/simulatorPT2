using System;
using System.Collections.Generic;

using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;


public class ChargeManager : MonoBehaviour
{
    public static ChargeManager Instance;

    public static List<Charge> charges = new();
    
    bool allChargesCollide = false;
    public static bool detectChange = false;
    public bool run = false;
    float distance;

    public GameObject levelObject;
    levels levelScript;
    

    void Start()
    {
        levelScript = levelObject.GetComponent<levels>();
    }



    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {

    }

    public void Register(Charge c)
    {
        if (!charges.Contains(c))
            charges.Add(c);
    }

    public void Unregister(Charge c)
    {
        charges.Remove(c);
    }

    public void rungame() //runs when play button press
    {
       
       
            if (charges.Count > 3)
            {
                if (charges[2].charge < 0 && charges[3].charge > 0 || charges[3].charge < 0 && charges[2].charge > 0)
                {
                    charges[2].calcForce(charges[3]);
                    charges[3].calcForce(charges[2]);

                }
                else
                {
                    charges[2].calcForce(charges[3]);
                    charges[3].calcForce(charges[2]);
                    charges[2].transform.Rotate(0, 180, 0);
                    charges[3].transform.Rotate(0, 180, 0);
                }
            }
            detectChange = true;
        


    }

   

    public void add( float num)
    {
        var charge = charges[charges.Count - 1];
        if (charge.charge >= 1 && charge.charge < 4) // positive 
        {
            charge.charge += 1;
            resizecharge(num);
        }
        else if(charge.charge <= -1 && charge.charge > -4) //negative
        {
            charge.charge -= 1;
            resizecharge(num);
        }
   
    }

    public void minus(float num)
    {

        var charge = charges[charges.Count - 1];
        if (charge.charge > 1)// positive 
        {
            charge.charge -= 1;
            resizecharge(num);
        }
        else if(charge.charge < -1) //negative
        {
            charge.charge += 1;
            resizecharge(num);

        }

       
        
    }

    public void resizecharge(float num)
    {
        Vector3 scale = charges[charges.Count - 1].transform.localScale;
        scale.x += num;
        scale.y += num;
        scale.z += num;
        charges[charges.Count - 1].transform.localScale = scale;
    }

    public IReadOnlyList<Charge> Charges => charges;
}