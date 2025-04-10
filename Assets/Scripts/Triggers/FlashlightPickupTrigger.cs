using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FMODUnity;


public class FlashlightPickupTrigger : MonoBehaviour
{
    public FlashLight flashlight;
    public Door door;
    public GameObject KitchenDoor;

    [FMODUnity.EventRef]
    public string doorSoundEvent = "event:/triggers/E1FirstVariant";

    public void OnDestroy()
    {
        if (flashlight.isPickedUp)
        {
            door.isLocked = false;
            door = KitchenDoor.GetComponent<Door>();
            door.grab();
            door.ProcessMove();
            door.transform.DOLocalRotate(new Vector3(0, -35, 0), 1.0f);
            
            //play sound on door
            RuntimeManager.PlayOneShot(doorSoundEvent, door.transform.position);
        }
    }
}
