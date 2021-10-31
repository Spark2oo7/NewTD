using UnityEngine;
using System.Collections.Generic;

public class MoveResource : MonoBehaviour
{
    public Resource resource;
    public Warehouse targetWarehouse;
    public SpriteRenderer spriteRenderer;
    private List<Vector3> dots;
    public float stepTime = 1;
    private float time = 0f;
    private int current = 0;

    void Update()
    {
        time += Time.deltaTime;
        current = (int)(time / stepTime);
        if (current == dots.Count - 1)
        {
            if (targetWarehouse)
            {
                targetWarehouse.Give(resource, 1, true);
            }
            Destroy(gameObject);
            return;
        }
        transform.position = Vector3.Lerp(dots[current], dots[current+1], (time%stepTime) / stepTime);
    }

    public void SetAll(List<Vector3> dotsList, Resource res, Warehouse tw)
    {
        dots = dotsList;
        resource = res;
        targetWarehouse = tw;

        spriteRenderer.color = resource.color;
    }
}
