using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobbber : MonoBehaviour
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

    private GameInputHandler input;

    void Start()
    {
        initialLocalPos = transform.localPosition;
        input = GameInputHandler.Instance;
    }

    void Update()
    {
        Breathe();
    }

    //Function uses cos and sin from -1 to 1 to emulate breathing without animation
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
            bobOffsetX = Mathf.Cos(timer/2) * XAmplitude;
        }

        Vector3 finalPos = initialLocalPos + new Vector3(bobOffsetX, bobOffsetY, 0f);

        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos, Time.deltaTime * 5f);
    }
}
