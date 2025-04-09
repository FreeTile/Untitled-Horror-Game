using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using System.Collections.Generic;

public class SettingsMenu : MonoBehaviour
{
    public Volume postProcessingVolume;
    private ColorAdjustments colorAdjust;

    public TMP_Dropdown ResDropDown;
    public TMP_Dropdown qualityDropdown;
    public Toggle fullscreenToggle;
    public Toggle vsyncToggle;
    public Slider brightnessSlider;
    public Slider gammaSlider;
    public TMP_Text brightnessValueText;
    public TMP_Text gammaValueText;

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public TMP_Text masterValueText;
    public TMP_Text musicValueText;
    public TMP_Text sfxValueText;

    Resolution[] AllResolutions;
    int SelectedResolution;
    List<Resolution> SelectedResolutionList = new List<Resolution>();

    void Start()
    {
        if (postProcessingVolume != null && postProcessingVolume.profile.TryGet(out colorAdjust))
        {
            // ready to use
        }


        AllResolutions = Screen.resolutions;
        List<string> resolutionsStringList = new List<string>();
        string newRes;
        foreach (Resolution res in AllResolutions)
        {
            newRes = res.width + " x " + res.height;
            if (!resolutionsStringList.Contains(newRes))
            {
                resolutionsStringList.Add(newRes);
                SelectedResolutionList.Add(res);
            }
        }
        ResDropDown.ClearOptions();
        ResDropDown.AddOptions(resolutionsStringList);


        List<string> qualities = new List<string>(QualitySettings.names);
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(qualities);


        LoadSettings();
    }

    public void ChangeResolution()
    {
        SelectedResolution = ResDropDown.value;
        Resolution res = SelectedResolutionList[SelectedResolution];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        PlayerPrefs.SetInt("ResolutionIndex", SelectedResolution);
    }

    public void ToggleFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }

    public void ToggleVSync(bool isOn)
    {
        QualitySettings.vSyncCount = isOn ? 1 : 0;
        PlayerPrefs.SetInt("VSync", isOn ? 1 : 0);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("QualityLevel", qualityIndex);
    }

    public void SetBrightness(float value)
    {
        if (colorAdjust != null)
        {
            colorAdjust.postExposure.value = value;
            PlayerPrefs.SetFloat("Brightness", value);
        }

        if (brightnessValueText != null)
            brightnessValueText.text = value.ToString("F2");
    }

    public void SetGamma(float value)
    {
        if (colorAdjust != null)
        {
            float contrast = Mathf.Lerp(-50f, 50f, value);
            colorAdjust.contrast.value = contrast;
            PlayerPrefs.SetFloat("Gamma", contrast);

            if (gammaValueText != null)
                gammaValueText.text = contrast.ToString("F0");
        }
    }

    public void SetMasterVolume(float value)
    {
        PlayerPrefs.SetFloat("MasterVolume", value);
        if (masterValueText != null)
            masterValueText.text = Mathf.RoundToInt(value * 100f).ToString();
    }

    public void SetMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        if (musicValueText != null)
            musicValueText.text = Mathf.RoundToInt(value * 100f).ToString();
    }

    public void SetSFXVolume(float value)
    {
        PlayerPrefs.SetFloat("SFXVolume", value);
        if (sfxValueText != null)
            sfxValueText.text = Mathf.RoundToInt(value * 100f).ToString();
    }

    void LoadSettings()
    {
        // Resolution
        int resIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);
        if (resIndex < SelectedResolutionList.Count)
        {
            ResDropDown.value = resIndex;
            Resolution res = SelectedResolutionList[resIndex];
            Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        }

        // Fullscreen
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", Screen.fullScreen ? 1 : 0) == 1;
        Screen.fullScreen = isFullscreen;
        fullscreenToggle.isOn = isFullscreen;

        // VSync
        bool isVSync = PlayerPrefs.GetInt("VSync", 1) == 1;
        QualitySettings.vSyncCount = isVSync ? 1 : 0;
        vsyncToggle.isOn = isVSync;

        // Quality
        int quality = PlayerPrefs.GetInt("QualityLevel", QualitySettings.GetQualityLevel());
        QualitySettings.SetQualityLevel(quality);
        qualityDropdown.value = quality;

        // Brightness
        if (PlayerPrefs.HasKey("Brightness"))
        {
            float brightness = PlayerPrefs.GetFloat("Brightness");
            brightnessSlider.value = brightness;
            if (colorAdjust != null) colorAdjust.postExposure.value = brightness;
            if (brightnessValueText != null) brightnessValueText.text = brightness.ToString("F2");
        }

        // Gamma
        if (PlayerPrefs.HasKey("Gamma"))
        {
            float gamma = PlayerPrefs.GetFloat("Gamma");
            gammaSlider.value = Mathf.InverseLerp(-50f, 50f, gamma);
            if (colorAdjust != null) colorAdjust.contrast.value = gamma;
            if (gammaValueText != null) gammaValueText.text = gamma.ToString("F0");
        }

        // Audio Volumes
        float master = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float music = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 1f);

        masterSlider.value = master;
        musicSlider.value = music;
        sfxSlider.value = sfx;

        if (masterValueText != null) masterValueText.text = Mathf.RoundToInt(master * 100f).ToString();
        if (musicValueText != null) musicValueText.text = Mathf.RoundToInt(music * 100f).ToString();
        if (sfxValueText != null) sfxValueText.text = Mathf.RoundToInt(sfx * 100f).ToString();
    }
}
