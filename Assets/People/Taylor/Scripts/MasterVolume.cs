using UnityEngine;
using UnityEngine.Audio; 
using UnityEngine.UI;   

public class MasterVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public string exposedParamName = "mastervolume"; 

    private Slider volumeSlider;

    void Awake()
    {
        volumeSlider = GetComponent<Slider>();

        if (volumeSlider != null)
        {
            float savedVolume = PlayerPrefs.GetFloat(exposedParamName, 0.75f);
            volumeSlider.value = savedVolume;
            SetVolume(savedVolume); 
        }
    }

    public void SetVolume(float sliderValue)
    {
        float mixerVolume = Mathf.Log10(sliderValue) * 20;
        mixer.SetFloat(exposedParamName, mixerVolume);

        PlayerPrefs.SetFloat(exposedParamName, sliderValue);
    }
}
