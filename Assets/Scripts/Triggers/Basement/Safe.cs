using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class Safe : MonoBehaviour
{
    [SerializeField] private string musicBusPath = "bus:/Music";
    [SerializeField] private GameObject Basement;
    [SerializeField] private GameObject LongBasement;
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Shelf;
    [SerializeField] private GameObject Monster;
    [SerializeField] private GameObject EventGameOver;

    void Update()
    {
        if(GetComponent<SpringJoint>() != null)
        {
            ChangeBasement();
        }
    }

    void ChangeBasement()
    {
        EventGameOver.SetActive(true);
        Bus musicBus = RuntimeManager.GetBus(musicBusPath);
        musicBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
        Monster.SetActive(false);
        //Disable longbasement
        LongBasement.SetActive(false);
        //enable basement
        Basement.SetActive(true);
        //Player pos + 47.5m
        CharacterController controller = Player.GetComponent<CharacterController>();
        controller.enabled = false;
        Player.transform.position = Player.transform.position + new Vector3(0f, 0f, 45f);
        controller.enabled = true;
        //Safe pos + 47.5m
        Shelf.transform.position = Shelf.transform.position + new Vector3(0f, 0f, 45f);
        gameObject.GetComponent<Safe>().enabled = false;
    }
}
