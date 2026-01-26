using UnityEngine;

public class Charge : MonoBehaviour
{
    public float charge = 1f;

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
}