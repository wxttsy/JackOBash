using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("SFXsound", 0.75f);
        slider.value = PlayerPrefs.GetFloat("GameSound", 0.75f);

    }
    private void Update()
    {
<<<<<<< Updated upstream
       
=======

>>>>>>> Stashed changes
    }
    public void SetLevelSFXsound(float sliderValue)
    {
        mixer.SetFloat("SFXsound", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("SFXsound", sliderValue);
    }

    public void SetLevelGameSound(float SliderValue)
    {
        mixer.SetFloat("GameSound", Mathf.Log10(SliderValue) * 20);
        PlayerPrefs.SetFloat("GameSound", SliderValue);
    }


<<<<<<< Updated upstream
}

=======
}
>>>>>>> Stashed changes
