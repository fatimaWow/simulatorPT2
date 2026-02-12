using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;

public class ChargeManager : MonoBehaviour
{
    public static ChargeManager Instance;

    public readonly List<Charge> charges = new();
    public bool run = false;
    bool allChargesCollide = false;
    int ad = 1;
    //
   

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

        //foreach (var c in charges)
        //{
        //    if (c.is_collide == true)
        //    {
        //        allChargesCollide = true;
        //    }
        //    else
        //    {
        //        allChargesCollide = false;
        //    }

        //}

        //if (allChargesCollide == true)
        //{
        //    run = true;
        //}

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

    public void rungame()
    {
        run = true;
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
        
    }

    public void add( float num)
    {
        var charge = charges[charges.Count - 1];
        if (charge.charge > 0 && charge.charge > 1)
        {
            charge.charge += 1;
        }
        else if(charge.charge < 0 && charge.charge < -1)
        {
            charge.charge -= 1;
        }
        
        Vector3 scale = charges[charges.Count - 1].transform.localScale;
        scale.x += num;
        scale.y += num;
        scale.z += num;
        charges[charges.Count - 1].transform.localScale = scale;
    }

    public void minus(float num)
    {

        var charge = charges[charges.Count - 1];
        if (charge.charge > 0 && charge.charge > 1)
        {
            charge.charge -= 1;
        }
        else if(charge.charge < 0 && charge.charge < -1)
        {
            charge.charge += 1;

        }

        charges[charges.Count - 1].charge -= 1;
        Vector3 scale = charges[charges.Count - 1].transform.localScale;
        scale.x += num;
        scale.y += num;
        scale.z += num;
        charges[charges.Count - 1].transform.localScale = scale;
    }

    //public void reset()
    //{
    //    for (int i = 2; i < charges.Count; i++)
    //    {
    //        charges[i].destroySelf();
    //    }

    //}



    public IReadOnlyList<Charge> Charges => charges;
}