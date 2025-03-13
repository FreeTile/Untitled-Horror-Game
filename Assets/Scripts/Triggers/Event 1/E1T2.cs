using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1T2 : MonoBehaviour
{
    [SerializeField] private GameObject Trigger1, Trigger3;
    [SerializeField] private GameObject Eyes;

    private void OnTriggerEnter()
    {
        Debug.Log("Started Event 1");
        Trigger1.SetActive(true);
        Trigger3.SetActive(true);
        Eyes.SetActive(true);
    }
}
