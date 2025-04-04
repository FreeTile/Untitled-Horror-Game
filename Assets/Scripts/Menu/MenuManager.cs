using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private string musicBusPath = "bus:/Music";
    [SerializeField] private string SFXBusPath = "bus:/SFX";

    public TextMeshProUGUI versionText;
    public EventReference ClickSound;
    public EventReference UISelected;
    public EventReference MainMenuMusic;
    // Start is called before the first frame update
    void Start()
    {
        //versionText.text = "Version: " + Application.version;
        RuntimeManager.PlayOneShot(MainMenuMusic);
    }

    public void GoToGame()
    {
        //Load GameScene
        SceneManager.LoadScene("Game");

    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void ToggleGlitch()
    {
        if (GlitchFeature.Instance != null)
        {
            GlitchFeature.Instance.glitchEnabled = !GlitchFeature.Instance.glitchEnabled;
            Debug.Log("Glitch enabled: " + GlitchFeature.Instance.glitchEnabled);
        }
        else
        {
            Debug.LogWarning("GlitchFeature instance is not available.");
        }
    }
    
   

    //Sound stuff goes here
    public void PlaySoundButtonSelected()
    {
        RuntimeManager.PlayOneShot(UISelected);
    }
    public void PlaySoundClick()
    {
        RuntimeManager.PlayOneShot(ClickSound);
    }

    public void StopMusic()
    {
        Bus musicBus = RuntimeManager.GetBus(musicBusPath);
        musicBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    private void OnSceneUnloaded(Scene scene)
    {
        Bus SFXBus = RuntimeManager.GetBus(SFXBusPath);
        SFXBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private void OnDestroy()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
}
