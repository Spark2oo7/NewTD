using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SelectLevels : MonoBehaviour
{
    public GameObject[] openButtons = new GameObject[5];
    public GameObject[] closeButtons = new GameObject[5];

    void Start()
    {
        for (int b = 0; b <= PlayerPrefs.GetInt("completeLevels", 0); b++)
        {
            openButtons[b].SetActive(true);
        }
    }

    public void LoadLevel(int loadLevel)
    {
        SceneManager.LoadScene(loadLevel-1);
    }
}
