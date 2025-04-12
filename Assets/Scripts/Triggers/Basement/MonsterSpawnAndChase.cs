using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class MonsterSpawnAndChase : EventHandler
{
    [SerializeField]
    GameObject Monster;
    [SerializeField] private string musicBusPath = "bus:/Music";
    [SerializeField] private string SFXBusPath = "bus:/SFX";

    public override IEnumerator Event()
    {
        Bus musicBus = RuntimeManager.GetBus(musicBusPath);
        musicBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
        Monster.SetActive(true);
        
        yield return null;
    }
}
