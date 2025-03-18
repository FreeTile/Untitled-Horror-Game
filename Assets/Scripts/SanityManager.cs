using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class SanityManager : MonoBehaviour
{

    public static SanityManager Instance { get; private set; }
    public Stage stage { get; private set; }
    public float StageValue;
    private float sanityValue;
    private GameInputHandler input;
    [SerializeField]
    private float minDelta, maxDelta;
    [SerializeField] 
    private float measureDuration = 0.5f;
    private float MouseSensitivity = 1f;

    public enum Stage
    {
        Low,
        Medium,
        High
    }
    void Start()
    {
        if (Instance == null) //Creating syngleton instance at the beginning 
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        stage = Stage.High;
        StageValue = 100;
        input = GameInputHandler.Instance;
        MouseSensitivity = this.GetComponent<PlayerController>().MouseSensitivity;
    }

    public void DecreaseSanity(float Value)
    {
        StageValue -= Value;
        if (StageValue <= 100 && stage != Stage.High)
        {
            stage--;
        }
    }

    public void IncreaseSanity(float Value)
    {
        sanityValue += Value;
        if (StageValue >= 100 && stage != Stage.High)
        {
            stage++;
        }
    }

    public void CheckFearLevel(int minSanityLoss, int maxSanityLoss)
    {
        StartCoroutine(MeasureMouseShake(minSanityLoss, maxSanityLoss));
    }

    private IEnumerator MeasureMouseShake(int minSanityLoss, int maxSanityLoss)
    {

        float elapsed = 0f;
        float Delta = 0f;

        while (elapsed < measureDuration)
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
    }
}
