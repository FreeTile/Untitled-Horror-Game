using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SettingsMenu : MonoBehaviour
{
    public Volume postProcessingVolume;
    private ColorAdjustments colorAdjust;

    void Start()
    {
        if (postProcessingVolume != null && postProcessingVolume.profile.TryGet(out colorAdjust))
        {
            // ready to use
        }
    }

    public void ToggleFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void ToggleVSync(bool isOn)
    {
        QualitySettings.vSyncCount = isOn ? 1 : 0;
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetBrightness(float value)
    {
        if (colorAdjust != null)
        {
            colorAdjust.postExposure.value = value;
        }
    }

    public void SetGamma(float value)
    {
        if (colorAdjust != null)
        {
            colorAdjust.contrast.value = Mathf.Lerp(-50f, 50f, value); // simulate gamma with contrast
        }
    }
}
