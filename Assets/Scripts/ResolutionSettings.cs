using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionSettings : MonoBehaviour
{
    public Dropdown resolutionSettings;
    public Toggle fullScreenMode;
    Resolution[] res;
    int CurrentResolution;
    int fullScreenModeEnabled;
    bool IsFullScreen;

    void Start()
    {
        res = Screen.resolutions;
        string[] strRes = new string[res.Length];
        for (int i = 0; i < res.Length; i++)
        {
            strRes[i] = res[i].width.ToString() + "x" + res[i].height.ToString();
            Debug.Log(strRes[i]);
        }
        resolutionSettings.AddOptions(strRes.ToList());

        fullScreenModeEnabled = PlayerPrefs.GetInt("FullScreen");
        if(fullScreenModeEnabled == 1)
        {
            IsFullScreen = true;
            fullScreenMode.isOn = true;
        }
        else
        {
            IsFullScreen = false;
            fullScreenMode.isOn = false;
        }
        CurrentResolution = PlayerPrefs.GetInt("CurrentResolution");
        if(CurrentResolution == 0)
        {
            Screen.SetResolution(res[res.Length-1].width, res[res.Length-1].height, IsFullScreen);
            CurrentResolution = res.Length-1;
            PlayerPrefs.SetInt("CurrentResolution", CurrentResolution);
        }
        else
        {
            Screen.SetResolution(res[CurrentResolution].width, res[CurrentResolution].height, IsFullScreen);
        }
        resolutionSettings.value = CurrentResolution;
    }
    public void SetCurrentResolution()
    {
        Screen.SetResolution(res[resolutionSettings.value].width,res[resolutionSettings.value].height, IsFullScreen);
        PlayerPrefs.SetInt("CurrentResolution", resolutionSettings.value);
    }
    public void SetFullscreenMode()
    {
        if(fullScreenMode.isOn)
        {
            Screen.SetResolution(res[resolutionSettings.value].width, res[resolutionSettings.value].height, true);
            fullScreenModeEnabled = 1;
            IsFullScreen = true;
            fullScreenMode.isOn = true;
        }
        else
        {
            Screen.SetResolution(res[resolutionSettings.value].width, res[resolutionSettings.value].height, false);
            fullScreenModeEnabled = 0;
            IsFullScreen = false;
            fullScreenMode.isOn = false;
        }
        PlayerPrefs.SetInt("FullScreen", fullScreenModeEnabled);   
    }
}
