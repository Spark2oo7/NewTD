using UnityEngine;

public class SpawnerEnemy : MonoBehaviour
{
    public GameObject[] enemyes = new GameObject[2];
    public AnimationCurve[] chancesFromTime = new AnimationCurve[2];

    private Vector3 randomPosition()
    {
        Vector3 pos = Vector3.zero;
        float value = Random.value;
        float sizeMap = PlayerStats.gridSize;
        if (value < 0.25)
        {
            pos = new Vector3(Random.Range(-sizeMap, sizeMap), sizeMap, 0f);
        }
        else if(value < 0.5)
        {
            pos = new Vector3(sizeMap, Random.Range(-sizeMap, sizeMap), 0f);
        }
        else if (value < 0.75)
        {
            pos = new Vector3(Random.Range(-sizeMap, sizeMap), -sizeMap, 0f);
        }
        else
        {
            pos = new Vector3(-sizeMap, Random.Range(-sizeMap, sizeMap), 0f);
        }

        return pos;
    }

    void Update()
    {
        for (int i = 0; i < chancesFromTime.Length; i++)
        {
            float chance = chancesFromTime[i].Evaluate(PlayerStats.time);
            if (Random.value < chance * Time.deltaTime)
            {
                Instantiate(enemyes[i], randomPosition(), transform.rotation);
            }
        }
    }
}
