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
    [SerializeField] private GameObject Trigger1;
    [SerializeField] private LayerMask mask;

    public override IEnumerator Event()
    {
        while (true)
        {

            RaycastHit hit;
            Physics.Raycast(PlayerCam.transform.position, (transform.position - PlayerCam.transform.position).normalized, out hit, 30f, mask);
            Debug.DrawRay(PlayerCam.transform.position, (transform.position - PlayerCam.transform.position).normalized, Color.green);
            Debug.Log(hit.collider);
            if (IsInCameraFrustum() && hit.collider != null && hit.transform.gameObject == transform.gameObject)
            {
                break;
            }
            else
            {
                yield return null;
            }
        }
        yield return new WaitForSeconds(0.3f);

        Debug.Log("DemDalsh");

        Debug.Log("Screamer");
        GameManager.Instance.TriggerJumpScare();
        //Decrease Sanity here

        
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

        Trigger1.SetActive(false);
    }

    private bool IsInCameraFrustum()
    {
        Vector3 viewportPos = PlayerCam.WorldToViewportPoint(transform.position);
        return viewportPos.z > 0 &&
               viewportPos.x > 0 && viewportPos.x < 1 &&
               viewportPos.y > 0 && viewportPos.y < 1;
    }
}
