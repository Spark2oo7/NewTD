using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int money;
    public int startMoney;
    static public float time = 0f;
    static public bool fixationSelection = false;
    static public bool pause = false;
    static public bool particlesEnabled = true;
    static public int gridSize;
    public int inspectorGridSize;
    public static float timeToEnd;
    public float inspectortimeToEnd;

    void Awake()
    {
        // PlayerStats.Pause();
        PlayerStats.time = 0;
        timeToEnd = inspectortimeToEnd;
        gridSize = inspectorGridSize;
        money = startMoney;
        fixationSelection = PlayerPrefs.GetInt("fixationSelection", 0) == 1;
        particlesEnabled = PlayerPrefs.GetInt("particlesEnabled", 1) == 1;
        pause = false;
    }

    void Update()
    {
        PlayerStats.time += Time.deltaTime;
    }

    static public void Pause()
    {
        Time.timeScale = 0;
        pause = true;
    }
    
    static public void Play()
    {
        Time.timeScale = 1;
        pause = false;
    }
}
