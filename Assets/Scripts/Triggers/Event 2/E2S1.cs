using System.Collections;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using UnityEngine.UI;

public class E2S1 : EventHandler
{
    public GameObject lightBulb;
    public int minSanityLoss = 50;
    public int maxSanityLoss = 80;
    [SerializeField] private Camera PlayerCam;
    [SerializeField] private GameObject Monster;
    [SerializeField] private LayerMask mask;
    //private string E2S1BulbBreakSound = "event:/BulbSound";

    public override IEnumerator Event()
    {
        while (true)
        {
            RaycastHit hit;
            Physics.Raycast(PlayerCam.transform.position, (transform.position - PlayerCam.transform.position).normalized, out hit, 30f, mask);
            Debug.DrawRay(PlayerCam.transform.position, (transform.position - PlayerCam.transform.position).normalized, Color.green);
            if (IsInCameraFrustum() && hit.collider != null && hit.transform.gameObject == transform.gameObject)
            {
                break;
            }
            else
            {
                yield return null;
            }
        }

        Debug.Log("DemDalsh");

        Debug.Log("Screamer");
        GameManager.Instance.TriggerJumpScare();
        //Decrease Sanity here

        this.transform.parent.gameObject.SetActive(false);
        int sanityLoss = Random.Range(minSanityLoss, maxSanityLoss);
        GameManager.Instance.sanityManager.TriggerScreamer(sanityLoss);
        if (lightBulb != null)
        {
            lightBulb.SetActive(false);
        }
        else
        {
            Debug.LogWarning("No link to the light bulb!");
        }

        //EventInstance bulbBreak = RuntimeManager.CreateInstance(E2S1BulbBreakSound);
        //bulbBreak.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        //bulbBreak.start();
        //bulbBreak.release();
    }

    private bool IsInCameraFrustum()
    {
        Vector3 viewportPos = PlayerCam.WorldToViewportPoint(transform.position);
        return viewportPos.z > 0 &&
               viewportPos.x > 0 && viewportPos.x < 1 &&
               viewportPos.y > 0 && viewportPos.y < 1;
    }
}
