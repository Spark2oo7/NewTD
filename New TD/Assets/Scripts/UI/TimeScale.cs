using UnityEngine;
using UnityEngine.UI;

public class TimeScale : MonoBehaviour
{
    public void SetVolume(float volume)
    {
        Time.timeScale = VolumeToScale(volume);
    }

    public float VolumeToScale(float volume)
    {
        return volume*volume + 0.5f * volume + 0.5f;
    }
}
