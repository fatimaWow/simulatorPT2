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

    public void rotate(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
        gameObject.SetActive(true);
    }
}
