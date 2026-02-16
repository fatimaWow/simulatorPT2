using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using UnityEngine;
using UnityEngine.InputSystem;
public class rayGun : MonoBehaviour
{
    public InputActionReference shootingButton;
    public LineRenderer linePrefab;
    public Transform shootingPoint;
    public float maxLineDist = 5;
    public LayerMask layermask;
    public float lineShowTimer = .3f;

    public GameObject ufo1;
    public GameObject ufo2;

    Collider hitColliderTemp;

    public GameObject explode;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shootingButton.action.WasPressedThisFrame())
        {
            Shoot();
        }
    }

    IEnumerator shootRoutine(float waitTime)
    {
        
        yield return new WaitForSeconds(waitTime);//waitfor seconds

        hitColliderTemp.gameObject.SetActive(false);

        // Code here will execute after the wait time has passed
        Debug.Log("Action resumed after waiting. Current timestamp: " + Time.time);
    }

    public void Shoot()
    {
        Debug.Log("shoot");

        Ray ray = new Ray(shootingPoint.position, shootingPoint.forward);
        bool hasHit = Physics.Raycast(ray, out RaycastHit hit, maxLineDist, layermask);

        Vector3 endPoint = Vector3.zero;

        if (hasHit)
        {
            Collider hitCollider = hit.collider;
            hitColliderTemp = hitCollider;

            if (hitCollider.gameObject.layer == LayerMask.NameToLayer("Ufo"))
            {
                Debug.Log("hit " + hitCollider.gameObject.name);
                //  hitCollider.gameObject.transform.GetChild(0).gameObject.SetActive(true); //play exopled anim

                GameObject explodeImpact = Instantiate(explode, hit.point, Quaternion.LookRotation(hit.normal));
                StartCoroutine(shootRoutine(.7f));

                Destroy(explodeImpact, 1.5f);
            }
            endPoint = hit.point;
        }
        else
        {
            endPoint = shootingPoint.position + shootingPoint.forward * maxLineDist;

        }

        LineRenderer line = Instantiate(linePrefab);
        line.positionCount = 2;
        line.SetPosition(0,shootingPoint.position);
        line.SetPosition(1,endPoint);

        Destroy(line.gameObject, lineShowTimer);
    }
}
