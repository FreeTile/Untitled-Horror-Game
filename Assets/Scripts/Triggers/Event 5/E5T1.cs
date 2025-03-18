using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E5T1 : MonoBehaviour
{
    [SerializeField]
    GameObject Trigger;
    private void OnDisable()
    {
        Trigger.SetActive(true);
    }
}
