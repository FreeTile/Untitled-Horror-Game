using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityManager : MonoBehaviour
{
    public Stage stage { get; private set; }
    public float StageValue;
    private float sanityValue;
    
    public enum Stage
    {
        Low,
        Medium,
        High
    }
    void Start()
    {
        stage = Stage.High;
        StageValue = 100;
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
}
