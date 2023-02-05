using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSlider : MonoBehaviour
{
    public Slider slider;
    public AudioMixer mixer;
    public int index;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    private void FixedUpdate()
    {
        if (mixer != null && slider != null)
        {
            float value = 0;

            switch (index)
            {
                case 0:
                    mixer.GetFloat("master", out value);
                    break;
                case 1:
                    mixer.GetFloat("sfxVol", out value);
                    break;
                case 2:
                    mixer.GetFloat("musicVol", out value);
                    break;
            }

            slider.value = value;

        }
    }

    public void SetValue(float value)
    {
        if(mixer != null)
        {
            switch (index) {
                case 0:
                    mixer.SetFloat("MasterVolume", value);
                    break;
                case 1:
                    mixer.SetFloat("SFXVolume", value);
                    break;
                case 2:
                    mixer.SetFloat("MusicVolume", value);
                    break;
            }


            
        }
    }
}
