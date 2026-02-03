using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;

public class ChargeManager : MonoBehaviour
{
    public static ChargeManager Instance;

    private readonly List<Charge> charges = new();
    bool run = false;
    int ad = 1;

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

    //void Update()
    //{

    //    if (charges[0].is_collide == true && charges[1].is_collide == true && run == false) {
    //        rungame();
    //        run = true;
        
    //    }
               
    //}

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
        for (int i = 0; i < charges.Count; i++)
        {
            charges[i].run();
        }
        if (charges[2].charge < 0 &&  charges[3].charge > 0 || charges[3].charge < 0 && charges[2].charge > 0)
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

    public void add( float num)
    {

        
        charges[charges.Count-1].charge += 1;
        Vector3 scale = charges[charges.Count - 1].transform.localScale;
        scale.x += num;
        scale.y += num;
        scale.z += num;
        charges[charges.Count - 1].transform.localScale = scale;
    }

    public void minus(float num)
    {


        charges[charges.Count - 1].charge -= 1;
        Vector3 scale = charges[charges.Count - 1].transform.localScale;
        scale.x += num;
        scale.y += num;
        scale.z += num;
        charges[charges.Count - 1].transform.localScale = scale;
    }



    public IReadOnlyList<Charge> Charges => charges;
}