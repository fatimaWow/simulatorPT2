
using UnityEngine;

public class Charge : MonoBehaviour
{
    public float charge = 1f;
    public bool is_collide = false;
    Rigidbody rb;
    private ElectrostaticGrid plane;
    private float k = 8f;
    public forceVector vec;
    public bool maxReached = false;
    public GameObject trajectory;

    //  public int charge;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        vec = GetComponentInChildren<forceVector>();
    }

    void OnEnable()
    {
        if (!ChargeManager.Instance)
            new GameObject("ChargeManager").AddComponent<ChargeManager>();

        ChargeManager.Instance.Register(this);
    }

    void OnDisable()
    {
        if (ChargeManager.Instance)
            ChargeManager.Instance.Unregister(this);
    }

    public Vector2 GetLocalXZ(Transform grid)
    {
        Vector3 local = grid.InverseTransformPoint(transform.position);
        return new Vector2(local.x, local.z);
    }

    //public void run()
    //{
    //    if (is_collide)
    //    {
           
    //        plane.UpdateField();
    //    }
    //}
    public void calcForce(Charge target)
    {
        //float product = Mathf.Abs(charge) * Mathf.Abs(target.charge);
        //float distance = Vector3.Distance(transform.position, target.transform.position);
        //float force = k*(product / Mathf.Pow(distance, 2));
        //Debug.Log("Force: " + force);

        //vec.rotate(target);
        transform.LookAt(target.transform);
        vec.vectorActive();



    }

    public void destroySelf()
    {
        Destroy(gameObject);
    }


    void OnCollisionEnter(Collision collision)
    {

       

        // chekc if charge collide with plane
        if (collision.gameObject.CompareTag("plane"))
        {
            trajectory.SetActive(false);
            Debug.Log("collision with plane");
            is_collide = true;

            if (rb == null)
            {
                Debug.LogError("Rigidbody component not found on this GameObject!");
            }

            rb.isKinematic = true;
            Debug.Log("Rigidbody is now kinematic.");


            //transform.localEulerAngles = new Vector3(0, 0, 0);

             plane = collision.collider.GetComponent<ElectrostaticGrid>();

            if (!plane) return;

            // plane.UpdateField();
         //   run();


        }
    }
}