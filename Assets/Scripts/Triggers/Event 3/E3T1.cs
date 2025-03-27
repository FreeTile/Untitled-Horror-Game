using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class E3T1 : EventHandler
{

    public float explosionForce = 20f;
    public float explosionRadius = 5f;
    public float upwardModifier = 1f;

    public Transform explosionCenter;

    public GameObject BathroomDoor;
    
    public Door door;

    public GameObject plateDroppedSound;
    public GameObject[] kitchenProps;

    public override IEnumerator Event()
    {
        door = BathroomDoor.GetComponent<Door>();
        door.ProcessMove();
        BathroomDoor.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.2f);
        
        door.isLocked = true;
        //Lock bathroom door, start scary sounds and flickering light 

        yield return new WaitForSeconds(2);
        foreach (GameObject prop in kitchenProps)
        {
            Rigidbody rb = prop.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, explosionCenter.position, explosionRadius, upwardModifier, ForceMode.Impulse);
            }
        }
        //playing sound
        plateDroppedSound.SetActive(false);
        //Something happened in the kitchen
        door.isLocked = false;
        yield return base.Event();

    }
}