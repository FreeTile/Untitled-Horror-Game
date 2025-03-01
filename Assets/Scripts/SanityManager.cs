using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityManager : MonoBehaviour
{
    public int stage { get; private set; }
    private float sanityValue;
    void Start()
    {
        stage = 3;
    }

    public void DecreaseSanity(float Value)
    {
        sanityValue -= Value * Time.deltaTime;
        stage = (int)(sanityValue / 100) + 1;
    }

    public void IncreaseSanity(float Value)
    {
        sanityValue += Value;
        if (sanityValue > 300f)
        {
            sanityValue = 300f;
        }
    }
}
