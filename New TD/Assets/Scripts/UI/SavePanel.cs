using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class SavePanel : MonoBehaviour
{
    public int number;
    public LevelManager levelManager;
    public Text numberText;
    public Text modeText;
    public Text timeText;
    public bool saveOrLoad;
    public bool delete;

    void Start()
    {
        string path = LevelManager.GetPath(number, true);
        numberText.text = number.ToString();
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Shortly parametrs = JsonUtility.FromJson<Shortly>(json);
            modeText.text = parametrs.mode;
            timeText.text = parametrs.time.ToString("0") + "с";
        }
        else
        {
            modeText.text = "";
            timeText.text = "";
            if (delete)
            {
                Destroy(gameObject);
            }
        }
    }
    public void Click()
    {
        if(saveOrLoad)
        {
            levelManager.Save(number);
        }
        else
        {
            levelManager.Load(number);
        }
    }
}
