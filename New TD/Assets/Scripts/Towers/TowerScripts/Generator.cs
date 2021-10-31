using UnityEngine;

public class Generator : MonoBehaviour
{
    public Warehouse warehouse;
    public Export output;
    public float repeatRate = 1;

    void Start()
    {
        InvokeRepeating("Generate", 1f, repeatRate);
    }

    void Generate()
    {
        warehouse.Give(output.resource, output.count, false);
    }
}
