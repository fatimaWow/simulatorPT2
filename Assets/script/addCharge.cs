using NUnit.Framework;
using System.Collections.Generic;

using UnityEngine;

public class addCharge : MonoBehaviour
{

    //player
    public GameObject xrorigin;
    public GameObject obj;
    public Transform spawn;



    public void SpawnObject()
    {

        if (obj != null && spawn != null)
        {
            spawn = xrorigin.transform;
            Vector3 vector = spawn.transform.position;
            vector.z += 4;
            vector.y += .5f;
            Instantiate(obj, vector, Quaternion.identity);
            Debug.Log("spaawnObject called");
        }
        else
        {
            Debug.Log("object null");
        }
    }


}