using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class House : MonoBehaviour
{
    public Tilemap map_t;
    public Vector3Int CellPosition;

    public GameObject gameOver;
    public GameObject win;
    public GameObject UkraineWin;

    public string enemyTag;

    void Start()
    {
        CellPosition = map_t.WorldToCell(transform.position);
        InvokeRepeating("Delete", 0f, 1f);
    }

    void Update()
    {
        if (PlayerStats.time > PlayerStats.timeToEnd)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
            if (enemies.Length <= 0)
            {
                if (PlayerPrefs.GetInt("completeLevels", 0) == SceneManager.GetActiveScene().buildIndex)
                {
                    PlayerPrefs.SetInt("completeLevels", PlayerPrefs.GetInt("completeLevels", 0) + 1);
                }
                PlayerStats.Pause();
                if (LevelManager.Ukraine)
                {
                    UkraineWin.SetActive(true);
                }
                else
                {
                    win.SetActive(true);
                }
            }
        }
    }

    void Delete()
    {
        if (!map_t.GetTile(CellPosition))
        {
            // map_t.SetTile(CellPosition, null);
            PlayerStats.Pause();
            gameOver.SetActive(true);
            Destroy(gameObject);
            return;
        }
    }
}
