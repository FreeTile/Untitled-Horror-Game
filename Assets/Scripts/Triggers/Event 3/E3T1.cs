using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E3T1 : EventHandler
{

    public float explosionForce = 20f;
    public float explosionRadius = 5f;
    public float upwardModifier = 1f;

    public Transform explosionCenter;

    public GameObject plateDroppedSound;
    public GameObject[] kitchenProps;

    public override IEnumerator Event()
    {

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
        yield return base.Event();
    }
}
