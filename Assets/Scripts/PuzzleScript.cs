using FMODUnity;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleScript : MonoBehaviour
{
    [Header("Locked Door")]
    [SerializeField] private Door lockedDoor;

    private bool unlockDoor = false;
    public ButtonSwapTexture[] buttons;
    private int targetValue = 5;

    public EventReference DoorUnlocked;

    public void CheckCombination()
    {
        unlockDoor = true;
        foreach (ButtonSwapTexture button in buttons)
        {
            if (button.GetCurrentIndex() != targetValue)
            {
                unlockDoor = false;
                break;
            }
            Debug.Log(button.GetCurrentIndex());
        }
        if(lockedDoor != null && unlockDoor == true)
        {
            Debug.Log("Locked Door is not null");
            lockedDoor.isLocked = false;
            RuntimeManager.PlayOneShot(DoorUnlocked);
        }
    }
}
