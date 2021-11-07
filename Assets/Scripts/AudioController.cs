using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public AudioSource Sounds;

    private float musicVolume;
    private float soundsVolume;

    public Slider musicSlider;
    public Slider soundsSlider;

    void Start()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        soundsVolume = PlayerPrefs.GetFloat("SoundsVolume");
        musicSlider.value = musicVolume;
        soundsSlider.value = soundsVolume;
    }
    void Update()
    {
        backgroundMusic.volume = musicVolume;
        Sounds.volume = soundsVolume;
    }
    public void SetMusicVolume(float vol)
    {
        musicVolume = vol;
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
    }
    public void SetSoundsVolume(float vol)
    {
        soundsVolume = vol;
        PlayerPrefs.SetFloat("SoundsVolume", soundsVolume);
    }
}
