using UnityEngine;

public class DestroyPartecals : MonoBehaviour
{
    public float timeToDestoy;

    void Start()
    {
        Destroy(gameObject, timeToDestoy);
    }
}
