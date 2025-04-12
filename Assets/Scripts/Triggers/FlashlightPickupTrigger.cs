using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FMODUnity;
using TMPro;


public class FlashlightPickupTrigger : MonoBehaviour
{
    public FlashLight flashlight;
    public Door door;
    public GameObject KitchenDoor;
    public GameObject UIBatteryImage;
    public TMP_Text Dialogue;
    public FTUEPopUps ftuePopUps;

    [FMODUnity.EventRef]
    public string doorSoundEvent = "event:/triggers/E1FirstVariant";

    public void Start()
    {   
        UIBatteryImage.SetActive(false);
    }
    public void OnDestroy()
    {
        if (flashlight.isPickedUp)
        {
            if (UIBatteryImage != null)
            {
                UIBatteryImage.SetActive(true);
            }
            if(ftuePopUps != null)
            {
                ftuePopUps.DisplayTextManually(Dialogue, "Press F to use flashlight, You can collect batteries and use them in your inventory (TAB or I)");
            }
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
