using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trigger3 : MonoBehaviour
{
    [SerializeField] private Camera PlayerCam;
    [SerializeField] private GameObject Eyes;
    [SerializeField] private GameObject Screamer;
    [SerializeField] private LayerMask mask;
    private void OnCollisionStay(Collision collision)
    {
        RaycastHit hit;
        if (Physics.Raycast(PlayerCam.transform.position, PlayerCam.transform.forward, out hit, 50f, mask))
        {
            if(hit.transform.gameObject == Eyes)
            {
                Eyes.SetActive(false);
                Screamer.SetActive(true);
            }
        }
    }
}
