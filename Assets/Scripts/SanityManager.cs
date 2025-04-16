using UnityEngine;
using System.Collections;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.UI;
using UnityEngine.Windows;

public class SanityManager : MonoBehaviour
{
    [Header("Sanity Settings")]
    [Range(0, 100)]
    public int sanity = 100; // Initial sanity value

    [Header("FMOD Ambient Event")]
    [Tooltip("FMOD event path for the Ambient track")]
    public string ambientEvent = "event:/Ambient/Ambient";

    [Header("FMOD Parameter Names")]
    [Tooltip("Parameter name for Low state")]
    public string lowParameterName = "Low";
    [Tooltip("Parameter name for Medium state")]
    public string mediumParameterName = "Medium";
    [Tooltip("Parameter name for High state")]
    public string highParameterName = "High";

    [Header("Screamer Settings")]
    [Tooltip("Duration in seconds to measure mouse movement after a screamer trigger")]
    public float screamerDuration = 0.5f;
    [Tooltip("Multiplier for the impact of mouse movement on reducing sanity")]
    public float screamerImpactMultiplier = 0.05f;

    [Header("Sanity Brain DAMAGE")]
    public Image sanityBrain;

    public Sprite highSanitySprite;
    public Sprite mediumSanitySprite;
    public Sprite lowSanitySprite;

    [SerializeField] public string musicBusPath = "bus:/Music";
    public FMOD.Studio.Bus musicBus;

    public static EventInstance ambientInstance;

    //Underwater effect
    private FMOD.DSP lowpassDSP;
    private FMOD.Studio.Bus masterBus;
    private FMOD.ChannelGroup channelGroup;

    private GameInputHandler input;
    private float MouseSensitivity = 1f;
    [SerializeField]
    private float minDelta, maxDelta;

    void Start()
    {
        musicBus = RuntimeManager.GetBus(musicBusPath);
        ambientInstance = RuntimeManager.CreateInstance(ambientEvent);
        ambientInstance.start();
        UpdateSoundParameters();
        masterBus = RuntimeManager.GetBus("bus:/");
        masterBus.getChannelGroup(out channelGroup);
        RuntimeManager.CoreSystem.createDSPByType(FMOD.DSP_TYPE.LOWPASS, out lowpassDSP);

        input = GameInputHandler.Instance;
        MouseSensitivity = this.GetComponent<PlayerController>().MouseSensitivity;
    }

    // Update FMOD parameters based on current sanity value
    void UpdateSoundParameters()
    {
        Debug.Log("Current Sanity is: " + sanity);
        if (sanity <= 33)
        {
            //sanityBrain.sprite = lowSanitySprite;
            ambientInstance.setParameterByName(lowParameterName, 1.0f);
            ambientInstance.setParameterByName(mediumParameterName, 0.0f);
            ambientInstance.setParameterByName(highParameterName, 0.0f);
            ApplyUnderwaterEffect(true);
            Debug.Log("State set to Low");
        }
        else if (sanity <= 66)
        {
            //sanityBrain.sprite = mediumSanitySprite;
            ambientInstance.setParameterByName(lowParameterName, 0.0f);
            ambientInstance.setParameterByName(mediumParameterName, 1.0f);
            ambientInstance.setParameterByName(highParameterName, 0.0f);
            ApplyUnderwaterEffect(false);
            Debug.Log("State set to Medium");
        }
        else
        {
            //sanityBrain.sprite = highSanitySprite;
            ambientInstance.setParameterByName(lowParameterName, 0.0f);
            ambientInstance.setParameterByName(mediumParameterName, 0.0f);
            ambientInstance.setParameterByName(highParameterName, 1.0f);
            ApplyUnderwaterEffect(false);
            Debug.Log("State set to High");
        }
    }

    public void ApplyUnderwaterEffect(bool apply)
    {
        if (apply)
        {
            lowpassDSP.setParameterFloat((int)FMOD.DSP_LOWPASS.CUTOFF, 500f);

            channelGroup.addDSP(0, lowpassDSP);
            Debug.Log("Applied underwater effect");
        }
        else
        {
            channelGroup.removeDSP(lowpassDSP);
        }
    }

    // Method to trigger the screamer event
    public void CheckFearLevel(int minSanityLoss, int maxSanityLoss, float duration)
    {
        StartCoroutine(MeasureMouseShake(minSanityLoss, maxSanityLoss, duration));
    }

    private IEnumerator MeasureMouseShake(int minSanityLoss, int maxSanityLoss, float duration)
    {
        Debug.Log("Checking fear level");
        float elapsed = 0f;
        float Delta = 0f;

        while (elapsed < duration)
        {
            if (input.LookInput.magnitude * MouseSensitivity > Delta)
            {
                Delta = input.LookInput.magnitude * MouseSensitivity;
            }
            
            elapsed += Time.deltaTime;
            yield return null;
        }

        float clampedDelta = Mathf.Clamp(Delta, minDelta, maxDelta);

        float t = (clampedDelta - minDelta) / (maxDelta - minDelta);

        float finalSanityLoss = Mathf.Lerp(minSanityLoss, maxSanityLoss, t);

        DecreaseSanity(finalSanityLoss);

        Debug.Log("Decreased sanity by " + finalSanityLoss);
    }

    // Method to decrease sanity by a given amount and update parameters accordingly
    public void DecreaseSanity(float amount)
    {
        sanity -= (int)amount;
        sanity = Mathf.Clamp(sanity, 0, 100);
        UpdateSoundParameters();
    }
    public void SetMusicVolume(float volume)
    {
        musicBus.setVolume(volume);
    }

    // Stop and release the FMOD event instance when the object is destroyed
    private void OnDestroy()
    {
        ambientInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        ambientInstance.release();
    }
}