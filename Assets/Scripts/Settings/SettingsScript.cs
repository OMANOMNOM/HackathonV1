using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Audio;
public class SettingsScript : MonoBehaviour
{
    [Header("Qualitys")]
    public RenderPipelineAsset[] Qualitys;
    public TMP_Dropdown Qualitydropdown;

    [Header("Resolutions")]
    public TMP_Dropdown Resolutiondropdown;
    Resolution[] resoultions;

    [Header("Audio Mixers")]
    public AudioMixer MainAudioMixer;

    // Start is called before the first frame update
    void Start()
    {
        Qualitydropdown.value = QualitySettings.GetQualityLevel();

        resoultions = Screen.resolutions;

        Resolutiondropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentresindex = 0;

        for (int i = 0; i < resoultions.Length; i++)
        {
            string option = resoultions[i].width + " x " + resoultions[i].height;
            options.Add(option);

            if (resoultions[i].width == Screen.currentResolution.width && resoultions[i].height == Screen.currentResolution.height)
            {
                currentresindex = i;
            }
        }

        Resolutiondropdown.AddOptions(options);
        Resolutiondropdown.value = currentresindex;
        Resolutiondropdown.RefreshShownValue();
    }

    public void ChangeQualityLevel(int value)
    {
        QualitySettings.SetQualityLevel(value);
        QualitySettings.renderPipeline = Qualitys[value];
    }

    public void FullScreenToggle(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void MainVolumeChange(float volume)
    {
        MainAudioMixer.SetFloat("BGMVolume", volume);
    }
    public void SFXVolumeChange(float volume)
    {
        MainAudioMixer.SetFloat("SFXVolume", volume);
    }

    public void MasterVolumeChange(float volume)
    {
        MainAudioMixer.SetFloat("MasterVolume", volume);
    }

    public void ResolutionChange(int resolutionindex)
    {
        Resolution resolution = resoultions[resolutionindex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
