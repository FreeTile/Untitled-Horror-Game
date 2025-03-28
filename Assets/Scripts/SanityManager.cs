using UnityEngine;
using System.Collections;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.UI;

public class SanityManager : MonoBehaviour
{
    [Header("Sanity Settings")]
    [Range(0, 100)]
    public int sanity = 70; // Initial sanity value

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

    private EventInstance ambientInstance;

    void Start()
    {
        ambientInstance = RuntimeManager.CreateInstance(ambientEvent);
        ambientInstance.start();
        UpdateSoundParameters();
        // Create and start the ambient event instance
        FindObjectOfType<GlitchFeature>().glitchEnabled = false;
        Debug.Log(FindObjectOfType<GlitchFeature>().glitchEnabled);
        
    }

    // Update FMOD parameters based on current sanity value
    void UpdateSoundParameters()
    {
        Debug.Log("Current Sanity is: " + sanity);
        if (sanity <= 29)
        {
            //sanityBrain.sprite = lowSanitySprite;
            ambientInstance.setParameterByName(lowParameterName, 1.0f);
            ambientInstance.setParameterByName(mediumParameterName, 0.0f);
            ambientInstance.setParameterByName(highParameterName, 0.0f);
            Debug.Log("State set to Low");
        }
        else if (sanity <= 69)
        {
            //sanityBrain.sprite = mediumSanitySprite;
            ambientInstance.setParameterByName(lowParameterName, 0.0f);
            ambientInstance.setParameterByName(mediumParameterName, 1.0f);
            ambientInstance.setParameterByName(highParameterName, 0.0f);
            Debug.Log("State set to Medium");
        }
        else
        {
            //sanityBrain.sprite = highSanitySprite;
            ambientInstance.setParameterByName(lowParameterName, 0.0f);
            ambientInstance.setParameterByName(mediumParameterName, 0.0f);
            ambientInstance.setParameterByName(highParameterName, 1.0f);
            Debug.Log("State set to High");
        }
    }

    // Method to trigger the screamer event
    public void TriggerScreamer()
    {
        StartCoroutine(MeasureMouseJerk());
    }

    // Coroutine that measures total mouse movement during the screamer duration,
    // calculates the reduction in sanity, and updates the FMOD parameters accordingly.
    IEnumerator MeasureMouseJerk()
    {
        float elapsed = 0f;
        float totalMouseDelta = 0f;
        Vector3 lastMousePosition = Input.mousePosition;

        while (elapsed < screamerDuration)
        {
            yield return null;
            Vector3 currentMousePosition = Input.mousePosition;
            totalMouseDelta += Vector3.Distance(currentMousePosition, lastMousePosition);
            lastMousePosition = currentMousePosition;
            elapsed += Time.deltaTime;
        }

        int sanityReduction = Mathf.RoundToInt(totalMouseDelta * screamerImpactMultiplier);
        sanity -= sanityReduction;
        Debug.Log("Sanity reduction is: " + sanityReduction);
        sanity = Mathf.Clamp(sanity, 0, 100);

        UpdateSoundParameters();
    }

    // Method to decrease sanity by a given amount and update parameters accordingly
    public void DecreaseSanity(int amount)
    {
        sanity -= amount;
        sanity = Mathf.Clamp(sanity, 0, 100);
        UpdateSoundParameters();
    }

    // Stop and release the FMOD event instance when the object is destroyed
    private void OnDestroy()
    {
        ambientInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        ambientInstance.release();
    }
}