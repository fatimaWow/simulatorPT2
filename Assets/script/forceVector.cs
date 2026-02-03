using UnityEngine;

public class forceVector : MonoBehaviour
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void vectorActive()
    {
        //var direction = (target.transform.position - transform.position).normalized;
        //var targetRotation = Quaternion.LookRotation(direction);
        //transform.rotation = targetRotation;

        gameObject.SetActive(true);
    }
}
