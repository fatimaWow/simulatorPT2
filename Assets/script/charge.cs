

using UnityEngine;

public class Charge : MonoBehaviour
{
    public float charge = 1f;
    public bool is_collide = false;
    Rigidbody rb;
    private ElectrostaticGrid plane;
    private float k = 8f;
    public forceVector vec;
    public GameObject vecObject;
    public bool maxReached = false;
    public GameObject trajectory;

    //  public int charge;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        vec = GetComponentInChildren<forceVector>(true);

       

        if (vec == null)
            Debug.LogError("forceVector missing on " + gameObject.name);
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
        if (target == null || vec == null) return;

        float sign = Mathf.Sign(charge * target.charge);

        Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

        //// If same sign → repel → reverse direction
        //Vector3 finalDirection = (charge > 0) ? -directionToTarget : directionToTarget;

        vec.rotate(directionToTarget);
    }

    public void destroySelf()
    {
        Destroy(gameObject);
    }


    void OnCollisionEnter(Collision collision)
    {



        if (collision.gameObject.CompareTag("trajectory"))
        {
            Debug.Log("traj collide");
            trajectory.SetActive(false);
        }

        if (collision.gameObject.CompareTag("plane"))
        {
         
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