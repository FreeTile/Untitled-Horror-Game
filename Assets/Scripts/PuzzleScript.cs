using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleScript : MonoBehaviour
{
    [Header("Locked Door")]
    [SerializeField] private GameObject lockedDoor;

    private bool unlockDoor = false;
    public ButtonSwapTexture[] buttons;
    private int targetValue = 5;

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
        }
        if(lockedDoor != null && unlockDoor == true)
        {
            Debug.Log("Locked Door is not null");
            /*lockedDoor.gameObject.SetActive(false);
            MARK - OPEN DOOR4_C1 (STORAGE DOOR)*/
        }
    }
}
