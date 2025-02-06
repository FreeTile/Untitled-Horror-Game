using UnityEngine;
using FMODUnity;

public class ClickTriggerSound : MonoBehaviour
{
    //Trigger other sound that is located somewhere else, when triggering some other object in front.
    public StudioEventEmitter soundEmitter;

    private void OnMouseDown()
    {
        if (soundEmitter != null)
        {
            soundEmitter.Play();
        }
        else
        {
            Debug.LogWarning("Ссылка на soundEmitter не назначена!");
        }
    }
}
