using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2T2 : MonoBehaviour
{

    [SerializeField] private GameObject monster;
    private void OnTriggerEnter()
    {
        monster.SetActive(false);
        Debug.Log("Monster disappeared");

    }
}
