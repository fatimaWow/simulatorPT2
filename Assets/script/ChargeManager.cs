using UnityEngine;
using System.Collections.Generic;

public class ChargeManager : MonoBehaviour
{
    public static ChargeManager Instance;

    private readonly List<Charge> charges = new();

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

    public void Register(Charge c)
    {
        if (!charges.Contains(c))
            charges.Add(c);
    }

    public void Unregister(Charge c)
    {
        charges.Remove(c);
    }

    public IReadOnlyList<Charge> Charges => charges;
}