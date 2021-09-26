using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Toggle toggleFixationSelection;
    public Toggle toggleParticlesEnabled;

    void Start()
    {
        toggleFixationSelection.isOn = PlayerStats.fixationSelection;
        toggleParticlesEnabled.isOn = PlayerStats.particlesEnabled;
    }

    private void OnEnable()
    {
        PlayerStats.Pause();
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
        Invoke("DisablePause", 0.1f);
    }

    private void DisablePause()
    {
        PlayerStats.Play();
    }

    public void ToggleFixationSelection(bool fixationSelection)
    {
        PlayerStats.fixationSelection = fixationSelection;
        if (fixationSelection)
            PlayerPrefs.SetInt("fixationSelection", 1);
        else
            PlayerPrefs.SetInt("fixationSelection", 0);
    }
    
    public void ToggleParticlesEnabled(bool particlesEnabled)
    {
        PlayerStats.particlesEnabled = particlesEnabled;
        if (particlesEnabled)
            PlayerPrefs.SetInt("particlesEnabled", 1);
        else
            PlayerPrefs.SetInt("particlesEnabled", 0);
    }
}
