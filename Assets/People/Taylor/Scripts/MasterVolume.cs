using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections; 

public class MasterVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public string exposedParamName = "MasterVolume";

    private Slider volumeSlider;

    void Awake()
    {
        volumeSlider = GetComponent<Slider>();
        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0.001f;
            volumeSlider.maxValue = 1f;

            float savedVolume = PlayerPrefs.GetFloat(exposedParamName, 0.75f);

            volumeSlider.value = savedVolume;

            SetVolume(savedVolume);

            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
        else
        {
            Debug.LogError("MasterVolume script requires a Slider component attached to this GameObject.");
        }
    }

    public void SetVolume(float sliderValue)
    {
        
        float mixerVolume = Mathf.Log10(Mathf.Max(sliderValue, 0.0001f)) * 20;

        mixer.SetFloat(exposedParamName, mixerVolume);

        PlayerPrefs.SetFloat(exposedParamName, sliderValue);

        PlayerPrefs.Save();
    }
}
