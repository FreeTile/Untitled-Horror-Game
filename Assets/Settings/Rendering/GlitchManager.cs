using UnityEngine;

public class GlitchManager : MonoBehaviour
{
    [Header("Glitch Material")]
    public Material glitchMaterial;

    [Header("Timing Between Glitches")]
    public float intervalMin = 3f;
    public float intervalMax = 8f;

    [Header("Glitch Duration")]
    public float durationMin = 0.1f;
    public float durationMax = 0.5f;

    private float nextGlitchTime;
    private float glitchEndTime;
    private bool isGlitching;

    void Start()
    {
        ScheduleNextGlitch();
        if (glitchMaterial)
            glitchMaterial.SetFloat("_GlitchIntensity", 0f);
    }

    void Update()
    {
        if (!glitchMaterial) return;

        if (!isGlitching)
        {
            if (Time.time >= nextGlitchTime)
            {
                StartGlitch();
            }
        }
        else
        {
            if (Time.time >= glitchEndTime)
            {
                EndGlitch();
            }
        }
    }

    private void StartGlitch()
    {
        isGlitching = true;
        float glitchDuration = Random.Range(durationMin, durationMax);
        glitchEndTime = Time.time + glitchDuration;

        glitchMaterial.SetFloat("_GlitchIntensity", 1f);
    }

    private void EndGlitch()
    {
        isGlitching = false;
        glitchMaterial.SetFloat("_GlitchIntensity", 0f);
        ScheduleNextGlitch();
    }

    private void ScheduleNextGlitch()
    {
        float interval = Random.Range(intervalMin, intervalMax);
        nextGlitchTime = Time.time + interval;
    }
}
