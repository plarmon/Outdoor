using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField]
    private AudioMixer mixer;
    [SerializeField]
    private Slider masterSlider;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider sfxSlider;

    const string MIXER_MASTER = "MasterVolume";
    const string MIXER_MUSIC = "MusicVolume";
    const string MIXER_SFX = "SFXVolume";

    private void Awake() {
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    private void SetMasterVolume(float value) {
        mixer.SetFloat(MIXER_MASTER, Mathf.Log10(value) * 20);
    }

    private void SetMusicVolume(float value) {
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);
    }

    private void SetSFXVolume(float value) {
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20);
    }
}
