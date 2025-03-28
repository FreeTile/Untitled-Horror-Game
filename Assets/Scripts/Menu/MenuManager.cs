using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public EventReference ValueChange;
    public EventReference BackSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToGame()
    {
        //Load GameScene
        SceneManager.LoadScene("Ambient");

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
    
    public void PlaySoundBackButton()
    {
        RuntimeManager.PlayOneShot(BackSound);
    }
    public void PlaySoundValueChange()
    {
        RuntimeManager.PlayOneShot(ValueChange);
    }

}
