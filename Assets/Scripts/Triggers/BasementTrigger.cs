using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class BasementTrigger : MonoBehaviour
{
    public GameObject Monster;
    [SerializeField] private string musicBusPath = "bus:/Music";
    [SerializeField] private string SFXBusPath = "bus:/SFX";
    private void OnTriggerEnter(Collider other)
    {
        Bus musicBus = RuntimeManager.GetBus(musicBusPath);
        musicBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
        Monster.SetActive(true);
    }
}
