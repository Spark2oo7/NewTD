using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class SpawnerEnemy : MonoBehaviour
{
    public GameObject[] enemyes = new GameObject[2];
    public AnimationCurve[] chancesFromTime = new AnimationCurve[2];
    public Transform listEnemys;
    public Tilemap spawnPositionsTilemap;
    private List<Vector3> spawnPositions;

    private Vector3 RandomPosition()
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
    public Vector3 GetPosition()
    {
        if (spawnPositions == null)
        {
            return RandomPosition();
        }
        else
        {
            return spawnPositions[Random.Range(0, spawnPositions.Count)];
        }
    }

    void Start()
    {
        if (spawnPositionsTilemap != null)
        {
            spawnPositions = new List<Vector3>();
            foreach (var pos in spawnPositionsTilemap.cellBounds.allPositionsWithin)
            {
                TileBase tile = spawnPositionsTilemap.GetTile(pos);
                if (tile != null)
                {
                    spawnPositions.Add(CellToWorldPosition(pos));
                }
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < chancesFromTime.Length; i++)
        {
            float chance = chancesFromTime[i].Evaluate(PlayerStats.time);
            if (Random.value < chance * Time.deltaTime)
            {
                Instantiate(enemyes[i], GetPosition(), transform.rotation, listEnemys);
            }
        }
    }

    public void LoadEnemys(SavedEnemy[] enemys)
    {
        foreach (SavedEnemy enemy in enemys)
        {
            Vector3 position = new Vector3(enemy.position.x, enemy.position.y, 0f);
            Instantiate(enemyes[0], position, transform.rotation, listEnemys);
        }
    }
    public Vector3 CellToWorldPosition(Vector3Int cellPosition)
    {
        return new Vector3(cellPosition.x, cellPosition.y, 0);
    }
}
