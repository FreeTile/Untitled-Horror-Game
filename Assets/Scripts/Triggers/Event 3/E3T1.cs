using System.Collections;
using UnityEngine;
using DG.Tweening;
using FMODUnity;
using FMOD.Studio;

public class E3T1 : EventHandler
{
    public float explosionForce = 20f;
    public float explosionRadius = 5f;
    public float upwardModifier = 1f;

    public Transform explosionCenter;
    public GameObject BathroomDoor;
    public Door door;
    public GameObject FridgeDoor;
    public GameObject FTUETriggerPuzzle2;

    public Light bathroomLight;
    public Color targetLightColor = Color.red;
    public float colorTransitionDuration = 1.0f;
    public float effectDuration = 20.0f;

    public int minSanityLoss = 10;
    public int maxSanityLoss = 20;

    private Color originalLightColor;

    public GameObject platesDroppedCenter;
    public GameObject[] kitchenProps;
    public ParticleSystem bloodParticles;

    public EventReference kitchenSounds;

    public override IEnumerator Event()
    {
        GameManager.Instance.sanityManager.CheckFearLevel(minSanityLoss, maxSanityLoss, 10f);
        if (bathroomLight != null)
        {
            originalLightColor = bathroomLight.color;
        }

        door = BathroomDoor.GetComponent<Door>();
        door.ProcessMove();
        BathroomDoor.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.2f);
        FridgeDoor.transform.DOLocalRotate(new Vector3(0, -60, 0), 0.1f);
        door.isLocked = true;

        if (bathroomLight != null)
        {
            bathroomLight.DOColor(targetLightColor, colorTransitionDuration);
        }

        if (bloodParticles != null)
        {
            bloodParticles.Play();
        }

        yield return new WaitForSeconds(effectDuration);

        if (bathroomLight != null)
        {
            bathroomLight.DOColor(originalLightColor, colorTransitionDuration);
        }

        if (bloodParticles != null)
        {
            bloodParticles.Stop();
        }

        foreach (GameObject prop in kitchenProps)
        {
            Rigidbody rb = prop.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, explosionCenter.position, explosionRadius, upwardModifier, ForceMode.Impulse);
            }
        }

        EventInstance kitchenSoundDrop = RuntimeManager.CreateInstance(kitchenSounds);

        RuntimeManager.AttachInstanceToGameObject(kitchenSoundDrop, platesDroppedCenter.transform, (Rigidbody)null);

        kitchenSoundDrop.start();
        kitchenSoundDrop.release();

        FTUETriggerPuzzle2.SetActive(true);
        door.isLocked = false;
        yield return base.Event();
    }
}
