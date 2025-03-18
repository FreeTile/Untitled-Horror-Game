using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger2 : MonoBehaviour
{
    [SerializeField] private GameObject Trigger1, Trigger3;
    [SerializeField] private GameObject Eyes;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Started Event 1");
        Trigger1.SetActive(true);
        Trigger3.SetActive(true);
        Eyes.SetActive(true);
    }
}
