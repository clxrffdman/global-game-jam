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
        slider = GetComponentInChildren<Slider>();

    }

    private void FixedUpdate()
    {
        
    }

    public void SetValue(float value)
    {
        if(mixer != null)
        {
            switch (index) {
                case 0:
                    mixer.SetFloat("master", value);
                    break;
                case 1:
                    mixer.SetFloat("sfxVol", value);
                    break;
                case 2:
                    mixer.SetFloat("musicVol", value);
                    break;
            }
        }
    }
}
