using UnityEngine;
using UnityEngine.UI;

public class WaveText : MonoBehaviour
{
    public Text waveText;
    public float[] startWaves = new float[4];
    public float[] endWaves = new float[4];
    public int nextWaveID = 0;

    void Start()
    {
        waveText.text = "";
    }

    void Update()
    {
        if (startWaves.Length != nextWaveID)
        {
            waveText.text = "";
            if (PlayerStats.time > startWaves[nextWaveID] - 99)
            {
                if (PlayerStats.time < startWaves[nextWaveID])
                {
                    waveText.text = (startWaves[nextWaveID] - PlayerStats.time).ToString("0") + "с до начала валны";
                }
                else if(PlayerStats.time < endWaves[nextWaveID])
                {
                    waveText.text = (endWaves[nextWaveID] - PlayerStats.time).ToString("0") + "с до конца волны";
                }
                else
                {
                    nextWaveID += 1;
                }
            }
        }
    }
}
