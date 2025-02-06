using UnityEngine;
using FMODUnity;

public class DoorSound : MonoBehaviour
{
    
    private StudioEventEmitter eventEmitter;

    private void Awake()
    {
        eventEmitter = GetComponent<StudioEventEmitter>();
        if (eventEmitter == null)
        {
            Debug.LogError("На объекте отсутствует компонент StudioEventEmitter!");
        }
    }

    //On click we playsound, just for now. Gonna update it future.
    private void OnMouseDown()
    {
        if (eventEmitter != null)
        {
            eventEmitter.Play();
        }
    }
}
