using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2T1 : MonoBehaviour
{
    [SerializeField] private GameObject monster;
    private void OnTriggerEnter()
    {
        monster.SetActive(true);
        Debug.Log("Monster appeared");
    }
}
