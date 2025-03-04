using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class HeadBobFootsteps : MonoBehaviour
{
    [Header("Idle")]
    [Tooltip("Frequency of breathing")]
    public float breathingFrequency = 1.0f;

    [Header("Walk/Run")]
    [Tooltip("Fixed frequency while walking or running")]
    public float walkingFrequency = 2.0f;
    [SerializeField] private float sprintMultiplier = 1.5f;

    [Header("Amplitude")]
    [SerializeField] private float XAmplitude = 0.125f;
    [SerializeField] private float YAmplitude = 0.05f;

    private Vector3 initialLocalPos;
    private float timer = 0f;
    //NEW: Added this to track bob position for footstep sound
    private float lastBobOffsetY = 0f;

    [Header("Footsteps (FMOD) Settings")]
    public FMODUnity.EventReference m_EventPath;

    //NEW: Surface parameters from 0.0 - 1.0f
    public float m_House;
    public float m_Grass;
    //Debug for footsteps surface
    [Header("Debug Settings")]
    public bool m_Debug;

    private GameInputHandler input;

    void Start()
    {
        initialLocalPos = transform.localPosition;
        input = GameInputHandler.Instance; 
        //Saving offset to compare
        lastBobOffsetY = Mathf.Sin(timer) * YAmplitude;
    }

    void Update()
    {
        Breathe();
    }

    //Function uses cos and sin from -1 to 1 to emulate breathing without animation
    void PlayFootstepSound()
    {
        // Default: play Grass sound
        m_House = 0.0f;
        m_Grass = 1.0f;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1000.0f))
        {
            // Check if the object below is on the "Ground" layer
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                // Determine surface type based on the object's tag
                if (hit.collider.CompareTag("Grass"))
                {
                    m_Grass = 1.0f;
                    m_House = 0.0f;
                }
                else if (hit.collider.CompareTag("House"))
                {
                    m_Grass = 0.0f;
                    m_House = 1.0f;
                }
                else
                {
                    // Default to Grass if the tag does not match expected values
                    m_Grass = 1.0f;
                    m_House = 0.0f;
                }
            }
            else
            {
                // If the object is not on the "Ground" layer, assume it's not ground and play House sound
                m_Grass = 0.0f;
                m_House = 1.0f;
            }
        }
        else
        {
            // If the raycast didn't hit anything, default to House sound
            m_Grass = 0.0f;
            m_House = 1.0f;
        }

        if (m_Debug)
            Debug.Log("Footstep triggered - House: " + m_House + " Grass: " + m_Grass);

        // Create and start the FMOD event instance with the appropriate parameters
        EventInstance e = RuntimeManager.CreateInstance(m_EventPath);
        e.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        e.setParameterByName("House", m_House);
        e.setParameterByName("Grass", m_Grass);

        e.start();
        e.release();
    }

    private void Breathe()
    {
        bool isMoving = input.MoveInput.sqrMagnitude > 0.1f;
        float frequency = 0f;

        if (isMoving)
        {
            frequency = walkingFrequency * (input.SprintInput ? sprintMultiplier : 1f); //The breathing rate depends on whether the player is running or not
        }
        else
        {
            frequency = breathingFrequency;
        }

        timer += Time.deltaTime * frequency;
        float bobOffsetY = Mathf.Sin(timer) * YAmplitude;
        float bobOffsetX = 0f;
        if (input.MoveInput.y > 0f) //Shakes head to the sides only if the player is moving forward
        {
            bobOffsetX = Mathf.Cos(timer / 2) * XAmplitude;
        }

        Vector3 finalPos = initialLocalPos + new Vector3(bobOffsetX, bobOffsetY, 0f);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos, Time.deltaTime * 5f);

        // NEW: If the player is moving and the bobbing cycle is in the "up" phase (transition from negative to zero or positive),
        // trigger the footstep sound.
        if (isMoving && lastBobOffsetY < 0f && bobOffsetY >= 0f)
        {
            PlayFootstepSound();
        }

        lastBobOffsetY = bobOffsetY;
    }
}
